using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Api.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}
