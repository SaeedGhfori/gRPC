using Grpc.Net.Client;
using grpcServer.Protos;

namespace WinFormsClient
{
    public partial class Form1 : Form
    {
        GrpcChannel channel;
        ProductService.ProductServiceClient client;
        public Form1()
        {
            InitializeComponent();
            channel = GrpcChannel.ForAddress("https://localhost:7164/");
            client = new ProductService.ProductServiceClient(channel);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var response = client.GetAllProduct(new RequestAllProduct
            {
                Page = 1,
                PageSize = 20
            });

            dataGridView1.DataSource = response.Items;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var response = client.AddNewProduct(new RequestAddProductDto
            {
                Brand = textBox1.Text,
                Name = textBox2.Text,
                Price = Convert.ToInt32(textBox3.Text), 
            });
            if (response.IsSuccess)
            {
                MessageBox.Show("اضافه شد");
            }
        }
    }
}
