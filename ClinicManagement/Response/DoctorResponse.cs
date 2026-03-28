namespace ClinicManagement.Response
{
    public class DoctorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Specialization { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
