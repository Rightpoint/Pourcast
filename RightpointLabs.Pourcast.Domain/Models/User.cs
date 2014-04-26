namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;
    using System.Collections.Generic;

    public class User : Entity
    {
        private List<string> _roleIds;

        private User() { }

        public User(string id, string username)
            : base(id)
        {
            Username = username;
            _roleIds = new List<string>();
        }

        public string Username { get; set; }

        public IEnumerable<String> RoleIds
        {
            get
            {
                return _roleIds;
            }
        }

        public void AddRole(string roleId)
        {
            if (!HasRole(roleId))
            {
                _roleIds.Add(roleId);
            }
        }

        public bool HasRole(string roleId)
        {
            return _roleIds.Contains(roleId);
        }
    }
}
