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

    public async Task<Result<LiqPayPaymentData>> CreatePaymentAsync(int id, decimal total)
    {
        try
        {
            var paymentData = _liqPayHelper.GeneratePaymentData(
                total,
                "UAH",
                "",
                id
            );

            return Result.Success(paymentData);
        }
        catch (Exception ex)
        {
            return Result.Fail<LiqPayPaymentData>($"Failure creating payment: {ex.Message}");
        }
    }

    public async Task<Result> UpdatePaymentStatusSubscriptionAsync(int subscriptionId, long transactionId, string status)
    {
        var subscription = await Context.Subscriptions.FindAsync(subscriptionId);
        if (subscription is not null)
        {
            switch (status)
            {
                case "TestPaid":
                    subscription.PaymentStatus = PaymentStatus.TestPaid;
                    break;
                case "Paid":
                    subscription.PaymentStatus = PaymentStatus.Paid;
                    break;
                case "Failed":
                    subscription.PaymentStatus = PaymentStatus.Failed;
                    subscription.Status = SubscriptionStatus.Cancelled;
                    break;
                default:
                    subscription.PaymentStatus = subscription.PaymentStatus;
                    break;
            }

            subscription.TransactionId = transactionId;
            subscription.PaymentProcessedAt = DateTime.UtcNow;

            await Context.SaveChangesAsync();

            return Result.Success();
        }

        return Result.Fail($"No payment for {subscriptionId} is in db");
    }

    public async Task<Result> UpdatePaymentStatusBookingAsync(int bookingId, long transactionId, string status)
    {
        var booking = await Context.Bookings.FindAsync(bookingId);
        if (booking is not null)
        {
            switch (status)
            {
                case "TestPaid":
                    booking.PaymentStatus = PaymentStatus.TestPaid;
                    break;
                case "Paid":
                    booking.PaymentStatus = PaymentStatus.Paid;
                    break;
                case "Failed":
                    booking.PaymentStatus = PaymentStatus.Failed;
                    booking.Status = BookingStatus.Cancelled;
                    break;
                default:
                    booking.PaymentStatus = booking.PaymentStatus;
                    break;
            }

            booking.TransactionId = transactionId;
            booking.PaymentProcessedAt = DateTime.UtcNow;

            await Context.SaveChangesAsync();

            return Result.Success();
        }

        return Result.Fail($"No payment for {bookingId} is in db");
    }

    public async Task<Result<LiqPayRefundResponse>> RefundPaymentAsync(int id)
    {
        try
        {
            var result = await _liqPayHelper.RefundAsync<LiqPayRefundResponse>(id);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            return Result.Fail<LiqPayRefundResponse>($"Error refunding payment: {e.Message}");
        }
    }
}
