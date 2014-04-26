namespace RightpointLabs.Pourcast.Web
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    using Microsoft.Practices.ServiceLocation;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;

    public class PourcastRoleProvider : RoleProvider
    {
        private IIdentityOrchestrator _identityOrchestrator
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IIdentityOrchestrator>();
                //return DependencyResolver.Current.GetService<IIdentityOrchestrator>();
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return _identityOrchestrator.IsUserInRole(username, roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            return _identityOrchestrator.GetRolesForUser(username).Select(x => x.Name).ToArray();
        }

        public override void CreateRole(string roleName)
        {
            _identityOrchestrator.CreateRole(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            _identityOrchestrator.DeleteRole(roleName);

            return true;
        }

        public override bool RoleExists(string roleName)
        {
            return _identityOrchestrator.RoleExists(roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            _identityOrchestrator.AddUsersToRoles(usernames, roleNames);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return _identityOrchestrator.GetUsersInRole(roleName).Select(x => x.Username).ToArray();
        }

        public override string[] GetAllRoles()
        {
            return _identityOrchestrator.GetAllRoles().Select(x => x.Name).ToArray();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new System.NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}