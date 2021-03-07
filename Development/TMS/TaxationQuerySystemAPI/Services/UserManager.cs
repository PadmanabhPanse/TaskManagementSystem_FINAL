using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxationQuerySystemAPI.Models;

namespace TaxationQuerySystemAPI.Services
{
    public class UserRoleManager
    {
        readonly TMSDBContext _context;
        public UserRoleManager(TMSDBContext context)
        {
            _context = context;
        }
        public User getAdmin()
        {
            return (from _user in _context.Users
                    join _userrole in _context.UserRoles on _user.UserId equals _userrole.UserId
                    join _role in _context.Roles on _userrole.RoleId equals _role.RoleId
                    join _owner in _context.TaskOwners on _user.UserId equals _owner.UserId
                    where string.Compare(_role.RoleName, "admin") == 0
                    select new User { UserId = _user.UserId, UserName = _user.UserName, Email = _user.Email, PhoneNumber = _user.PhoneNumber, OwnerId = _owner.TaskOwnerId })
                                          .SingleOrDefault();
        }
        public User getUsersByOwner(string RoleName, long OwnerId)
        {
            return (from _user in _context.Users
                    join _userrole in _context.UserRoles on _user.UserId equals _userrole.UserId
                    join _role in _context.Roles on _userrole.RoleId equals _role.RoleId
                    join _owner in _context.TaskOwners on _user.UserId equals _owner.UserId
                    where string.Compare(_role.RoleName, RoleName) == 0 && _owner.TaskOwnerId == OwnerId
                    select new User { UserId = _user.UserId, UserName = _user.UserName, Email = _user.Email, PhoneNumber = _user.PhoneNumber, OwnerId = OwnerId })
                                          .SingleOrDefault();
        }
    }
}
