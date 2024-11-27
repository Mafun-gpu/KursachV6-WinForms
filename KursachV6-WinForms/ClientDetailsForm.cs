using System;
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Поле Ф.И.О. и Телефон обязательны для заполнения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
