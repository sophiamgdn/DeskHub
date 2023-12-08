using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskHub.Models
{
    public static class UserDataService
    {
        public static List<User> Users { get; private set; }

        static UserDataService()
        {
            Users = new List<User>();
        }

        public static void AddUser(User user)
        {
            Users.Add(user);
        }

        // You can add other methods here for getting, updating, or deleting users if needed
    }
}
