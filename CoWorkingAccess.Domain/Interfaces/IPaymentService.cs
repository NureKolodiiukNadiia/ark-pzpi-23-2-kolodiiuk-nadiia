using CoWorkingAccess.Domain.Common;

namespace CoWorkingAccess.Domain.Interfaces;

public interface IPaymentService
{
    Task<Result> UpdatePaymentStatus(int paymentId, long transactionId, string status);
}
