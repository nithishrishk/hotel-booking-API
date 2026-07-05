using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Api.Domain.Model
{
    public class AddRoomDto
    {
        [Required]
        [MaxLength(50)]
        public string RoomNo { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string RoomType { get; set; } = string.Empty; // 'Single' or 'Double'
        
        public bool IsAC { get; set; }
        
        [Required]
        public decimal Price { get; set; }
    }
}
