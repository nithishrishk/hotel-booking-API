using HotelBooking.Api.Domain.Model;

namespace HotelBooking.Api.Infrastructure.Services.Interfaces
{
    public interface IRoomService
    {
        Task<bool> AddRoomAsync(AddRoomDto addRoomDto);
        Task<IEnumerable<RoomStatusDto>> GetRoomsStatusByDateAsync(DateTime date);
        Task<bool> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto);
        Task<string> DeleteRoomAsync(int id);
    }
}
