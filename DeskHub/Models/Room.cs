using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeskHub.Models
{
    public class Room
    {
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public string RoomType { get; set; }
        public int RoomCapacity { get; set; }
        public string Branch { get; set; }
        public double Rate { get; set; }
        public bool IsAvailable { get; set; }

    }


}
