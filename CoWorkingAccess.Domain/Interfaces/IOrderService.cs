namespace CoWorkingAccess.Domain.Interfaces;

public interface IOrderService
{
    Task UpdateOrderStatus(int orderId, string status);
}