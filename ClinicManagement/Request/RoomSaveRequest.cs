namespace ClinicManagement.Request
{
    public class RoomSaveRequest
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }
        public int TypeId { get; set; }
        public string Status { get; set; }
    }
}
