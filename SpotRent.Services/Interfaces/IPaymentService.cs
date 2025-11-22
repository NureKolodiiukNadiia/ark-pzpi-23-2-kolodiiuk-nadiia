using SpotRent.Domain.Common;
using SpotRent.Services.Payment;

namespace SpotRent.Services.Interfaces;

public interface IPaymentService
{
    Task<Result> UpdatePaymentStatus(int paymentId, long transactionId, string status);

    Task<Result<LiqPayPaymentData>> CreatePayment(int subscriptionId, decimal subscriptionTotalAmount);
}
