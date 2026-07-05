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
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public BookingService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> BookRoomAsync(int userId, BookRoomDto bookRoomDto)
        {
            // Find an available room of the requested type for the given date range
            var availableRoom = await _context.Rooms
                .Where(r => r.RoomType == bookRoomDto.RoomType && r.IsActive)
                .Where(r => !r.Bookings.Any(b => b.Status != "Completed" && 
                                                 b.FromDate <= bookRoomDto.ToDate && 
                                                 b.ToDate >= bookRoomDto.FromDate))
                .FirstOrDefaultAsync();

            if (availableRoom == null)
            {
                return false; // No rooms available
            }

            var booking = new Booking
            {
                RoomId = availableRoom.Id,
                UserId = userId,
                FromDate = bookRoomDto.FromDate,
                ToDate = bookRoomDto.ToDate,
                Status = "Booked"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserBookingDto>> GetUserBookingsAsync(int userId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using IDbConnection db = new SqlConnection(connectionString);

            var parameters = new { UserId = userId };
            var result = await db.QueryAsync<UserBookingDto>(
                "sp_GetUserBookings", 
                parameters, 
                commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<IEnumerable<UserBookingDto>> GetAllBookingsAsync(DateTime? date)
        {
            var query = _context.Bookings
                .Include(b => b.Room)
                .AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(b => b.FromDate <= date.Value.Date && b.ToDate >= date.Value.Date);
            }

            var bookings = await query.Select(b => new UserBookingDto
            {
                BookingId = b.Id,
                RoomNo = b.Room.RoomNo,
                RoomType = b.Room.RoomType,
                FromDate = b.FromDate,
                ToDate = b.ToDate,
                Status = b.Status
            }).ToListAsync();

            return bookings;
        }
    }
}
