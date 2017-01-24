namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;
    using System.Collections.Generic;

    public class User : Entity
    {
        private User() { }

        public User(string id, string username)
            : base(id)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
