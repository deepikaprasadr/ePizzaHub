using ePizzaHub.Core.Entities;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace ePizzaHub.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly RazorpayClient _client;
        ICartRepository _cartRepo;
        IConfiguration _configuration;
        IRepository<PaymentDetail> _paymentRepo;
        public PaymentService(IConfiguration configuration, IRepository<PaymentDetail> paymentRepo, ICartRepository cartRepo) { 
            _configuration= configuration;
            _paymentRepo= paymentRepo;
            _cartRepo= cartRepo;
            _client = new RazorpayClient(_configuration["RazorPay:Key"], _configuration["RazorPay:Secret"]);
        }
        public string CreateOrder(decimal amount, string currency, string receipt)
        {
            try
            {
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", amount); // amount in the smallest currency unit
                options.Add("receipt", receipt);
                options.Add("currency", "INR");
                Razorpay.Api.Order order = _client.Order.Create(options);
                return order["id"].ToString();  
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Payment GetPaymentDetails(string paymentId)
        {
           return _client.Payment.Fetch(paymentId);
        }

        public int SavePaymentDetails(PaymentDetail model)
        {
            _paymentRepo.Add(model);
            Cart cart = _cartRepo.Find(model.CartId);
            cart.IsActive= false;
            return _paymentRepo.SaveChanges();
        }

        public bool VerifySignature(string signature, string orderId, string paymentId)
        {
            string payload = string.Format("{0}|{1}", orderId, paymentId);
            string secret = RazorpayClient.Secret;
            string actualSignature = getActualSignature(payload, secret);
            return actualSignature.Equals(signature);
        }

        private static string getActualSignature(string payload, string secret)
        {
            byte[] secretBytes = StringEncode(secret);
            HMACSHA256 hashHmac = new HMACSHA256(secretBytes);
            var bytes = StringEncode(payload);

            return HashEncode(hashHmac.ComputeHash(bytes));
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
