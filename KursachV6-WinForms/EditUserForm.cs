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
            txtPassword.Text = ""; // Пароль не показывается
            cmbRole.SelectedItem = user.Role;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            user.Username = txtUsername.Text;
            user.PasswordHash = Utils.HashPassword(txtPassword.Text);
            user.Role = cmbRole.SelectedItem?.ToString();

            userManager.UpdateUser(user);
            MessageBox.Show("Пользователь успешно обновлён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
