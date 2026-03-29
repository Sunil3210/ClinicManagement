namespace ClinicManagement.Request
{
    public class PatientSaveRequest
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
}
