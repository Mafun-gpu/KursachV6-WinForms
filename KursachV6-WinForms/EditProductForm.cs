using SalesManagerApp;
using System;
using System.Windows.Forms;

namespace KursachV6_WinForms
{
    public partial class EditProductForm : Form
    {
        private ProductManager productManager;
        private int productId;

        public EditProductForm(ProductManager productManager, int productId)
        {
            InitializeComponent();
            this.productManager = productManager;
            this.productId = productId;

            LoadProductData(); // Загружаем данные товара
        }

        private void LoadProductData()
        {
            var product = productManager.GetProductById(productId);
            if (product != null)
            {
                txtName.Text = product.Name;
                txtDescription.Text = product.Description;
                txtPrice.Text = product.Price.ToString();
                txtStock.Text = product.Stock.ToString();
            }
            else
            {
                MessageBox.Show("Товар не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Закрываем окно, если товар не найден
            }
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
                    productManager.EditProduct(productId, name, description, price, stock);
                    MessageBox.Show("Товар успешно обновлён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
