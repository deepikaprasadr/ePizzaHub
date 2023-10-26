using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ePizzaHub.UI.Controllers
{
    public class PaymentController : Controller
    {
        IConfiguration _configuration;
        IPaymentService _paymentService;
        IUserAccessor _userAccessor;
        IOrderService _orderService;
        public PaymentController(IConfiguration configuration, IPaymentService paymentService, IUserAccessor userAccessor, IOrderService orderService)
        {
            _configuration = configuration;
            _paymentService = paymentService;
            _userAccessor = userAccessor;
            _orderService = orderService;
        }
        UserModel CurrentUser
        {
            get
            {
                return _userAccessor.GetUser();
            }
        }
        public IActionResult Index()
        {
            PaymentModel payment = new PaymentModel();
            CartModel cart = TempData.Peek<CartModel>("Cart");
            if (cart != null)
            {
                payment.Cart = cart;
                payment.GrandTotal = cart.GrandTotal;
                payment.Currency = "INR";
                payment.Description = string.Join(",", cart.Items.Select(r => r.Name));
                payment.RazorpayKey = _configuration["Razorpay:Key"];
                payment.Receipt = Guid.NewGuid().ToString();
                payment.OrderId = _paymentService.CreateOrder(payment.GrandTotal * 100, payment.Currency, payment.Receipt);
            }

            return View(payment);

        }

        [HttpPost]
        public IActionResult Status(IFormCollection form)
        {
            try
            {
                if (form.Keys.Count > 0 && !string.IsNullOrWhiteSpace(form["rzp_paymentid"]))
                {
                    string paymentId = form["rzp_paymentid"];
                    string orderId = form["rzp_orderid"];
                    string signature = form["rzp_signature"];
                    string transactionId = form["Receipt"];
                    string currency = form["Currency"];

                    var payment = _paymentService.GetPaymentDetails(paymentId);
                    bool IsSignVerified = _paymentService.VerifySignature(signature, orderId, paymentId);

                    if (IsSignVerified && payment != null)
                    {
                        CartModel cart = TempData.Get<CartModel>("Cart");
                        PaymentDetail model = new PaymentDetail();

                        model.CartId = cart.Id;
                        model.Total = cart.Total;
                        model.Tax = cart.Tax;
                        model.GrandTotal = cart.GrandTotal;

                        model.Status = payment.Attributes["status"]; //captured
                        model.TransactionId = transactionId;
                        model.Currency = payment.Attributes["currency"];
                        model.Email = payment.Attributes["email"];
                        model.Id = paymentId;
                        model.UserId = CurrentUser.Id;

                        int status = _paymentService.SavePaymentDetails(model);
                        if (status > 0)
                        {
                            Response.Cookies.Append("CId", "");
                            AddressModel addressModel = TempData.Get<AddressModel>("Address");
                            //place order
                            _orderService.PlaceOrder(CurrentUser.Id, orderId, paymentId, cart, addressModel);
                            //TO DO: send email
                            TempData.Set("PaymentDetails", model);
                            return RedirectToAction("Receipt");
                        }
                        else
                        {
                            ViewBag.Message = "Although, due to some technical issues it's not get updated in our side. Get in touch with Deepika";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            ViewBag.Message = "Your payment has been failed. You can contact Deepikaprasad.";
            return View();
        }

        public IActionResult Receipt()
        {
            PaymentDetail model = TempData.Peek<PaymentDetail>("PaymentDetails");
            return View(model);
        }
    }
}
