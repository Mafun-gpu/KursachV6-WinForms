using SalesManagerApp;
using System;
using System.Linq;
using System.Windows.Forms;

namespace KursachV6_WinForms
{
    public partial class ClientManagementForm : Form
    {
        private ClientManager clientManager;

        public ClientManagementForm(ClientManager clientManager)
        {
            InitializeComponent();
            this.clientManager = clientManager;
            this.Load += ClientManagementForm_Load;
        }

        private void ClientManagementForm_Load(object sender, EventArgs e)
        {
            InitializeContextMenu();

            dgvClients.AllowUserToAddRows = false; 
            dgvClients.Columns.Clear();
            dgvClients.Columns.Add("Id", "ID");
            dgvClients.Columns.Add("FullName", "Полное имя");
            dgvClients.Columns.Add("Phone", "Телефон");
            dgvClients.Columns.Add("Email", "Email");
            dgvClients.Columns.Add("Address", "Адрес");
            dgvClients.Columns.Add("Notes", "Примечания");

            UpdateClientGrid();
        }

        private void btnAddClient_Click(object sender, EventArgs e)
        {
            var addClientForm = new ClientDetailsForm("Добавить клиента");
            if (addClientForm.ShowDialog() == DialogResult.OK)
            {
                clientManager.AddClient(
                    addClientForm.FullName,
                    addClientForm.Phone,
                    addClientForm.Email,
                    addClientForm.Address,
                    addClientForm.Notes
                );

                UpdateClientGrid();
                MessageBox.Show("Клиент успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteClient_Click(object sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                int clientId = Convert.ToInt32(dgvClients.SelectedRows[0].Cells[0].Value);
                var clientToDelete = clientManager.GetClientById(clientId);

                var confirmResult = MessageBox.Show(
                    $"Вы уверены, что хотите удалить клиента: {clientToDelete.FullName}?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirmResult == DialogResult.Yes)
                {
                    clientManager.DeleteClient(clientId);
                    UpdateClientGrid();
                    MessageBox.Show("Клиент успешно удален.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditClient_Click(object sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                int clientId = Convert.ToInt32(dgvClients.SelectedRows[0].Cells[0].Value);
                var existingClient = clientManager.GetClientById(clientId);

                var editClientForm = new ClientDetailsForm("Редактировать клиента", existingClient);
                if (editClientForm.ShowDialog() == DialogResult.OK)
                {
                    clientManager.EditClient(
                        clientId,
                        editClientForm.FullName,
                        editClientForm.Phone,
                        editClientForm.Email,
                        editClientForm.Address,
                        editClientForm.Notes
                    );

                    UpdateClientGrid();
                    MessageBox.Show("Данные клиента успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditClient_Click(object sender, EventArgs e)
        {
            btnEditClient_Click(sender, e);
        }

        private void DeleteClient_Click(object sender, EventArgs e)
        {
            btnDeleteClient_Click(sender, e);
        }

        private void ShowClientDetails_Click(object sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                int clientId = Convert.ToInt32(dgvClients.SelectedRows[0].Cells[0].Value);
                var client = clientManager.GetClientById(clientId);
                MessageBox.Show($"Детали клиента:\n\nИмя: {client.FullName}\nТелефон: {client.Phone}\nEmail: {client.Email}\nАдрес: {client.Address}\nПримечания: {client.Notes}",
                                "Детали клиента", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для просмотра.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.ToLower();
            var filteredClients = clientManager.GetAllClients()
                .Where(c => c.FullName.ToLower().Contains(searchQuery) ||
                            c.Phone.ToLower().Contains(searchQuery) ||
                            c.Email.ToLower().Contains(searchQuery) ||
                            c.Address.ToLower().Contains(searchQuery))
                .ToList();

            dgvClients.Rows.Clear();
            foreach (var client in filteredClients)
            {
                dgvClients.Rows.Add(client.Id, client.FullName, client.Phone, client.Email, client.Address, client.Notes);
            }
        }

        private void btnResetSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear(); 
            UpdateClientGrid();
        }

        private void UpdateClientGrid()
        {
            dgvClients.Rows.Clear(); 

        
            foreach (var client in clientManager.GetAllClients())
            {
                dgvClients.Rows.Add(client.Id, client.FullName, client.Phone, client.Email, client.Address, client.Notes);
            }
        }

        private void InitializeContextMenu()
        {
            contextMenuClients = new ContextMenuStrip();
            var editItem = new ToolStripMenuItem("Редактировать клиента", null, EditClient_Click);
            var deleteItem = new ToolStripMenuItem("Удалить клиента", null, DeleteClient_Click);
            var detailsItem = new ToolStripMenuItem("Просмотреть детали", null, ShowClientDetails_Click);

            contextMenuClients.Items.AddRange(new[] { editItem, deleteItem, detailsItem });

     
            dgvClients.ContextMenuStrip = contextMenuClients;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuClients_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}