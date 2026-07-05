namespace HotelBooking.Api.Domain.Model
{
    public class RoomStatusDto
    {
        public int Id { get; set; }
        public string RoomNo { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public bool IsAC { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty; // 'Booked' or 'Not Booked'
    }
}
