using HotelBooking.Api.Domain.Model;
using HotelBooking.Api.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> BookRoom([FromBody] BookRoomDto bookRoomDto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized();
            }

            var success = await _bookingService.BookRoomAsync(userId, bookRoomDto);
            if (!success)
            {
                return BadRequest(new { message = "Rooms are not available for the selected period and room type." });
            }

            // A real app might return the booked room no here, but our service just returns true/false for simplicity.
            // We can assume success means room is booked.
            return Ok(new { message = "Room booked successfully." });
        }

        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized();
            }

            var result = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(result);
        }
    }
}
