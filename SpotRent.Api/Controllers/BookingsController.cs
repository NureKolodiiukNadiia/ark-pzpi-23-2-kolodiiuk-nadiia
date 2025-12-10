using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpotRent.Services.Interfaces;
using SpotRent.Api.Dtos;
using SpotRent.Api.Dtos.Bookings;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Extensions;
using SpotRent.Services.Bookings;
using SpotRent.Api.Logging;
using SpotRent.Domain.Enums;

namespace SpotRent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : BaseController<BookingsController>
{
    private readonly IBookingService _bookingService;

    public BookingsController(
        IBookingService bookingService,
        ILogger<BookingsController> logger)
        : base(logger)
    {
        _bookingService = bookingService;
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("{userId:int}")]
    [EndpointSummary("Creates a new booking for a user.")]
    [EndpointDescription("Validates the booking payload for the specified user and persists the reservation when the request is valid.")]
    public async Task<IActionResult> CreateBooking(int userId, CreateBookingRequest request)
    {
        Log(LogLevel.Information, BookingsControllerEventIds.CreateBookingAttempt,
            "Create booking attempt for user {UserId}", userId);

        if (userId < 1 || !request.IsValid())
        {
            Log(LogLevel.Warning, BookingsControllerEventIds.CreateBookingInvalid,
                "Create booking request is not valid");

            return StatusCode(StatusCodes.Status400BadRequest, "Request is not valid");
        }

        var result = await _bookingService.CreateBookingAsync(userId, request);
        result.OnSuccess(() =>
                Log(LogLevel.Information, BookingsControllerEventIds.CreateBookingSuccess,
                    "Successfully created booking"))
            .OnFailure(() =>
                Log(LogLevel.Error, BookingsControllerEventIds.CreateBookingFailure,
                    "Failed to create booking. Error: {Error}", result.Error));

        return result.Failure
            ? StatusCode(StatusCodes.Status500InternalServerError, result.Error)
            : StatusCode(StatusCodes.Status201Created, result.Value);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("users/{userId}/history")]
    // [Authorize(Roles = "User")]
    [EndpointSummary("Gets the historical bookings for a user.")]
    [EndpointDescription("Validates the user identifier and returns the full booking history including pagination metadata.")]
    public async Task<IActionResult> GetUserBookingsHistory(int userId)
    {
        Log(LogLevel.Information, BookingsControllerEventIds.GetUserBookingsHistoryAttempt,
            "Get user bookings history attempt for user {UserId}", userId);

        if (userId < 1)
        {
            Log(LogLevel.Warning, BookingsControllerEventIds.GetUserBookingsHistoryInvalid,
                "User ID is not valid");

            return StatusCode(StatusCodes.Status400BadRequest, "Id is not valid");
        }

        var result = await _bookingService.GetUserBookingsHistoryAsync(userId);
        result.OnSuccess(() =>
                Log(LogLevel.Information, BookingsControllerEventIds.GetUserBookingsHistorySuccess,
                    "Successfully retrieved user bookings history"))
            .OnFailure(() =>
                Log(LogLevel.Error, BookingsControllerEventIds.GetUserBookingsHistoryFailure,
                    "Failed to retrieve user bookings history. Error: {Error}", result.Error));

        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        List<BookingDetailsResponse> dtos = new();
        foreach (var booking in result.Value)
        {
            dtos.Add(BookingDetailsResponse.MapBooking(booking));
        }

        var dto = new
        {
            Data = dtos,
            Pagination = new PaginationDto
            {
                Page = 1,
                PageSize = dtos.Count,
                Total = dtos.Count
            }
        };

        return StatusCode(StatusCodes.Status200OK, dto);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("users/{userId}/active")]
    // [Authorize(Roles = "User")]
    [EndpointSummary("Lists active bookings for a user.")]
    [EndpointDescription("Returns all in-progress or upcoming bookings for the specified user after validating the identifier.")]
    public async Task<IActionResult> GetUserActiveBookings(int userId)
    {
        Log(LogLevel.Information, BookingsControllerEventIds.GetUserActiveBookingsAttempt,
            "Get user active bookings attempt for user {UserId}", userId);

        if (userId < 1)
        {
            Log(LogLevel.Warning, BookingsControllerEventIds.GetUserActiveBookingsInvalid,
                "User ID is not valid");

            return StatusCode(StatusCodes.Status400BadRequest, "Id is not valid");
        }

        var result = await _bookingService.GetUserActiveBookingsAsync(userId);
        result.OnSuccess(() =>
                Log(LogLevel.Information, BookingsControllerEventIds.GetUserActiveBookingsSuccess,
                    "Successfully retrieved user active bookings"))
            .OnFailure(() =>
                Log(LogLevel.Error, BookingsControllerEventIds.GetUserActiveBookingsFailure,
                    "Failed to retrieve user active bookings. Error: {Error}", result.Error));

        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        List<BookingDetailsResponse> dtos = new();
        foreach (var booking in result.Value)
        {
            dtos.Add(BookingDetailsResponse.MapBooking(booking));
        }

        var dto = new
        {
            Data = dtos,
            Pagination = new PaginationDto
            {
                Page = 1,
                PageSize = dtos.Count,
                Total = dtos.Count
            }
        };

        return StatusCode(StatusCodes.Status200OK, dto);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("owners/{ownerId}")]
    // [Authorize(Roles = "Owner")]
    [EndpointSummary("Gets bookings for an owner across their spaces.")]
    [EndpointDescription("Retrieves every booking tied to the owner's spaces after confirming the owner identifier is valid.")]
    public async Task<IActionResult> GetOwnerBookings(int ownerId)
    {
        Log(LogLevel.Information, BookingsControllerEventIds.GetOwnerBookingsAttempt,
            "Get owner bookings attempt for owner {OwnerId}", ownerId);

        if (ownerId < 1)
        {
            Log(LogLevel.Warning, BookingsControllerEventIds.GetOwnerBookingsInvalid,
                "Owner ID is not valid");

            return StatusCode(StatusCodes.Status400BadRequest, ownerId);
        }

        var result = await _bookingService.GetOwnerBookingsAsync(ownerId);
        result.OnSuccess(() =>
                Log(LogLevel.Information, BookingsControllerEventIds.GetOwnerBookingsSuccess,
                    "Successfully retrieved owner bookings"))
            .OnFailure(() =>
                Log(LogLevel.Error, BookingsControllerEventIds.GetOwnerBookingsFailure,
                    "Failed to retrieve owner bookings. Error: {Error}", result.Error));

        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        List<BookingDetailsResponse> dtos = new();
        foreach (var booking in result.Value)
        {
            dtos.Add(BookingDetailsResponse.MapBooking(booking));
        }

        var dto = new
        {
            Data = dtos,
            Pagination = new PaginationDto
            {
                Page = 1,
                PageSize = dtos.Count,
                Total = dtos.Count
            }
        };

        return StatusCode(StatusCodes.Status200OK, dto);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("owners/{ownerId}/active")]
    // [Authorize(Roles = "Owner")]
    [EndpointSummary("Gets active bookings for an owner.")]
    [EndpointDescription("Returns only active bookings associated with the owner's spaces once the owner ID is validated.")]
    public async Task<IActionResult> GetOwnerActiveBookings(int ownerId)
    {
        Log(LogLevel.Information, BookingsControllerEventIds.GetOwnerBookingsAttempt,
            "Get owner active bookings attempt for owner {OwnerId}", ownerId);

        if (ownerId < 1)
        {
            Log(LogLevel.Warning, BookingsControllerEventIds.GetOwnerBookingsInvalid,
                "Owner ID is not valid");

            return StatusCode(StatusCodes.Status400BadRequest, ownerId);
        }

        var result = await _bookingService.GetOwnerActiveBookingsAsync(ownerId);
        result.OnSuccess(() =>
                Log(LogLevel.Information, BookingsControllerEventIds.GetOwnerBookingsSuccess,
                    "Successfully retrieved owner active bookings"))
            .OnFailure(() =>
                Log(LogLevel.Error, BookingsControllerEventIds.GetOwnerBookingsFailure,
                    "Failed to retrieve owner active bookings. Error: {Error}", result.Error));

        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        List<BookingDetailsResponse> dtos = new();
        foreach (var booking in result.Value)
        {
            dtos.Add(BookingDetailsResponse.MapBooking(booking));
        }

        var dto = new
        {
            Data = dtos,
            Pagination = new PaginationDto
            {
                Page = 1,
                PageSize = dtos.Count,
                Total = dtos.Count
            }
        };

        return StatusCode(StatusCodes.Status200OK, dto);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("filter/{userId:int}")]
    // [Authorize(Roles = "User, Owner")]
    [EndpointSummary("Filters bookings with paging and sorting options.")]
    [EndpointDescription("Supports filtering by user, space, time range, status, and payment information to return the relevant bookings page.")]
    public async Task<IActionResult> GetBookings(
        [FromQuery] int userId,
        [FromQuery] int? spaceId,
        [FromQuery] DateTime? startTime,
        [FromQuery] DateTime? endTime,
        [FromQuery] BookingStatus status,
        [FromQuery] PaymentStatus paymentStatus,
        [FromQuery] string sort,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        Log(LogLevel.Information, BookingsControllerEventIds.GetBookingsAttempt, "Get bookings attempt");
        var requesterRole = HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                            ?? HttpContext?.User?.FindFirst("role")?.Value
                            ?? "Unknown";

        Log(LogLevel.Debug, BookingsControllerEventIds.GetBookingsAttempt, "Requester role: {Role}", requesterRole);
        var orderBy = BuildOrderByDelegate(sort);
        var skipCount = (page - 1) * pageSize;
        int? takeCount = pageSize;

        var result = await _bookingService.GetBookingsAsync(new BookingFilterRequest
        {
            Role = requesterRole,
            UserId = userId,
            SpaceId = spaceId,
            StartTime = startTime,
            EndTime = endTime,
            Status = status,
            PaymentStatus = paymentStatus,
            OrderBy = orderBy,
            SkipCount = skipCount,
            TakeCount = takeCount
        });
        result.OnSuccess(() =>
                Log(LogLevel.Information, BookingsControllerEventIds.GetBookingsSuccess,
                    "Successfully retrieved bookings"))
            .OnFailure(() =>
                Log(LogLevel.Error, BookingsControllerEventIds.GetBookingsFailure,
                    "Failed to retrieve bookings. Error: {Error}", result.Error));

        switch (result.Failure)
        {
            case true when result.Error == "Unauthorized":
                return StatusCode(StatusCodes.Status401Unauthorized);
            case true:
                return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        var bookings = result.Value.Select(BookingDetailsResponse.MapBooking);
        var total = result.Value.Count();
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return StatusCode(StatusCodes.Status200OK, new
        {
            Bookings = bookings, Total = total, TotalPages = totalPages, Page = page, PageSize = pageSize
        });
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id:int}")]
    [EndpointSummary("Retrieves booking details by identifier.")]
    [EndpointDescription("Validates the booking ID, loads the booking from storage, and returns its details if found.")]
    public async Task<IActionResult> GetBooking(int id)
    {
        Log(LogLevel.Information, BookingsControllerEventIds.GetBookingAttempt,
            "Get booking attempt for ID {Id}", id);

        if (id < 1)
        {
            Log(LogLevel.Warning, BookingsControllerEventIds.GetBookingInvalid, "Booking ID is not valid");

            return StatusCode(StatusCodes.Status400BadRequest, "Id is not valid");
        }

        var result = await _bookingService.GetBookingByIdAsync(id);
        result.OnSuccess(() =>
                Log(LogLevel.Information, BookingsControllerEventIds.GetBookingSuccess,
                    "Successfully retrieved booking"))
            .OnFailure(() =>
                Log(LogLevel.Error, BookingsControllerEventIds.GetBookingFailure,
                    "Failed to retrieve booking. Error: {Error}", result.Error));

        if (result.Failure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        var dto = BookingDetailsResponse.MapBooking(result.Value);

        return StatusCode(StatusCodes.Status200OK, dto);
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id:int}")]
    [EndpointSummary("Cancels a booking.")]
    [EndpointDescription("Intended to cancel the specified booking; currently returns a placeholder response.")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        Log(LogLevel.Information, BookingsControllerEventIds.CancelBookingAttempt,
            "Cancel booking attempt for ID {Id}", id);

        return StatusCode(418);
    }

    private Func<IQueryable<Booking>, IOrderedQueryable<Booking>> BuildOrderByDelegate(string sort)
    {
        Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy = sort switch
        {
            "start-time-asc" => q => q.OrderBy(s => s.StartTime),
            "start-time-desc" => q => q.OrderByDescending(s => s.StartTime),
            "newest" => q => q.OrderByDescending(s => s.CreatedAt),
            _ => q => q.OrderBy(s => s.CreatedAt)
        };

        return orderBy;
    }
}
