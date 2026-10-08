using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using grpcServer.Protos;
using WinFormsClient.Interceptors;

namespace WinFormsClient
{
    public partial class Form1 : Form
    {
        private const string DefaultAddress = "https://localhost:7164/";

        private static readonly Color ColorConnected = Color.FromArgb(22, 163, 74);
        private static readonly Color ColorError = Color.FromArgb(220, 38, 38);
        private static readonly Color ColorSuccess = Color.FromArgb(74, 222, 128);
        private static readonly Color ColorErrorLog = Color.FromArgb(248, 113, 113);
        private static readonly Color ColorInfo = Color.FromArgb(125, 211, 252);
        private static readonly Color ColorMuted = Color.FromArgb(148, 163, 184);

        private GrpcChannel? _channel;
        private ProductService.ProductServiceClient? _client;
        private Button[] _actions = [];

        public Form1()
        {
            InitializeComponent();
            _actions = [btnAdd, btnUpdate, btnGetById, btnDelete, btnStreamAll, btnPaged, btnConnect];
            StyleGrid();
            Log("برنامه راه‌اندازی شد.", ColorMuted);
        }

        private ProductService.ProductServiceClient Client =>
            _client ?? throw new InvalidOperationException("اتصال برقرار نشده است.");

        // ------------------------- اتصال -------------------------
        private async void Form1_Load(object sender, EventArgs e)
        {
            Connect(txtAddress.Text.Trim());
            await RunAsync("دریافت اولیه", () => LoadPagedAsync(1, 20));
        }

        private void Connect(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                Log("آدرس سرور خالی است.", ColorErrorLog);
                return;
            }

