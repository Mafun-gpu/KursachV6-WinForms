using SalesManagerApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KursachV6_WinForms
{
    public partial class CreateOrderForm : Form
    {
        private OrderManager orderManager;
        private ClientManager clientManager;
        private ProductManager productManager;
        private Dictionary<int, int> orderProducts = new Dictionary<int, int>();

        public CreateOrderForm(OrderManager orderManager, ClientManager clientManager, ProductManager productManager)
        {
            InitializeComponent();
            this.orderManager = orderManager;
            this.clientManager = clientManager;
            this.productManager = productManager;
        }

        private void CreateOrderForm_Load(object sender, EventArgs e)
        {
           
            cmbClients.DataSource = clientManager.GetAllClients();
            cmbClients.DisplayMember = "FullName";
            cmbClients.ValueMember = "Id";

            cmbProducts.DataSource = productManager.GetAllProducts();
            cmbProducts.DisplayMember = "Name";
            cmbProducts.ValueMember = "Id";
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            int productId = (int)cmbProducts.SelectedValue;
            int quantity = (int)nudQuantity.Value;

            if (orderProducts.ContainsKey(productId))
            {
                orderProducts[productId] += quantity;
            }
            else
            {
                orderProducts[productId] = quantity;
            }

            UpdateOrderDetails();
        }

        private void UpdateOrderDetails()
        {
            lstOrderDetails.Items.Clear();
            foreach (var item in orderProducts)
            {
                var product = productManager.GetProductById(item.Key);
                lstOrderDetails.Items.Add($"{product.Name} - {item.Value} шт.");
            }
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            int clientId = (int)cmbClients.SelectedValue;

            try
            {
                orderManager.CreateOrder(clientId, orderProducts);
                MessageBox.Show("Заказ успешно создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
