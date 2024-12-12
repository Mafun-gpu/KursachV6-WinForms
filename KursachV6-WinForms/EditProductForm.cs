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

            LoadProductData(); 
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
                this.Close(); 
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();

           
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Название товара не может быть пустым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Описание товара не может быть пустым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Пожалуйста, введите корректную цену товара. Цена не может быть отрицательной.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное количество товара. Количество не может быть отрицательным.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
