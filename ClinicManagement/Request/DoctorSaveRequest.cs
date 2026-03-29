namespace ClinicManagement.Request
{
    public class DoctorSaveRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
