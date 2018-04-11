using DemoWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DemoWebApplication.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        DemoWebAppDBContext _context = new DemoWebAppDBContext();
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            User user = _context.Users.Where(w => w.Email.ToUpper() == username.Trim().ToUpper()).FirstOrDefault();

            var roles = _context.UsersRoles.Where(f => f.UserID == user.ID)
                        .Join(_context.Roles, a => a.RoleID, b => b.ID, (p, q) => new { userRole = p, role = q })
                        .Select(s => s.role.Name);

            if (roles != null)
                return roles.ToArray();
            else
                return new string[] { };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            User user = _context.Users.Where(w => w.Email.ToUpper() == username.Trim().ToUpper()).FirstOrDefault();

            var roles = _context.UsersRoles.Where(f => f.UserID == user.ID)
                        .Join(_context.Roles, a => a.RoleID, b => b.ID, (p, q) => new { userRole = p, role = q })
                        .Select(s => s.role.Name);

            if (user != null)
                return roles.Any(r => r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
            else
                return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public void AddUserToRole(int userID, string RoleName)
        {
            Role role = _context.Roles.Where(w => w.Name.ToUpper() == RoleName.Trim().ToUpper()).FirstOrDefault();

            UsersRole usersRole = new UsersRole
            {
                UserID = userID,
                RoleID = role.ID
            };

            try
            {
                _context.UsersRoles.Add(usersRole);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}