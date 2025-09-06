using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Staff : BaseEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Role { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }

    public class StaffList
    {
        public StaffList()
        {
            this.Staffs = new List<Staff>();
        }
        public List<Staff> Staffs { get; set; }
        public int TotalCount { get; set; }
    }
}
