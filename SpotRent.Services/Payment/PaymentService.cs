using SpotRent.Domain.Common;
using SpotRent.Domain.Enums;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Payment;

public class PaymentService : IPaymentService
{
    private readonly SpotRentDbContext _context;

    public PaymentService(SpotRentDbContext context)
    {
        _context = context;
    }

    public async Task<Result> UpdatePaymentStatus(int paymentId, long transactionId, string status)
    {
        throw new NotImplementedException();
        // var payment = await _context.Payments.FindAsync(paymentId);
        // if (payment is not null)
        // {
        //     payment.Status = status switch
        //     {
        //         "TestPaid" => PaymentStatus.TestPaid,
        //         "Paid" => PaymentStatus.Paid,
        //         "Failed" => PaymentStatus.Failed,
        //         _ => payment.Status
        //     };
        //     // payment.TransactionId = transactionId;
        //     payment.ProcessedAt = DateTime.UtcNow;
        //
        //     await _context.SaveChangesAsync();
        //
        //     return Result.Success();
        // }
        //
        // return Result.Fail($"No payment {paymentId} is in db");
    }
}
