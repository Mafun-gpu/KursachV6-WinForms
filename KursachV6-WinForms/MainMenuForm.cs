using SalesManagerApp;
using System;
using System.Windows.Forms;

namespace KursachV6_WinForms
{
    public partial class MainMenuForm : Form
    {
        private string userRole;
        private string currentUserRole;
        private ClientManager clientManager;
        private OrderManager orderManager;
        private ProductManager productManager;
        private UserManager userManager;

        public MainMenuForm(string role, UserManager manager)
        {
            InitializeComponent();

            userRole = role;

            clientManager = new ClientManager();
            productManager = new ProductManager();
            orderManager = new OrderManager(productManager);
            currentUserRole = role;
            userManager = manager;

            SetPermissions();
        }

        private void SetPermissions()
        {
           
            if (userRole == "manager")
            {
                btnReports.Enabled = false; 
            }
        }

        private void btnManageClients_Click(object sender, EventArgs e)
        {
            var clientForm = new ClientManagementForm(clientManager);
            clientForm.ShowDialog();
        }

        private void btnManageProducts_Click(object sender, EventArgs e)
        {
            var productForm = new ProductManagementForm();
            productForm.ShowDialog();
        }

        private void btnManageOrders_Click(object sender, EventArgs e)
        {
            var orderForm = new OrderManagementForm(productManager, clientManager);
            orderForm.ShowDialog();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            var reportForm = new ReportForm(orderManager, clientManager, productManager);
            reportForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Добро пожаловать! Ваша роль: {userRole}";
        }
    }
}
