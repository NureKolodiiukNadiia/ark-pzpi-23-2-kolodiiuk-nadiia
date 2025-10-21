using Microsoft.AspNetCore.Mvc;
using SpotRent.Domain.Enums;
using SpotRent.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using SpotRent.Domain.Entities;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    private readonly IPaymentService _paymentService;

    private readonly IMapper _mapper;

    public BookingsController(IBookingService bookingService, IPaymentService paymentService, IMapper mapper)
    {
        _bookingService = bookingService;
        _paymentService = paymentService;
        _mapper = mapper;
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
        var bookingResult = await _bookingService.GetBookingByIdAsync(id);
        if (bookingResult.Failure)
        {
            return BadRequest(bookingResult.Error);
        }

        return Ok(bookingResult.Value);
    }

    // POST /api/bookings
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var booking = _mapper.Map<CreateBookingRequest, Booking>(request);
        var bookingCreationResult = await _bookingService.CreateBookingAsync(booking);
        if (bookingCreationResult.Failure)
        {
            return BadRequest(bookingCreationResult.Error);
        }

        return Ok(bookingCreationResult.Value);
    }

    // PUT /api/bookings/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingRequest request)
    {
        var booking = _mapper.Map<UpdateBookingRequest, Booking>(request);
        var updateBookingResult = await _bookingService.UpdateBookingAsync(booking);
        if (updateBookingResult.Failure)
        {
            return BadRequest(updateBookingResult.Error);
        }

        return Ok(updateBookingResult.Value);
    }

    // DELETE /api/bookings/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var deleteBookingResult = await _bookingService.DeleteBookingAsync(id);
        if (deleteBookingResult.Failure)
        {
            return BadRequest(deleteBookingResult.Error);
        }

        return Ok();
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