            _channel?.Dispose();
            var interceptor = new WinFormsClient.Interceptors.ClientErrorInterceptor();
            _channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Interceptor = interceptor
            });
            _client = new ProductService.ProductServiceClient(_channel, interceptor);
            SetStatus("● متصل", ColorConnected);
            Log($"اتصال به {address} برقرار شد.", ColorMuted);
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            Connect(txtAddress.Text.Trim());
            await RunAsync("بررسی اتصال", async () =>
            {
                await Client.GetAllAsync(new RequestAllProduct { Page = 1, PageSize = 1 });
                Log("ارتباط با سرور تایید شد.", ColorSuccess);
            });
        }

        // ------------------------- افزودن گروهی (Bidirectional) -------------------------
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ReadNamePrice(out var name, out var price))
                return;

            var count = (int)nudCount.Value;
            await RunAsync("افزودن", async () =>
            {
                using var call = Client.AddProduct();

                // خواندن پاسخ‌ها به صورت همزمان
                var readTask = Task.Run(async () =>
                {
                    var list = new List<ProductReply>();
                    await foreach (var item in call.ResponseStream.ReadAllAsync())
                        list.Add(item);
                    return list;
                });

                for (var i = 1; i <= count; i++)
                {
                    var productName = count == 1 ? name : $"{name} {i}";
                    await call.RequestStream.WriteAsync(new ProductRequest
                    {
                        Name = productName,
                        Price = price
                    });
                    Log($"ارسال: {productName} - {price:N0}", ColorMuted);
                }

                await call.RequestStream.CompleteAsync();
                var results = await readTask;

                foreach (var product in results)
                {
                    AddRow(product);
                    Log($"+ #{product.Id} «{product.Name}» با قیمت {product.Price:N0} اضافه شد.", ColorSuccess);
                }

                UpdateTotal();
                Log($"{results.Count} محصول با Bidirectional Stream اضافه شد.", ColorInfo);
            });
        }

        // ------------------------- ویرایش (Unary) -------------------------
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ReadId(out var id) || !ReadNamePrice(out var name, out var price))
                return;

            await RunAsync("ویرایش", async () =>
            {
                var product = await Client.UpdateProductAsync(new ProductRequest
                {
                    Id = id,
                    Name = name,
                    Price = price
                });

                UpdateRowById(product);
                Log($"#{product.Id} «{product.Name}» به قیمت {product.Price:N0} ویرایش شد (Unary).", ColorSuccess);
            });
        }

        // ------------------------- دریافت با شناسه (Unary) -------------------------
        private async void btnGetById_Click(object sender, EventArgs e)
        {
            if (!ReadId(out var id))
                return;

            await RunAsync("دریافت محصول", async () =>
            {
                var product = await Client.GetProductByIdAsync(new ProductByIdRequest { Id = id });

                txtId.Text = product.Id.ToString();
                txtName.Text = product.Name;
                txtPrice.Text = product.Price.ToString();
                UpdateRowById(product);
                HighlightRow(product.Id);

                Log($"#{product.Id} «{product.Name}» با قیمت {product.Price:N0} دریافت شد (Unary).", ColorInfo);
            });
        }

        // ------------------------- حذف (Client Stream) -------------------------
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            var ids = CollectDeleteIds();
            if (ids.Count == 0)
            {
                Log("برای حذف، ابتدا ردیف(ها) را در جدول انتخاب کنید یا شناسه وارد کنید.", ColorErrorLog);
                return;
            }

            var confirm = MessageBox.Show(
                $"{ids.Count} محصول حذف شود؟",
                "تایید حذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            await RunAsync("حذف", async () =>
            {
                using var call = Client.DeleteProduct();

                foreach (var id in ids)
                    await call.RequestStream.WriteAsync(new ProductByIdRequest { Id = id });

                await call.RequestStream.CompleteAsync();
                await call.ResponseAsync;

                RemoveRows(ids);
                UpdateTotal();
                Log($"{ids.Count} محصول با Client Stream حذف شد.", ColorErrorLog);
            });
        }

        // ------------------------- دریافت همه (Server Stream) -------------------------
        private async void btnStreamAll_Click(object sender, EventArgs e)
        {
            await RunAsync("دریافت همه", async () =>
            {
                using var call = Client.GetAllProduct(new Empty());

                dataGridView1.Rows.Clear();
                var count = 0;
                await foreach (var product in call.ResponseStream.ReadAllAsync())
                {
                    AddRow(product);
                    count++;
                }

                UpdateTotal();
                Log($"{count} محصول با Server Stream دریافت شد.", ColorInfo);
            });
        }

        // ------------------------- دریافت صفحه‌ای (Unary) -------------------------
        private async void btnPaged_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPage.Text.Trim(), out var page) || page < 1)
            {
                Log("شماره صفحه معتبر نیست.", ColorErrorLog);
                return;
            }
            if (!int.TryParse(txtPageSize.Text.Trim(), out var pageSize) || pageSize < 1)
            {
                Log("اندازه صفحه معتبر نیست.", ColorErrorLog);
                return;
            }

            await RunAsync("دریافت صفحه", () => LoadPagedAsync(page, pageSize));
        }

        private async Task LoadPagedAsync(int page, int pageSize)
        {
            var response = await Client.GetAllAsync(new RequestAllProduct
            {
                Page = page,
                PageSize = pageSize
            });

            dataGridView1.Rows.Clear();
            foreach (var product in response.Items)
                AddRow(product);

            UpdateTotal();
            Log($"صفحه {page} با {response.Items.Count} مورد دریافت شد (Unary).", ColorInfo);
        }

        // ------------------------- انتخاب ردیف → پر کردن فیلدها -------------------------
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count)
                return;

            var row = dataGridView1.Rows[e.RowIndex];
            txtId.Text = Convert.ToString(row.Cells["colId"].Value);
            txtName.Text = Convert.ToString(row.Cells["colName"].Value);
            txtPrice.Text = Convert.ToString(row.Cells["colPrice"].Value);
        }

        // ------------------------- ابزارها -------------------------
        private bool ReadId(out int id)
        {
            if (int.TryParse(txtId.Text.Trim(), out id))
                return true;

            Log("شناسه معتبر نیست.", ColorErrorLog);
            id = 0;
            return false;
        }

        private bool ReadNamePrice(out string name, out int price)
        {
            name = txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                Log("نام محصول خالی است.", ColorErrorLog);
                price = 0;
                return false;
            }
            if (!int.TryParse(txtPrice.Text.Trim(), out price) || price < 0)
            {
                Log("قیمت معتبر نیست.", ColorErrorLog);
                price = 0;
                return false;
            }
            return true;
        }

        private List<int> CollectDeleteIds()
        {
            var ids = new List<int>();

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (TryGetCellId(row, out var id))
                    ids.Add(id);
            }

            if (ids.Count == 0 && ReadSilentId(out var singleId))
                ids.Add(singleId);

            return ids.Distinct().ToList();
        }

        private bool ReadSilentId(out int id) =>
            int.TryParse(txtId.Text.Trim(), out id);

        private void AddRow(ProductReply product) =>
            dataGridView1.Rows.Add(product.Id, product.Name, product.Price);

        private void UpdateRowById(ProductReply product)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (TryGetCellId(row, out var id) && id == product.Id)
                {
                    row.Cells["colName"].Value = product.Name;
                    row.Cells["colPrice"].Value = product.Price;
                    return;
                }
            }
            AddRow(product);
            UpdateTotal();
        }

        private void RemoveRows(List<int> ids)
        {
            for (var i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                var row = dataGridView1.Rows[i];
                if (TryGetCellId(row, out var id) && ids.Contains(id))
                    dataGridView1.Rows.RemoveAt(i);
            }
        }

        private static bool TryGetCellId(DataGridViewRow row, out int id)
        {
            id = 0;
            var value = row.Cells["colId"]?.Value;
            return value is not null && int.TryParse(value.ToString(), out id);
        }

        private void HighlightRow(int id)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (TryGetCellId(row, out var rowId) && rowId == id)
                {
                    row.Selected = true;
                    dataGridView1.CurrentCell = row.Cells["colId"];
                    return;
                }
            }
        }

        private void UpdateTotal() =>
            lblTotal.Text = $"تعداد: {dataGridView1.Rows.Count}";

        private void StyleGrid()
        {
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dataGridView1.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        }

        private void SetStatus(string text, Color color)
        {
            statusPill.CardColor = color;
            lblStatusText.Text = text;
            statusPill.Invalidate();
        }

        private void SetBusy(bool busy)
        {
            foreach (var button in _actions)
                button.Enabled = !busy;
            UseWaitCursor = busy;
        }

        private async Task RunAsync(string opName, Func<Task> action)
        {
            SetBusy(true);
            try
            {
                await action();
            }
            catch (RpcException ex)
            {
                SetStatus("● خطا", ColorError);
                var machineCode = ex.Trailers.GetValue("x-error-code");
                var detail = string.IsNullOrEmpty(machineCode)
                    ? $"{opName} ناموفق [{ex.StatusCode}]: {ex.Status.Detail}"
                    : $"{opName} ناموفق [{ex.StatusCode}/{machineCode}]: {ex.Status.Detail}";
                Log(detail, ColorErrorLog);
            }
            catch (Exception ex)
            {
                SetStatus("● خطا", ColorError);
                Log($"{opName} خطا: {ex.Message}", ColorErrorLog);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void Log(string message, Color color)
        {
            if (txtLog.IsDisposed)
                return;

            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.SelectionLength = 0;
            txtLog.SelectionColor = color;
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            txtLog.SelectionColor = ColorMuted;
            txtLog.ScrollToCaret();
        }
    }
}
