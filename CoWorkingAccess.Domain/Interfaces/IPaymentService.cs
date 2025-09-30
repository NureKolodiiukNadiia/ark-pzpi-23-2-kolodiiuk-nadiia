namespace CoWorkingAccess.Domain.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "USD");

        Task<bool> ConfirmPaymentAsync(string paymentId);

        Task<bool> IsPaymentSuccessfulAsync(string paymentId);
    }
}
