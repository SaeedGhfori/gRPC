using System.Text;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using grpcServer.Protos;
using ConsoleClient.Interceptors;

Console.OutputEncoding = Encoding.UTF8;

const string serverAddress = "https://localhost:7164/";

using var interceptor = new ClientErrorInterceptor();
var channel = GrpcChannel.ForAddress(serverAddress, new GrpcChannelOptions
{
    Interceptor = interceptor
});
var client = new ProductService.ProductServiceClient(channel, interceptor);

Console.WriteLine("==============================================");
Console.WriteLine("   کلاینت کنسولی gRPC - مدیریت محصولات");
Console.WriteLine($"   سرور: {serverAddress}");
Console.WriteLine("==============================================");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("[1] افزودن محصولات            (Bidirectional Stream)");
    Console.WriteLine("[2] ویرایش محصول                (Unary)");
    Console.WriteLine("[3] دریافت محصول با شناسه       (Unary)");
    Console.WriteLine("[4] حذف چند محصول               (Client Stream)");
    Console.WriteLine("[5] دریافت همه محصولات          (Server Stream)");
    Console.WriteLine("[6] دریافت صفحه‌بندی‌شده        (Unary)");
    Console.WriteLine("[0] خروج");
    Console.Write("انتخاب شما: ");
    var choice = Console.ReadLine()?.Trim();
    if (choice is null)
        return;

    try
    {
        switch (choice)
        {
            case "1": await AddProductsAsync(client); break;
            case "2": await UpdateProductAsync(client); break;
            case "3": await GetProductByIdAsync(client); break;
            case "4": await DeleteProductsAsync(client); break;
            case "5": await GetAllProductsStreamAsync(client); break;
            case "6": await GetAllPagedAsync(client); break;
            case "0": return;
            default: Console.WriteLine("گزینه نامعتبر است."); break;
        }
    }
    catch (RpcException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        var machineCode = errorInterceptor.LastErrorCode ?? ex.Trailers.GetValue("x-error-code");
        Console.WriteLine($"خطای gRPC [{ex.StatusCode}]: {ex.Status.Detail}");
        if (!string.IsNullOrEmpty(machineCode))
            Console.WriteLine($"  کد ماشینی (اینترسپتور فرانت): {machineCode}");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"خطا: {ex.Message}");
        Console.ResetColor();
    }
}

// ------------------------- افزودن گروهی (Bidirectional) -------------------------
static async Task AddProductsAsync(ProductService.ProductServiceClient client)
{
    Console.WriteLine("نام و قیمت را با کاما وارد کنید (مثلاً: laptop,250000). خط خالی = پایان:");

    using var call = client.AddProduct();

    // خواندن پاسخ‌ها به صورت همزمان (دوطرفه بودن واقعی)
    var readTask = Task.Run(async () =>
    {
        var added = new List<ProductReply>();
        await foreach (var reply in call.ResponseStream.ReadAllAsync())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  + ");
            PrintProduct(reply);
            Console.ResetColor();
            added.Add(reply);
        }
        return added;
    });

    while (true)
    {
        Console.Write("> ");
        var line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
            break;

        var parts = line.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]) || !int.TryParse(parts[1], out var price))
        {
            Console.WriteLine("  ورودی نامعتبر است. قالب: نام,قیمت");
            continue;
        }

        await call.RequestStream.WriteAsync(new ProductRequest
        {
            Name = parts[0],
            Price = price
        });
    }

    await call.RequestStream.CompleteAsync();
    var results = await readTask;
    Console.WriteLine($"افزودن کامل شد. {results.Count} محصول اضافه شد.");
}

// ------------------------- ویرایش (Unary) -------------------------
static async Task UpdateProductAsync(ProductService.ProductServiceClient client)
{
    var id = ReadInt("شناسه: ");
    Console.Write("نام جدید: ");
    var name = Console.ReadLine()?.Trim() ?? string.Empty;
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("نام خالی است.");
        return;
    }
    var price = ReadInt("قیمت جدید: ");

    var reply = await client.UpdateProductAsync(new ProductRequest
    {
        Id = id,
        Name = name,
        Price = price
    });

    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("  ویرایش شد: ");
    PrintProduct(reply);
    Console.ResetColor();
}

// ------------------------- دریافت با شناسه (Unary) -------------------------
static async Task GetProductByIdAsync(ProductService.ProductServiceClient client)
{
    var id = ReadInt("شناسه: ");

    var reply = await client.GetProductByIdAsync(new ProductByIdRequest { Id = id });
    Console.Write("  نتیجه: ");
    PrintProduct(reply);
}

// ------------------------- حذف چندتایی (Client Stream) -------------------------
static async Task DeleteProductsAsync(ProductService.ProductServiceClient client)
{
    Console.Write("شناسه‌ها را با فاصله یا کاما وارد کنید (مثلاً: 3,7,12): ");
    var raw = Console.ReadLine() ?? string.Empty;
    var ids = raw
        .Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(s => int.TryParse(s, out var id) ? id : -1)
        .Where(id => id >= 0)
        .Distinct()
        .ToList();

    if (ids.Count == 0)
    {
        Console.WriteLine("شناسه‌ای وارد نشد.");
        return;
    }

    using var call = client.DeleteProduct();

    foreach (var id in ids)
        await call.RequestStream.WriteAsync(new ProductByIdRequest { Id = id });

    await call.RequestStream.CompleteAsync();
    await call.ResponseAsync;

    Console.WriteLine($"{ids.Count} مورد حذف شد.");
}

// ------------------------- دریافت همه (Server Stream) -------------------------
static async Task GetAllProductsStreamAsync(ProductService.ProductServiceClient client)
{
    using var call = client.GetAllProduct(new Empty());

    PrintHeader();
    var count = 0;
    await foreach (var product in call.ResponseStream.ReadAllAsync())
    {
        PrintProduct(product);
        count++;
    }
    Console.WriteLine($"جمع: {count} مورد.");
}

// ------------------------- دریافت صفحه‌بندی (Unary) -------------------------
static async Task GetAllPagedAsync(ProductService.ProductServiceClient client)
{
    var page = ReadInt("شماره صفحه: ");
    var pageSize = ReadInt("اندازه صفحه: ");
    if (page < 1) page = 1;
    if (pageSize < 1) pageSize = 20;

    var response = await client.GetAllAsync(new RequestAllProduct
    {
        Page = page,
        PageSize = pageSize
    });

    PrintHeader();
    foreach (var product in response.Items)
        PrintProduct(product);
    Console.WriteLine($"صفحه {page} - {response.Items.Count} مورد.");
}

// ------------------------- ابزارها -------------------------
static void PrintHeader()
{
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("  شناسه    |  نام                 |  قیمت");
    Console.WriteLine("--------------------------------------------------");
}

static void PrintProduct(ProductReply p)
{
    Console.WriteLine($"  {p.Id,-9} {p.Name,-20} {p.Price:N0}");
}

static int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        if (input is null)
            return 0;
        if (int.TryParse(input.Trim(), out var value))
            return value;
        Console.WriteLine("  عدد معتبر وارد کنید.");
    }
}
