using HotelBooking.Api.Domain.Model;

namespace HotelBooking.Api.Infrastructure.Services.Interfaces
{
    public interface IBookingService
    {
        Task<bool> BookRoomAsync(int userId, BookRoomDto bookRoomDto);
        Task<IEnumerable<UserBookingDto>> GetUserBookingsAsync(int userId);
        Task<IEnumerable<UserBookingDto>> GetAllBookingsAsync(DateTime? date);
    }
}
