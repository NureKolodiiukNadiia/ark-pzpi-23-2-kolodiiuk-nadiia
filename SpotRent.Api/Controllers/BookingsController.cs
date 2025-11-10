using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SpotRent.Domain.Enums;
using SpotRent.Services.Interfaces;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    private readonly IPaymentService _paymentService;

    public BookingsController(IBookingService bookingService, IPaymentService paymentService)
    {
        _bookingService = bookingService;
        _paymentService = paymentService;
    }

    // GET /api/bookings?userId=1&spaceId=2&status=Confirmed&startDate=2025-10-01&endDate=2025-10-31&limit=50&offset=0
    [HttpGet]
    public async Task<IActionResult> GetBookings(
        [FromQuery] int? userId,
        [FromQuery] int? spaceId,
        [FromQuery] BookingStatus? status,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int limit = 50,
        [FromQuery] int offset = 0)
    {
        throw new NotImplementedException();
    }

    // GET /api/bookings/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        throw new NotImplementedException();
    }

    // POST /api/bookings
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        throw new NotImplementedException();
    }

    // PUT /api/bookings/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingRequest request)
    {
        throw new NotImplementedException();
    }

    // DELETE /api/bookings/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        throw new NotImplementedException();
    }

    // GET /api/bookings/availability?spaceId=5&startTime=2025-10-10T09:00:00Z&endTime=2025-10-10T11:00:00Z
    [HttpGet("availability")]
    public async Task<IActionResult> CheckAvailability([FromQuery] [Required] int spaceId,
        [FromQuery] [Required] DateTime startTime, [FromQuery] [Required] DateTime endTime)
    {
        throw new NotImplementedException();
    }
}

public class CreateBookingRequest
{
}

// Reuse or adapt existing DTOs. CreateBookingRequest exists below in the file; include Update DTO here.
public class UpdateBookingRequest
{
    [Required] public DateTime StartTime { get; set; }

    [Required] public DateTime EndTime { get; set; }
}