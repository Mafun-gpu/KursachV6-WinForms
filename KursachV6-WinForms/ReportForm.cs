using SalesManagerApp;
using System;
using System.Linq;
using System.Windows.Forms;

namespace KursachV6_WinForms
{
    public partial class ReportForm : Form
    {
        private OrderManager orderManager;
        private ClientManager clientManager;
        private ProductManager productManager;

        public ReportForm(OrderManager orderManager, ClientManager clientManager, ProductManager productManager)
        {
            InitializeComponent();
            this.orderManager = orderManager;
            this.clientManager = clientManager;
            this.productManager = productManager;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите тип отчёта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string reportType = cmbReportType.SelectedItem.ToString();
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            switch (reportType)
            {
                case "Продажи за период":
                    dgvReport.DataSource = null; 
                    dgvReport.Columns.Clear();  

                    dgvReport.Columns.Add("OrderId", "Номер заказа");
                    dgvReport.Columns.Add("ClientId", "ID клиента");
                    dgvReport.Columns.Add("OrderDate", "Дата заказа");
                    dgvReport.Columns.Add("TotalPrice", "Сумма");
                    dgvReport.Columns.Add("Status", "Статус");

                    var ordersByPeriod = orderManager.GetOrdersByPeriod(startDate, endDate)
                        .Select(o => new
                        {
                            OrderId = o.OrderId,
                            ClientId = o.ClientId,
                            OrderDate = o.OrderDate.ToString("dd.MM.yyyy"),
                            TotalPrice = o.TotalPrice.ToString("C"),
                            Status = o.Status
                        })
                        .ToList();

                    dgvReport.Rows.Clear();
                    foreach (var order in ordersByPeriod)
                    {
                        dgvReport.Rows.Add(order.OrderId, order.ClientId, order.OrderDate, order.TotalPrice, order.Status);
                    }
                    break;

                case "Активность клиентов":
                    GenerateClientActivityReport();
                    break;

                case "Популярные товары":
                    GeneratePopularProductsReport();
                    break;

                default:
                    MessageBox.Show("Неизвестный тип отчёта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void GenerateClientActivityReport()
        {
            dgvReport.DataSource = null; 

            var clientActivities = orderManager.GetAllOrders()
                .GroupBy(o => o.ClientId)
                .Select(group => new
                {
                    ClientId = group.Key,
                    ClientName = clientManager.GetClientById(group.Key)?.FullName ?? "Неизвестный клиент",
                    TotalOrders = group.Count(),
                    TotalAmount = group.Sum(o => o.TotalPrice)
                })
                .OrderByDescending(c => c.TotalAmount)
                .ToList();

           
            if (!clientActivities.Any())
            {
                MessageBox.Show("Нет данных для отображения отчёта по активности клиентов.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

         
            dgvReport.Columns.Clear();
            dgvReport.Columns.Add("ClientId", "ID клиента");
            dgvReport.Columns.Add("ClientName", "Имя клиента");
            dgvReport.Columns.Add("TotalOrders", "Количество заказов");
            dgvReport.Columns.Add("TotalAmount", "Общая сумма");

            dgvReport.Rows.Clear();
            foreach (var activity in clientActivities)
            {
                dgvReport.Rows.Add(activity.ClientId, activity.ClientName, activity.TotalOrders, activity.TotalAmount.ToString("C"));
            }
        }

        private void GeneratePopularProductsReport()
        {
            dgvReport.DataSource = null; 

            var popularProducts = orderManager.GetAllOrders()
                .SelectMany(o => o.Products) 
                .GroupBy(p => p.Key) 
                .Select(group => new
                {
                    ProductId = group.Key,
                    ProductName = productManager.GetProductById(group.Key)?.Name ?? "Неизвестный товар",
                    QuantitySold = group.Sum(p => p.Value),
                    TotalRevenue = group.Sum(p => productManager.GetProductById(p.Key)?.Price * p.Value ?? 0)
                })
                .OrderByDescending(p => p.QuantitySold) 
                .ToList();

           
            dgvReport.Columns.Clear();
            dgvReport.Columns.Add("ProductId", "ID товара");
            dgvReport.Columns.Add("ProductName", "Название товара");
            dgvReport.Columns.Add("QuantitySold", "Проданное количество");
            dgvReport.Columns.Add("TotalRevenue", "Общая выручка");

            
            dgvReport.Rows.Clear();
            foreach (var product in popularProducts)
            {
                dgvReport.Rows.Add(product.ProductId, product.ProductName, product.QuantitySold, product.TotalRevenue.ToString("C"));
            }
        }
    }
}
