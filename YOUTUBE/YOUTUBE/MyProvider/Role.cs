using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using YOUTUBE.Models;

namespace YOUTUBE.MyProvider
{
    public class Role : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            DataClasses1DataContext db = new DataClasses1DataContext();
            account acc = db.accounts.Where(x => x.username == username).SingleOrDefault();
            string data = db.Admins.Where(x => x.MaQL == acc.MaQL).FirstOrDefault().chucvuql;
            if (acc.MaQL==1)
            {
                data = "phanquyen";
                string data2 = "admin";
                string[] result1 = { data,data2 };
                return result1;
            }else if(acc.MaQL==2)
            {
                //string data = db.Admins.Where(x => x.MaQL == acc.MaQL).FirstOrDefault().chucvuql;
                data = "phanquyen";
                string[] result2 = { data };
                return result2;
            }
            data = "khongdctruycap";
            string[] result = { data };
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}