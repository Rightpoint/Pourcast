namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;
    using System.Collections.Generic;

    public class Role : Entity
    {
        private List<string> _userIds;

        private Role() { }

        public Role(string id, string name)
            : base(id)
        {
            Name = name;
            _userIds = new List<string>();
        }

        public string Name { get; set; }

        public IEnumerable<String> UserIds
        {
            get
            {
                return _userIds;
            }
        }

        public void AddUser(string id)
        {
            if (!HasUser(id))
            {
                _userIds.Add(id);    
            }
        }

        public bool HasUser(string userId)
        {
            return _userIds.Contains(userId);
        }
    }
}
