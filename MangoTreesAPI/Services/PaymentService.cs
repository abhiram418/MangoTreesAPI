using MangoTreesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MangoTreesAPI.Services
{
    public class PaymentService
    {
        private readonly CustomerService customerService;
        public PaymentService(CustomerService _customerService)
        {
            customerService = _customerService;
        }

        public async Task ApplyOrderStatusAsync(string orderId, OrderStatusEnum orderStatus)
        {
            await customerService.UpdateOrderAsync(orderId, orderStatus);
        }
    }
}
