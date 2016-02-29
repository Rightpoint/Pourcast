using System.Reflection;
using log4net;

namespace RightpointLabs.Pourcast.Web
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    using Microsoft.Practices.ServiceLocation;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;

    public class PourcastRoleProvider : RoleProvider
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
            log.DebugFormat("PRP.IsUserInRole: {0}, {1}", username, roleName);
            return _identityOrchestrator.IsUserInRole(username, roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            log.DebugFormat("PRP.GetRolesForUser: {0}", username);
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
            log.DebugFormat("PRP.GetUsersInRole: {0}", roleName);
            return _identityOrchestrator.GetUsersInRole(roleName).Select(x => x.Username).ToArray();
        }

        public override string[] GetAllRoles()
        {
            return _identityOrchestrator.GetAllRoles().Select(x => x.Name).ToArray();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            log.DebugFormat("PRP.FindUsersInRole: {0}, {1}", roleName, usernameToMatch);
            throw new System.NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}