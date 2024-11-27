using SalesManagerApp;
using System;
using System.Windows.Forms;
using KursachV6_WinForms;

namespace SalesManagerUI
{
    public partial class LoginForm : Form
    {
        private UserManager userManager; // Поле для хранения экземпляра UserManager

        // Конструктор LoginForm принимает UserManager
        public LoginForm(UserManager userManager)
        {
            InitializeComponent();
            this.userManager = userManager; // Инициализация UserManager
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text; // Получение имени пользователя из текстового поля
            string password = txtPassword.Text; // Получение пароля из текстового поля

            // Аутентификация пользователя через UserManager
            if (userManager.Authenticate(username, password, out string role))
            {
                MessageBox.Show($"Добро пожаловать, {username}!", "Успешный вход", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Переход на главную форму и передача роли и UserManager
                var mainMenuForm = new MainMenuForm(role, userManager);
                this.Hide(); // Скрытие текущей формы
                mainMenuForm.ShowDialog(); // Открытие главного меню

                this.Close(); // Закрываем текущую форму
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        // Дополнительный метод (если потребуется для отладки или UI)
        private void lblError_Click(object sender, EventArgs e)
        {
            // Обработчик событий для нажатия на lblError.
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
