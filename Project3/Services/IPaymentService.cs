namespace Project3.Services
{
    public interface IPaymentService
    {
        Task<string> InitiatePayment(double amount, string returnUrl, string notifyUrl);
        Task<bool> ConfirmPayment(string transactionId, string resultCode);
    }
}
