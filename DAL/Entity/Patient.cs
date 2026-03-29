namespace DAL.Entity
{
    public class Patient:BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public char Gender { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
        public string Aadhar { get; set; }
    }

    public class PatientList
    {
        public PatientList()
        {
            this.Patients = new List<Patient>();
        }
        public List<Patient> Patients { get; set; }
        public int TotalCount { get; set; }
    }
}
