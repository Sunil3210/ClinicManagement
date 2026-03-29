using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Room:BaseEntity
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public string RoomType { get; set; }

        public string Status { get; set; }

    }

    public class RoomList
    {
        public RoomList()
        {
            this.Rooms = new List<Room>();
        }
        public List<Room> Rooms { get; set; }
        public int TotalCount { get; set; }
    }
}
