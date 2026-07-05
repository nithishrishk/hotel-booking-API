namespace HotelBooking.Api.Domain.Model
{
    public class UpdateRoomDto
    {
        public string RoomType { get; set; } = string.Empty;
        public string RoomStatus { get; set; } = "Active";
    }
}
