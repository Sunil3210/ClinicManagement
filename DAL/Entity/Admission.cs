using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Admission:BaseEntity
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int RoomId { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string Name { get; set; }
        public string RoomType { get; set; }
        public string RoomNumber { get; set; }
        public string Phone { get; set; }
        public char Gender { get; set; }
        public int Age { get; set; }
    }
    public class AdmissionList
    {
        public List<Admission> admissions { get; set; }
        public int TotalCount { get; set; }
        public AdmissionList()
        {
            this.admissions = new List<Admission>();
        }
    }
}
