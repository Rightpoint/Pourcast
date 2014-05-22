namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class IdentityOrchestrator : BaseOrchestrator, IIdentityOrchestrator
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

            if (role == null || user == null)
            {
                return false;
            }

            return role.HasUser(user.Id);
        }

        public IEnumerable<Role> GetRolesForUser(string username)
        {
            var roles = _roleRepository.GetAll();
            var user = _userRepository.GetByUsername(username);

            if (user == null) return new List<Role>();

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

        [Transactional]
        public void DeleteRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public bool RoleExists(string roleName)
        {
            return (_roleRepository.GetByName(roleName) != null);
        }

        [Transactional]
        public void CreateUser(string username)
        {
            if (_userRepository.GetByUsername(username) != null)
            {
                throw new Exception("User already exists.");
            }

            var id = _roleRepository.NextIdentity();
            var user = new User(id, username);

            _userRepository.Add(user);
        }

        [Transactional]
        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var users = _userRepository.GetAll().Where(u => usernames.Contains(u.Username));
            var roles = _roleRepository.GetAll().Where(r => roleNames.Contains(r.Name));

            foreach (var role in roles)
            {
                foreach (var user in users)
                {
                    role.AddUser(user.Id);
                    user.AddRole(role.Id);

                    _roleRepository.Update(role);
                    _userRepository.Update(user);
                }
            }
        }

        public IEnumerable<User> GetUsersInRole(string roleName)
        {
            var role = _roleRepository.GetByName(roleName);

            if (role == null) return new List<User>();

            return _userRepository.GetUsersInRole(role.Id);
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _roleRepository.GetAll();
        }

        [Transactional]
        public void AddUserToRole(string username, string roleName)
        {
            AddUsersToRoles(new []{ username }, new []{ roleName });
        }

        public bool UserExists(string username)
        {
            return (_userRepository.GetByUsername(username) != null);
        }
    }
}