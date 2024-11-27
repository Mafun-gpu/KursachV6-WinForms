using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SalesManagerApp
{
    public class UserManager
    {
        private List<User> users;
        private readonly string filePath;

        public UserManager()
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");
            users = LoadUsers();
        }

        // Method to retrieve a user by username
        public User GetUserByUsername(string username)
        {
            return users.FirstOrDefault(u => u.Username == username);
        }

        // Method to update an existing user
        public void UpdateUser(User user)
        {
            var existingUser = users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                existingUser.PasswordHash = user.PasswordHash;
                existingUser.Role = user.Role;
                SaveUsersToFile();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        // Method to add a new user
        public void AddUser(string username, string password, string role)
        {
            if (users.Any(u => u.Username == username))
            {
                throw new Exception("A user with this username already exists.");
            }

            var hashedPassword = Utils.HashPassword(password);
            int newId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1; // Генерация уникального ID
            users.Add(new User(newId, username, hashedPassword, role));
            SaveUsersToFile();
        }

        // Method to delete a user
        public void DeleteUser(int userId)
        {
            var userToDelete = users.FirstOrDefault(u => u.Id == userId);
            if (userToDelete != null)
            {
                users.Remove(userToDelete);
                SaveUsersToFile();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        // Load users from a file
        private List<User> LoadUsers()
        {
            var loadedUsers = new List<User>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 4) // Убедитесь, что файл содержит все 4 поля
                    {
                        loadedUsers.Add(new User(
                            int.Parse(parts[0]), // Id
                            parts[1],            // Username
                            parts[2],            // PasswordHash
                            parts[3]             // Role
                        ));
                    }
                }
            }
            else
            {
                Console.WriteLine("User file not found. Starting with an empty list.");
            }

            return loadedUsers;
        }


        // Save users to a file
        private void SaveUsersToFile()
        {
            using (var writer = new StreamWriter(filePath, false))
            {
                foreach (var user in users)
                {
                    writer.WriteLine($"{user.Id};{user.Username};{user.PasswordHash};{user.Role}");
                }
            }
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public bool Authenticate(string username, string password, out string role)
        {
            role = null;
            var user = users.FirstOrDefault(u => u.Username == username && u.PasswordHash == Utils.HashPassword(password));
            if (user != null)
            {
                role = user.Role;
                return true;
            }
            return false;
        }
    }
}
