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

        public OrderManagementForm(ProductManager productManager, ClientManager clientManager)
        {
            InitializeComponent();
            this.productManager = productManager;
            this.clientManager = clientManager;
            this.orderManager = new OrderManager(productManager);
            LoadOrders();
        }



        private void LoadOrders()
        {
            dgvOrders.Rows.Clear(); // Очищаем старые строки

            // Проверяем, есть ли уже столбцы в DataGridView
            if (dgvOrders.Columns.Count == 0)
            {
                dgvOrders.Columns.Add("OrderId", "ID заказа");
                dgvOrders.Columns.Add("ClientId", "ID клиента");
                dgvOrders.Columns.Add("OrderDate", "Дата заказа");
                dgvOrders.Columns.Add("TotalPrice", "Сумма");
                dgvOrders.Columns.Add("Status", "Статус");
            }

            // Загружаем данные из OrderManager
            var orders = orderManager.GetAllOrders();

            foreach (var order in orders)
            {
                dgvOrders.Rows.Add(order.OrderId, order.ClientId, order.OrderDate.ToString("dd.MM.yyyy"), order.TotalPrice.ToString("C"), order.Status);
            }
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            var createOrderForm = new CreateOrderForm(orderManager, clientManager, productManager);

            createOrderForm.FormClosed += (s, args) =>
            {
                // Обновляем таблицу заказов после закрытия окна создания заказа
                LoadOrders();
            };

            createOrderForm.ShowDialog();
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0) // Проверяем, что выбрана хотя бы одна строка
            {
                // Получаем ID заказа из первой колонки выбранной строки
                int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells[0].Value);

                // Подтверждение удаления
                var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить выбранный заказ?",
                                                    "Подтверждение удаления",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    // Удаляем заказ
                    orderManager.DeleteOrder(orderId);

                    // Обновляем таблицу заказов
                    LoadOrders();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void OrderManagementForm_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.ToLower();

            // Фильтруем заказы
            var filteredOrders = orderManager.GetAllOrders().Where(o =>
                o.OrderId.ToString().Contains(query) ||
                o.ClientId.ToString().Contains(query) ||
                o.Status.ToLower().Contains(query) ||
                o.OrderDate.ToString("dd.MM.yyyy").Contains(query)
            ).ToList();

            // Очищаем текущие строки в DataGridView
            dgvOrders.Rows.Clear();

            // Заполняем таблицу отфильтрованными данными
            foreach (var order in filteredOrders)
            {
                dgvOrders.Rows.Add(
                    order.OrderId,
                    order.ClientId,
                    order.OrderDate.ToString("dd.MM.yyyy"),
                    order.TotalPrice,
                    order.Status
                );
            }
        }

        private void btnResetSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadOrders(); // Перезагружаем полный список заказов
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
                        orderManager.UpdateOrder(order); // Предполагается, что этот метод сохраняет изменения
                        LoadOrders(); // Обновляем таблицу заказов
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
    }
}
