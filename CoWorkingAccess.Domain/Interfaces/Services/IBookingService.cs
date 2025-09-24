using CoWorkingAccess.Domain.Entities;

namespace CoWorkingAccess.Domain.Interfaces.Services;

public interface IBookingService
{
    Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);

    Task<IEnumerable<Booking>> GetAllBookingsAsync();

    Task<Booking?> GetBookingByIdAsync(int id);

    Task<Booking> CreateBookingAsync(Booking booking);

    Task<Booking> UpdateBookingAsync(Booking booking);

    Task<bool> CancelBookingAsync(int id);

    Task<decimal> CalculateBookingCostAsync(int workspaceId, DateTime startTime, DateTime endTime);
    Task<bool> IsBookingActiveAsync(int bookingId);
    Task<Booking?> GetActiveBookingAsync(int userId, int workspaceId);

    Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
    //Task<Order> PlaceOrderAsync(CreateOrderDto order);

    //Task<IEnumerable<Order>> GetAllOrdersAsync();

    //Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);

    //Task<Order> GetOrderAsync(int orderId);

    //Task UpdateOrderAsync(Order order);

    //Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId);
}
