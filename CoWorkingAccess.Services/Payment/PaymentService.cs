using CoWorkingAccess.Domain.Common;
using CoWorkingAccess.Domain.Enums;
using CoWorkingAccess.Domain.Interfaces;
using CoWorkingAccess.Infrastructure;

namespace CoWorkingAccess.Services.Payment;

public class PaymentService : IPaymentService
{
    private readonly CoWorkingAccessDbContext _context;

    public PaymentService(CoWorkingAccessDbContext context)
    {
        _context = context;
    }

    public async Task<Result> UpdatePaymentStatus(int paymentId, long transactionId, string status)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        if (payment is not null)
        {
            payment.Status = status switch
            {
                "TestPaid" => PaymentStatus.TestPaid,
                "Paid" => PaymentStatus.Paid,
                "Failed" => PaymentStatus.Failed,
                _ => payment.Status
            };
            // payment.TransactionId = transactionId;
            payment.ProcessedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Result.Success();
        }

        return Result.Fail($"No payment {paymentId} is in db");
    }
}
