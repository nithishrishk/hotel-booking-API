using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Api.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;
        
        public string? PasswordHash { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Provider { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string? ProviderId { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
