using SalesManagerApp;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KursachV6_WinForms
{
    public partial class AddProductForm : Form
    {
        private ProductManager productManager;

        public AddProductForm(ProductManager productManager)
        {
            InitializeComponent();
            this.productManager = productManager;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string description = txtDescription.Text;

            if (decimal.TryParse(txtPrice.Text, out decimal price) && price >= 0 &&
                int.TryParse(txtStock.Text, out int stock) && stock >= 0)
            {
                try
                {
                    productManager.AddProduct(name, description, price, stock);
                    MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные данные. Цена и количество не могут быть меньше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
