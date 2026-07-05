using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Api.Domain.Model
{
    public class SocialLoginDto
    {
        [Required]
        public string Provider { get; set; } = string.Empty; // 'Google' or 'Twitter'
        
        [Required]
        public string ProviderId { get; set; } = string.Empty;
        
        [Required]
        public string Username { get; set; } = string.Empty; // typically email or display name
    }
}
