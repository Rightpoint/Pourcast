namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IIdentityOrchestrator
    {
        bool IsUserInRole(string username, string roleName);

        IEnumerable<Role> GetRolesForUser(string username);

        void CreateRole(string roleName);

        void DeleteRole(string roleName);

        bool RoleExists(string roleName);

        void AddUsersToRoles(string[] usernames, string[] roleNames);

        IEnumerable<User> GetUsersInRole(string roleName);

        IEnumerable<Role> GetAllRoles();
    }
}
