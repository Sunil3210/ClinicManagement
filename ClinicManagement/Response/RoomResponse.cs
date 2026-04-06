using DAL.Entity;

namespace ClinicManagement.Response
{
    public class RoomResponse
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }
        public int TypeId { get; set; }
        public string RoomType { get; set; }

        public string Status { get; set; }
    }

    public class RoomListResponse
    {
        public RoomListResponse()
        {
            this.Rooms = new List<RoomResponse>();
        }
        public List<RoomResponse> Rooms { get; set; }
        public int TotalCount { get; set; }
    }

}
