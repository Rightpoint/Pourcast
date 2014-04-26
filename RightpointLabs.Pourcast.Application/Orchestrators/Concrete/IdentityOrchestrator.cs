namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class IdentityOrchestrator : IIdentityOrchestrator
    {
        private readonly IUserRepository _userRepository;

        private readonly IRoleRepository _roleRepository;

        public IdentityOrchestrator(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            if (roleRepository == null) throw new ArgumentNullException("roleRepository");

            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public bool IsUserInRole(string username, string roleName)
        {
            var user = _userRepository.GetByUsername(username);
            var role = _roleRepository.GetByName(roleName);

            return role.HasUser(user.Id);
        }

        public IEnumerable<Role> GetRolesForUser(string username)
        {
            var roles = _roleRepository.GetAll();
            var user = _userRepository.GetByUsername(username);

            return roles.Where(r => r.HasUser(user.Id));
        }

        [Transactional]
        public void CreateRole(string roleName)
        {
            if (_roleRepository.GetByName(roleName) != null)
            {
                throw new Exception("Role already exists.");
            }
            
            var id = _roleRepository.NextIdentity();
            var role = new Role(id, roleName);

            _roleRepository.Add(role);
        }

        public void DeleteRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public bool RoleExists(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> GetUsersInRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Role> GetAllRoles()
        {
            throw new System.NotImplementedException();
        }
    }
}