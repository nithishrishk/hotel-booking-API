using Dapper;
using HotelBooking.Api.Domain.Entities;
using HotelBooking.Api.Domain.Model;
using HotelBooking.Api.Infrastructure.Persistence.Context;
using HotelBooking.Api.Infrastructure.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HotelBooking.Api.Infrastructure.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public RoomService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> AddRoomAsync(AddRoomDto addRoomDto)
        {
            if (await _context.Rooms.AnyAsync(r => r.RoomNo == addRoomDto.RoomNo))
                return false; // Room already exists

            var room = new Room
            {
                RoomNo = addRoomDto.RoomNo,
                RoomType = addRoomDto.RoomType,
                IsAC = addRoomDto.IsAC,
                Price = addRoomDto.Price,
                IsActive = true
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RoomStatusDto>> GetRoomsStatusByDateAsync(DateTime date)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using IDbConnection db = new SqlConnection(connectionString);

            var parameters = new { SelectedDate = date.Date };
            var result = await db.QueryAsync<RoomStatusDto>(
                "sp_GetRoomsStatusByDate", 
                parameters, 
                commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<bool> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

            room.RoomType = updateRoomDto.RoomType;
            room.RoomStatus = updateRoomDto.RoomStatus;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null) return "Room not found.";

            // Check if room has active bookings
            var hasActiveBookings = room.Bookings.Any(b => 
                b.Status != "Completed" && 
                b.Status != "Cancelled" &&
                b.ToDate >= DateTime.Today);

            if (hasActiveBookings)
            {
                return "Cannot delete room with active or future bookings.";
            }

            room.RoomStatus = "Deleted";
            room.IsActive = false;
            
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return string.Empty; // Success
        }
    }
}
