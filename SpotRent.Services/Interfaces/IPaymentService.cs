using SpotRent.Domain.Common;

namespace SpotRent.Services.Interfaces;

public interface IPaymentService
{
    Task<Result> UpdatePaymentStatus(int paymentId, long transactionId, string status);
}
