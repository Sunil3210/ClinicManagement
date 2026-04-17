using DAL.Entity;

namespace ClinicManagement.Response
{
    public class AdmissionResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int RoomId { get; set; }
        public DateOnly? AdmissionDate { get; set; }
        public DateOnly? DischargeDate { get; set; }
        public string PatientName { get; set; }
        public string RoomNumber { get; set; }
    }

    public class AdmissionListResponse
    {
        public List<AdmissionResponse> admissions { get; set; }
        public int TotalCount { get; set; }
        public AdmissionListResponse()
        {
            this.admissions = new List<AdmissionResponse>();
        }
    }
}
