using SalesManagerApp;
using System;
using System.Windows.Forms;
using KursachV6_WinForms;

namespace SalesManagerUI
{
    public partial class LoginForm : Form
    {
        private UserManager userManager; 

      
        public LoginForm(UserManager userManager)
        {
            InitializeComponent();
            this.userManager = userManager; 
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text; 
            string password = txtPassword.Text; 

           
            if (userManager.Authenticate(username, password, out string role))
            {
                MessageBox.Show($"Добро пожаловать, {username}!", "Успешный вход", MessageBoxButtons.OK, MessageBoxIcon.Information);

              
                var mainMenuForm = new MainMenuForm(role, userManager);
                this.Hide(); 
                mainMenuForm.ShowDialog(); 

                this.Close(); 
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

      
        private void lblError_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
