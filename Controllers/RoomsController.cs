using HotelBooking.Api.Domain.Model;
using HotelBooking.Api.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;

        public RoomsController(IRoomService roomService, IBookingService bookingService)
        {
            _roomService = roomService;
            _bookingService = bookingService;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetRoomsStatus([FromQuery] DateTime date)
        {
            var result = await _roomService.GetRoomsStatusByDateAsync(date);
            return Ok(result);
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookings([FromQuery] DateTime? date)
        {
            var result = await _bookingService.GetAllBookingsAsync(date);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] AddRoomDto addRoomDto)
        {
            var success = await _roomService.AddRoomAsync(addRoomDto);
            if (!success)
            {
                return BadRequest(new { message = "Room number already exists or invalid data." });
            }
            return Ok(new { message = "Room added successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDto updateRoomDto)
        {
            var success = await _roomService.UpdateRoomAsync(id, updateRoomDto);
            if (!success)
            {
                return NotFound(new { message = "Room not found." });
            }
            return Ok(new { message = "Room updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var errorMessage = await _roomService.DeleteRoomAsync(id);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                if (errorMessage == "Room not found.") return NotFound(new { message = errorMessage });
                return BadRequest(new { message = errorMessage });
            }
            return Ok(new { message = "Room deleted successfully." });
        }
    }
}
