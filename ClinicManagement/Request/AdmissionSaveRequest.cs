namespace ClinicManagement.Request
{
    public class AdmissionSaveRequest
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int RoomId { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Active { get; set; }
    }
}
