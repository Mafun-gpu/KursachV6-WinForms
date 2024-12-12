using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KursachV6_WinForms
{
    public partial class ClientDetailsForm : Form
    {
        public string FullName { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public string Notes { get; private set; }

        public ClientDetailsForm(string title, Client existingClient = null)
        {
            InitializeComponent();

            this.Text = title;

            if (existingClient != null)
            {
                txtFullName.Text = existingClient.FullName;
                txtPhone.Text = existingClient.Phone;
                txtEmail.Text = existingClient.Email;
                txtAddress.Text = existingClient.Address;
                txtNotes.Text = existingClient.Notes;
            }

            txtPhone.TextChanged += TxtPhone_TextChanged;
            txtEmail.TextChanged += TxtEmail_TextChanged;
            txtFullName.KeyPress += TxtFullName_KeyPress;
        }

        private void TxtPhone_TextChanged(object sender, EventArgs e)
        {
            if (!IsValidPhone(txtPhone.Text))
            {
                errorProvider.SetError(txtPhone, "Неверный формат телефона");
            }
            else
            {
                errorProvider.SetError(txtPhone, string.Empty);
            }
        }

        private void TxtEmail_TextChanged(object sender, EventArgs e)
        {
            if (!IsValidEmail(txtEmail.Text))
            {
                errorProvider.SetError(txtEmail, "Неверный формат email");
            }
            else
            {
                errorProvider.SetError(txtEmail, string.Empty);
            }
        }

        private void TxtFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Блокируем ввод
                errorProvider.SetError(txtFullName, "Имя должно содержать только буквы");
            }
            else
            {
                errorProvider.SetError(txtFullName, string.Empty);
            }
        }

        private bool IsValidPhone(string phone)
        {
            var phoneRegex = new Regex(@"^\+?[0-9]{10,15}$");
            return phoneRegex.IsMatch(phone);
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@]+@[^@]+\.[^@]+$");
            return emailRegex.IsMatch(email);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Поле Ф.И.О. и Телефон обязательны для заполнения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidPhone(txtPhone.Text))
            {
                MessageBox.Show("Неверный формат телефона!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Неверный формат email!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FullName = txtFullName.Text;
            Phone = txtPhone.Text;
            Email = txtEmail.Text;
            Address = txtAddress.Text;
            Notes = txtNotes.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
