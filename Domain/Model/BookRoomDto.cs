using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Api.Domain.Model
{
    public class BookRoomDto
    {
        [Required]
        public DateTime FromDate { get; set; }
        
        [Required]
        public DateTime ToDate { get; set; }
        
        [Required]
        public string RoomType { get; set; } = string.Empty;
    }
}
