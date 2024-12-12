namespace SalesManagerApp
{
    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }

        public Client(int id, string fullName, string phone, string email, string address, string notes)
        {
            Id = id;
            FullName = fullName;
            Phone = phone;
            Email = email;
            Address = address;
            Notes = notes;
        }

        public override string ToString()
        {
            return $"{Id};{FullName};{Phone};{Email};{Address};{Notes}";
        }
    }
}
