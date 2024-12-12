using SalesManagerApp;
using System;
using System.Linq;
using System.Windows.Forms;

namespace KursachV6_WinForms
{
    public partial class ProductManagementForm : Form
    {
        private ProductManager productManager;

        public ProductManagementForm()
        {
            InitializeComponent();
            productManager = new ProductManager();
            ConfigureDataGridView();
            LoadProducts();
        }

        private void ConfigureDataGridView()
        {
          
            dgvProducts.Columns.Clear();
            dgvProducts.Columns.Add("Id", "ID");
            dgvProducts.Columns.Add("Name", "Название");
            dgvProducts.Columns.Add("Description", "Описание");
            dgvProducts.Columns.Add("Price", "Цена");
            dgvProducts.Columns.Add("Stock", "Остаток");

            
            dgvProducts.AllowUserToAddRows = true;
        }


        private void LoadProducts()
        {
            dgvProducts.Rows.Clear(); 

          
            foreach (var product in productManager.GetAllProducts())
            {
                dgvProducts.Rows.Add(product.Id, product.Name, product.Description, product.Price, product.Stock);
            }
        }

        private void ProductManagementForm_Load(object sender, EventArgs e)
        {
            dgvProducts.Columns.Clear();
            dgvProducts.Columns.Add("Id", "ID");
            dgvProducts.Columns.Add("Name", "Название");
            dgvProducts.Columns.Add("Description", "Описание");
            dgvProducts.Columns.Add("Price", "Цена");
            dgvProducts.Columns.Add("Stock", "Количество");
            LoadProducts();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
           
            var addProductForm = new AddProductForm(productManager);
            if (addProductForm.ShowDialog() == DialogResult.OK)
            {
                LoadProducts(); 
            }
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
        
            if (dgvProducts.SelectedRows.Count > 0 && dgvProducts.SelectedRows[0].Index != dgvProducts.Rows.Count - 1)
            {
              
                int productId = (int)dgvProducts.SelectedRows[0].Cells[0].Value;

                
                var editProductForm = new EditProductForm(productManager, productId);
                if (editProductForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts(); 
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
           
            if (dgvProducts.SelectedRows.Count > 0 && dgvProducts.SelectedRows[0].Index != dgvProducts.Rows.Count - 1)
            {
              
                int productId = (int)dgvProducts.SelectedRows[0].Cells[0].Value;

            
                var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить выбранный товар?",
                                                    "Подтверждение удаления",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    productManager.DeleteProduct(productId);
                    LoadProducts(); 
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("Введите текст для поиска.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var filteredProducts = productManager.GetAllProducts().Where(p =>
                p.Name.ToLower().Contains(query) ||
                p.Description.ToLower().Contains(query) ||
                p.Price.ToString().Contains(query) ||
                p.Stock.ToString().Contains(query)).ToList();

            dgvProducts.Rows.Clear(); 

            foreach (var product in filteredProducts)
            {
                dgvProducts.Rows.Add(product.Id, product.Name, product.Description, product.Price, product.Stock);
            }
        }

        private void btnResetSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadProducts(); 
        }
    }
}
