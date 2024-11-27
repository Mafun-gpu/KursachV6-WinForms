namespace SalesManagerApp
{
    public class User : Entity, IEntityInfo
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public User(int id, string username, string passwordHash, string role)
            : base(id)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }

        public override string GetInfo()
        {
            return $"User: {Username}, Role: {Role}";
        }
    }
}
