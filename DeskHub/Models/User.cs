using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskHub.Models
{
    public class User 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; } 
        public string Address { get; set; }
        public DateOnly Birthday { get; set; }
        public string UserType { get; set; }
        public string UserID { get; set;}

        
        public string FullName
        {
            get
            {
                // Concatenate first name, middle name, and last name
                return $"{FirstName} {(string.IsNullOrWhiteSpace(MiddleName) ? "" : MiddleName + " ")}{LastName}";
            }
        }
    }
}
