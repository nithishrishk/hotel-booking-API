namespace HotelBooking.Api.Domain.Model
{
    public class UserBookingDto
    {
        public int BookingId { get; set; }
        public string RoomNo { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
