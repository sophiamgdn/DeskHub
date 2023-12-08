using DeskHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskHub.Services
{
    public class UserDataService
    {
        private static List<User> registeredUsers = new List<User>();
        public void Register(User user)
        {
            registeredUsers.Add(user);
        }

        public User GetUserByUsername(string username)
        {
            return registeredUsers.Find(u => u.Username == username);
        }
    }
}
