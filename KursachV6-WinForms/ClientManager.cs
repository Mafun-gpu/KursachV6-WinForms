using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KursachV6_WinForms
{
    public class ClientManager
    {
        private List<Client> clients;

        public ClientManager()
        {
            clients = LoadClients();
        }

        public List<Client> ListClients()
        {
            return clients;
        }

       
        private List<Client> LoadClients()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clients.txt");
            List<Client> loadedClients = new List<Client>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    loadedClients.Add(new Client
                    {
                        Id = int.Parse(parts[0]),
                        FullName = parts[1],
                        Phone = parts[2],
                        Email = parts.Length > 3 ? parts[3] : "",
                        Address = parts.Length > 4 ? parts[4] : "",
                        Notes = parts.Length > 5 ? parts[5] : ""
                    });
                }
            }

            return loadedClients;
        }

 
        private void SaveClientsToFile()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clients.txt");
            using (var writer = new StreamWriter(filePath, false))
            {
                foreach (var client in clients)
                {
                    writer.WriteLine($"{client.Id};{client.FullName};{client.Phone};{client.Email};{client.Address};{client.Notes}");
                }
            }
        }

     
        public List<Client> GetAllClients()
        {
            return clients;
        }

       
        public void AddClient(string fullName, string phone, string email = "", string address = "", string notes = "")
        {
            int newId = clients.Count > 0 ? clients.Max(c => c.Id) + 1 : 1;
            clients.Add(new Client
            {
                Id = newId,
                FullName = fullName,
                Phone = phone,
                Email = email,
                Address = address,
                Notes = notes
            });
            SaveClientsToFile();
        }

      
        public void EditClient(int clientId, string newFullName, string newPhone, string newEmail = "", string newAddress = "", string newNotes = "")
        {
            var client = clients.FirstOrDefault(c => c.Id == clientId);
            if (client != null)
            {
                client.FullName = newFullName;
                client.Phone = newPhone;
                client.Email = newEmail;
                client.Address = newAddress;
                client.Notes = newNotes;
                SaveClientsToFile();
            }
            else
            {
                throw new Exception("Клиент с указанным ID не найден.");
            }
        }

     
        public void DeleteClient(int clientId)
        {
            var client = clients.FirstOrDefault(c => c.Id == clientId);
            if (client != null)
            {
                clients.Remove(client);
                SaveClientsToFile();
            }
            else
            {
                throw new Exception("Клиент с указанным ID не найден.");
            }
        }

      
        public bool ClientExists(int clientId)
        {
            return clients.Any(c => c.Id == clientId);
        }

      
        public Client GetClientById(int clientId)
        {
            return clients.FirstOrDefault(c => c.Id == clientId);
        }
    }

    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
    }
}
