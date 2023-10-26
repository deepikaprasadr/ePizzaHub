using ePizzaHub.Core.Entities;
using Razorpay.Api;     //from Razorpay.core pkg   //from nugetPkgMgr

namespace ePizzaHub.Services.Interfaces
{
    public interface IPaymentService
    {//from Razorpay.core pkg
        string CreateOrder(decimal amount, string currency, string receipt);
        Payment GetPaymentDetails(string paymentId); //from Razorpay.core pkg
        bool VerifySignature(string signature, string orderId, string paymentId);
        int SavePaymentDetails(PaymentDetail model);
    }
}
