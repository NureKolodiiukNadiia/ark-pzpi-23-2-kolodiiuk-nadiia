using Microsoft.Extensions.Logging;
using SpotRent.Domain.Common;
using SpotRent.Domain.Enums;
using SpotRent.Infrastructure;
using SpotRent.Services.Interfaces;

namespace SpotRent.Services.Payment;

public class PaymentService : BaseService<PaymentService>, IPaymentService
{
    private readonly LiqPayHelper _liqPayHelper;

    public PaymentService(SpotRentDbContext context, LiqPayHelper liqPayHelper, ILogger<PaymentService> logger)
        : base(context, logger)
    {
        _liqPayHelper = liqPayHelper;
    }

    public async Task<Result<LiqPayPaymentData>> CreatePayment(int orderId, decimal total)
    {
        try
        {
            var paymentData = _liqPayHelper.GeneratePaymentData(
                total,
                "UAH",
                "",
                orderId
            );

            return Result.Success(paymentData);
        }
        catch (Exception ex)
        {
            return Result.Fail<LiqPayPaymentData>($"Failure creating payment: {ex.Message}");
        }
    }

    public async Task<Result> UpdatePaymentStatus(int paymentId, long transactionId, string status)
    {
        throw new NotImplementedException();
        // var payment = await Context.Payments.FindAsync(paymentId);
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
        //     await Context.SaveChangesAsync();
        //
        //     return Result.Success();
        // }
        //
        // return Result.Fail($"No payment {paymentId} is in db");
    }
}