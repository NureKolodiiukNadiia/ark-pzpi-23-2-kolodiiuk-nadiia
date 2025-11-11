using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotRent.Domain.Enums;
using SpotRent.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using SpotRent.Domain.Entities;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    private readonly IPaymentService _paymentService;

    private readonly ILogger<BookingsController> _logger;

    public BookingsController(IBookingService bookingService, IPaymentService paymentService,
        ILogger<BookingsController> logger)
    {
        _bookingService = bookingService;
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var booking = _mapper.Map<CreateBookingRequest, Booking>(request);
        var bookingCreationResult = await _bookingService.CreateBookingAsync(booking);
        if (bookingCreationResult.Failure)
        {
            return BadRequest(bookingCreationResult.Error);
        }

        var response = _mapper.Map<Booking, BookingCreateResponse>(bookingCreationResult.Value);

        // Return Created with location header to GET /bookings/{id}
        return CreatedAtAction(nameof(GetBooking), new { id = bookingCreationResult.Value.Id }, response);
    }

    [HttpGet("users/{userId}/history")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetUserBookingsHistory(int userId)
    {
        var result = await _bookingService.GetUserBookingsHistoryAsync(userId);
        if (result.Failure)
            return BadRequest(result.Error);
    
        var items = _mapper.Map<List<BookingListItem>>(result.Value);
        var dto = new BookingListResponse
        {
            Data = items,
            Pagination = new PaginationDto
            {
                Page = 1,
                PageSize = items.Count,
                Total = items.Count
            }
        };
    
        return Ok(dto);
    }
    
    [HttpGet("users/{userId}/active")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetUserActiveBookings(int userId)
    {
        var result = await _bookingService.GetUserActiveBookingsAsync(userId);
        if (result.Failure)
            return BadRequest(result.Error);
    
        var items = _mapper.Map<List<BookingListItem>>(result.Value);
        var dto = new BookingListResponse
        {
            Data = items,
            Pagination = new PaginationDto
            {
                Page = 1,
                PageSize = items.Count,
                Total = items.Count
            }
        };
    
        return Ok(dto);
    }
    
    [HttpGet("owners/{ownerId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> GetOwnerBookings(int ownerId)
    {
        var result = await _bookingService.GetOwnerBookingsAsync(ownerId);
        if (result.Failure)
            return BadRequest(result.Error);
    
        var items = _mapper.Map<List<BookingListItem>>(result.Value);
        var dto = new BookingListResponse
        {
            Data = items,
            Pagination = new PaginationDto
            {
                Page = 1,
                PageSize = items.Count,
                Total = items.Count
            }
        };
    
        return Ok(dto);
    }
    
    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetBookings(
        [FromQuery] BookingStatus? status,
        [FromQuery] bool? upcoming,
        [FromQuery] bool? past,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        // Map query into service call - assumes service supports filtering/pagination.
        // If your IBookingService has a different signature adjust accordingly.
        var listResult = await _bookingService.GetBookingsAsync(status, upcoming, past, page, pageSize);
        if (listResult.Failure)
        {
            return BadRequest(listResult.Error);
        }

        var dto = new BookingListResponse
        {
            Data = _mapper.Map<List<Booking>, List<BookingListItem>>(listResult.Value.Items),
            Pagination = new PaginationDto
            {
                Page = listResult.Value.Page,
                PageSize = listResult.Value.PageSize,
                Total = listResult.Value.Total
            }
        };

        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var bookingResult = await _bookingService.GetBookingByIdAsync(id);
        if (bookingResult.Failure)
        {
            return BadRequest(bookingResult.Error);
        }

        var dto = _mapper.Map<Booking, BookingDetailsResponse>(bookingResult.Value);
        return Ok(dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingRequest request)
    {
        var booking = _mapper.Map<UpdateBookingRequest, Booking>(request);
        booking.Id = id;
        var updateBookingResult = await _bookingService.UpdateBookingAsync(booking);
        if (updateBookingResult.Failure)
        {
            return BadRequest(updateBookingResult.Error);
        }

        return Ok(_mapper.Map<Booking, BookingDetailsResponse>(updateBookingResult.Value));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> CancelBooking(int id, [FromBody] CancelBookingRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Reason))
            return BadRequest("Cancellation reason is required.");

        // Try to cancel via service (assumes Cancel/Refund support)
        var cancelResult = await _bookingService.CancelBookingAsync(id);
        if (cancelResult.Failure)
        {
            return BadRequest(cancelResult.Error);
        }

        var response = _mapper.Map<CancelBookingResponse>(cancelResult.Value);
        return Ok(response);
    }
}

#region DTOs

public class CreateBookingRequest
{
    [Required] public Guid SpaceId { get; set; }

    [Required] public DateTime StartTime { get; set; }

    [Required] public DateTime EndTime { get; set; }
}

public class BookingCreateResponse
{
    public Guid BookingId { get; set; }
    public string BookingReference { get; set; }
    public string Status { get; set; }
    public Guid SpaceId { get; set; }
    public string SpaceTitle { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double TotalHours { get; set; }
    public PricingDto Pricing { get; set; }
    public PaymentInitiationDto Payment { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class BookingDetailsResponse
{
    public Guid Id { get; set; }
    public string BookingReference { get; set; }
    public string Status { get; set; }
    public SpaceDto Space { get; set; }
    public RenterDto Renter { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double TotalHours { get; set; }
    public PricingDto Pricing { get; set; }
    public PaymentStatusDto Payment { get; set; }
    public AccessCodeDto AccessCode { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class BookingListResponse
{
    public List<BookingListItem> Data { get; set; }
    public PaginationDto Pagination { get; set; }
}

public class BookingListItem
{
    public Guid Id { get; set; }
    public string BookingReference { get; set; }
    public string Status { get; set; }
    public SpaceListDto Space { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
}

public class CancelBookingRequest
{
    [Required] public string Reason { get; set; }
}

public class CancelBookingResponse
{
    public Guid BookingId { get; set; }
    public string Status { get; set; }
    public RefundDto Refund { get; set; }
}

public class PricingDto
{
    public decimal HourlyRate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
}

public class PaymentInitiationDto
{
    public string PaymentUrl { get; set; }
    public string PaymentId { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class PaymentStatusDto
{
    public string Status { get; set; }
    public DateTime? PaidAt { get; set; }
}

public class AccessCodeDto
{
    public string Code { get; set; }
    public string QrCodeUrl { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
}

public class SpaceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
}

public class SpaceListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string PrimaryImage { get; set; }
}

public class RenterDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class RefundDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime ExpectedAt { get; set; }
}

public class PaginationDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public long Total { get; set; }
}

#endregion