using HotelBooking.Api.Domain.Model;

namespace HotelBooking.Api.Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> AuthenticateLocalAsync(LoginDto loginDto);
        Task<AuthResponseDto> AuthenticateSocialAsync(SocialLoginDto socialLoginDto);
    }
}
