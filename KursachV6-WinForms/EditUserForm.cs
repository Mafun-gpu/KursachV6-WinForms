using SalesManagerApp;
using System.Windows.Forms;
using System;

namespace KursachV6_WinForms
{
    public partial class EditUserForm : Form
    {
        private UserManager userManager;
        private User user;

        public EditUserForm(UserManager userManager, User user)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.user = user;
            LoadUserData();
        }

        private void LoadUserData()
        {
            txtUsername.Text = user.Username;
            txtPassword.Text = "";
            cmbRole.SelectedItem = user.Role;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Имя пользователя не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пароль не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль пользователя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            user.Username = username;
            user.PasswordHash = Utils.HashPassword(password);
            user.Role = cmbRole.SelectedItem?.ToString();

            userManager.UpdateUser(user);
            MessageBox.Show("Пользователь успешно обновлён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
