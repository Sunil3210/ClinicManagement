using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Doctor : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DepartmentName { get; set; }

    }
    public class DoctorList
    {
        public List<Doctor> Doctors { get; set; }
        public int TotalCount { get; set; }
        public DoctorList()
        {
            this.Doctors = new List<Doctor>();
        }
    }
}
