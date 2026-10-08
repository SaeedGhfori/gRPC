using grpcServer.GRPC;
using grpcServer.Models;
using grpcServer.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddSingleton<IProductRepository, ProductRepository>();


var app = builder.Build();

app.MapGrpcReflectionService();
app.MapGrpcService<ProductgRPCService>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
