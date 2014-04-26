namespace RightpointLabs.Pourcast.Domain.Models
{
    public class User : Entity
    {
        public string Username { get; set; }

        public User(string id, string username)
            : base(id)
        {
            Username = username;
        }
    }
}
