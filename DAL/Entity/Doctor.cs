using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Doctor:BaseEntity
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public int? DepartmentId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

}
