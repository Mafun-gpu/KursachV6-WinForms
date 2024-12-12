namespace SalesManagerApp
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public User(int id, string username, string passwordHash, string role)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }

        public string GetInfo()
        {
            return $"User: {Username}, Role: {Role}";
        }
    }
}
