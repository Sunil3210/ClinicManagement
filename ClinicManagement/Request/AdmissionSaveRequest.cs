namespace ClinicManagement.Request
{
    public class AdmissionSaveRequest
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int RoomId { get; set; }
        public DateOnly? AdmissionDate { get; set; }
        public DateOnly? DischargeDate { get; set; }
    }
}
