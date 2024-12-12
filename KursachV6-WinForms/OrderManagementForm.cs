using SalesManagerApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursachV6_WinForms
{
    public partial class OrderManagementForm : Form
    {
        private OrderManager orderManager;
        private ProductManager productManager;
        private ClientManager clientManager;
        private System.Windows.Forms.Button btnViewOrder;


        public OrderManagementForm(ProductManager productManager, ClientManager clientManager)
        {
            InitializeComponent();
            this.productManager = productManager;
            this.clientManager = clientManager;
            this.orderManager = new OrderManager(productManager);
            LoadOrders();
            dgvOrders.CellClick += dgvOrders_CellClick; 
        }
        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                dgvOrders.ClearSelection(); 
                dgvOrders.Rows[e.RowIndex].Selected = true; 
            }
        }
        private void OrderManagementForm_Load(object sender, EventArgs e)
        {
            dgvOrders.AllowUserToAddRows = false;
            LoadOrders();
        }

        private void LoadOrders()
        {
            dgvOrders.Rows.Clear();

            if (dgvOrders.Columns.Count == 0)
            {
                dgvOrders.Columns.Add("OrderId", "ID заказа");
                dgvOrders.Columns.Add("ClientName", "Клиент");
                dgvOrders.Columns.Add("OrderDate", "Дата заказа");
                dgvOrders.Columns.Add("TotalPrice", "Сумма");
                dgvOrders.Columns.Add("Status", "Статус");
            }

            var orders = orderManager.GetAllOrders();
            foreach (var order in orders)
            {
                var client = clientManager.GetClientById(order.ClientId);
                string clientName = client != null ? client.FullName : "Неизвестно";
                dgvOrders.Rows.Add(order.OrderId, clientName, order.OrderDate.ToString("dd.MM.yyyy"), order.TotalPrice.ToString("C"), order.Status);
            }
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            var createOrderForm = new CreateOrderForm(orderManager, clientManager, productManager);
            createOrderForm.FormClosed += (s, args) => { LoadOrders(); };
            createOrderForm.ShowDialog();
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells[0].Value);
                var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить выбранный заказ?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    orderManager.DeleteOrder(orderId);
                    LoadOrders();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnChangeStatus_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                int orderId = (int)dgvOrders.SelectedRows[0].Cells[0].Value;
                var order = orderManager.GetOrderById(orderId);

                if (order != null)
                {
                    string[] statuses = { "Создан", "В процессе", "Завершен", "Отменен" };
                    string newStatus = ShowStatusSelectionDialog(order.Status, statuses);

                    if (!string.IsNullOrEmpty(newStatus))
                    {
                        order.Status = newStatus;
                        orderManager.UpdateOrder(order);
                        LoadOrders();
                    }
                }
                else
                {
                    MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для изменения статуса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.ToLower();
            var filteredOrders = orderManager.GetAllOrders().Where(o =>
                o.OrderId.ToString().Contains(query) ||
                o.ClientId.ToString().Contains(query) ||
                clientManager.GetClientById(o.ClientId)?.FullName.ToLower().Contains(query) == true ||
                o.Status.ToLower().Contains(query) ||
                o.OrderDate.ToString("dd.MM.yyyy").Contains(query)
            ).ToList();

            dgvOrders.Rows.Clear();
            foreach (var order in filteredOrders)
            {
                var client = clientManager.GetClientById(order.ClientId);
                string clientName = client != null ? client.FullName : "Неизвестно";
                dgvOrders.Rows.Add(order.OrderId, clientName, order.OrderDate.ToString("dd.MM.yyyy"), order.TotalPrice, order.Status);
            }
        }

        private void btnResetSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadOrders();  // После сброса поиска загружаем все заказы
        }

        private string ShowStatusSelectionDialog(string currentStatus, string[] statuses)
        {
            using (var form = new Form())
            {
                form.Text = "Изменение статуса";
                form.Size = new Size(300, 200);

                var label = new Label { Text = "Выберите новый статус:", Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };
                var comboBox = new ComboBox { DataSource = statuses, Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList };
                comboBox.SelectedItem = currentStatus;

                var buttonOk = new Button { Text = "ОК", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom };
                var buttonCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, Dock = DockStyle.Bottom };

                form.Controls.Add(label);
                form.Controls.Add(comboBox);
                form.Controls.Add(buttonOk);
                form.Controls.Add(buttonCancel);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    return comboBox.SelectedItem.ToString();
                }

                return null;
            }
        }


        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells[0].Value);
                var order = orderManager.GetOrderById(orderId);

                if (order != null)
                {
                  
                    string orderDetails = $"Заказ №{order.OrderId}\n" +
                                          $"Клиент: {clientManager.GetClientById(order.ClientId)?.FullName}\n" +
                                          $"Дата: {order.OrderDate.ToString("dd.MM.yyyy")}\n" +
                                          $"Статус: {order.Status}\n" +
                                          $"Сумма: {order.TotalPrice:C}\n" +
                                          "Товары:\n";
                    foreach (var product in order.Products)
                    {
                     
                        var productName = productManager.GetProductById(product.Key)?.Name ?? "Неизвестно";
                        orderDetails += $"{productName} x {product.Value}\n";
                    }

                
                    MessageBox.Show(orderDetails, "Детали заказа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для просмотра.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
