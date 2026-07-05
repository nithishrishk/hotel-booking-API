using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Api.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string RoomNo { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string RoomType { get; set; } = string.Empty;
        
        public bool IsAC { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [MaxLength(50)]
        public string RoomStatus { get; set; } = "Active";

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
