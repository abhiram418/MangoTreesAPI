using MangoTreesAPI.Models;
using MangoTreesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MangoTreesAPI.Controllers
{
    [ApiController]
    [Route("Payment")]
    public class PaymentController: ControllerBase
    {
        private readonly PaymentService paymentService;
        private readonly IHttpContextAccessor httpContext;
        public PaymentController(PaymentService _paymentService, IHttpContextAccessor _httpContext)
        {
            httpContext = _httpContext;
            paymentService = _paymentService;
        }

        [HttpPost("initiate")]
        public IActionResult InitiatePayment(int orderId)
        {
            return Ok("url");
        }

        [HttpPost("confirm")]
        public async void ConfirmPayment(string orderId, OrderStatusEnum orderStatus)
        {
            await paymentService.ApplyOrderStatusAsync(orderId, orderStatus);
        }
    }
}
