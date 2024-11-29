using MangoTreesAPI.Models;
using MangoTreesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangoTreesAPI.Controllers
{
    [ApiController]
    [Route("Customer")]
    [Authorize(Roles = "customer,management,admin")]
    public class CustomerController: ControllerBase
    {
        private readonly ProductService productService;
        private readonly AuthService authService;
        private readonly CustomerService customerService;
        private readonly IHttpContextAccessor httpContext;
        public CustomerController(ProductService _productService, CustomerService _customerService, AuthService _authService, OtpService _otpService, IHttpContextAccessor _httpContext)
        {
            productService = _productService;
            authService = _authService;
            customerService = _customerService;
            httpContext = _httpContext;
        }

        [HttpGet]
        public async Task<ActionResult<UsersModels>> GetUserDetails()
        {
            var userId = httpContext.HttpContext?.User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token");
            try
            {
                var user = await customerService.GetUserDataAsync(userId);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
                }
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("EditCustomerDetails")]
        public async Task<ActionResult> UpdateCustomerDetails([FromBody] UserEditDetailsModel userDetails)
        {
            var userId = httpContext.HttpContext?.User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token");

            if (userDetails == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await customerService.UpdateCustomerDetailsAsync(userDetails, userId);
                return Ok(new { Message =  ResponseMessages.UpdateCustomerDetails.SuccessProperty1});
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpGet("Cart")]
        public async Task<ActionResult<ProductModel[]>> GetCartData(string cartId)
        {
            if (string.IsNullOrEmpty(cartId))
            {
                return BadRequest();
            }
            try
            {
                var cart = await customerService.GetCartAsync(cartId);
                var productList = await productService.GetProductListDataAsync(cart.Items);
                return Ok(productList);
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("Cart")]
        public async Task<ActionResult> UpdateCartData([FromBody] string[] items, string cartId)
        {
            if (string.IsNullOrEmpty(cartId) || items == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var CartItems = await customerService.AddToCartAsync(cartId, items);
                return Ok(new { CartItems = CartItems });
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpGet("Addresses")]
        public async Task<ActionResult<AddressResponseModel[]>> GetUserAddressData()
        {
            var userId = httpContext.HttpContext?.User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token");
            try
            {
                var addressList = await customerService.GetAddressListAsync(userId);
                return Ok(addressList);
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("Address")]
        public async Task<ActionResult> AddUserAddressData([FromBody] AddressModel addressData)
        {
            var userId = httpContext.HttpContext?.User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token");

            if (!ModelState.IsValid || addressData == null)
            {
                return BadRequest();
            }
            try
            {
                var addressId = await customerService.AddAddressAsync(addressData, userId);
                return Ok(new { AddressId = addressId });
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("UpdateAddress")]
        public async Task<ActionResult> UpdateUserAddressData([FromBody] AddressModel addressData, string addressId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await customerService.UpdateAddressAsync(addressData, addressId);
                return Ok(new { Message = ResponseMessages.Response.Success.ToString() });
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpDelete("Address")]
        public async Task<ActionResult> DeleteUserAddressData(string addressId)
        {
            var userId = httpContext.HttpContext?.User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token");

            try
            {
                await customerService.DeleteAddressAsync(addressId, userId);
                return Ok(new { Message = ResponseMessages.Response.Success.ToString() });
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpGet("Orders")]
        public async Task<ActionResult<OrderModel[]>> GetUserOrdersData()
        {
            var userId = httpContext.HttpContext?.User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token");

            try
            {
                var orders = await customerService.GetOrdersListAsync(userId);
                return Ok(orders);
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("Order")]
        public async Task<ActionResult<bool>> AddUserOrderData([FromBody] OrderRequestModel orderData)
        {
            var userId = httpContext.HttpContext?.User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token");

            if (!ModelState.IsValid || orderData == null)
            {
                return BadRequest();
            }

            try
            {
                var status = await customerService.PostOrderAsync(orderData, userId);
                return Ok(new { Message = ResponseMessages.Response.Success });
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpGet("Order/Receipt")]
        public async Task<ActionResult> GetReceiptData(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return BadRequest();
            }

            try
            {
                var Transaction = await productService.GetReceiptDataAsync(orderId);
                return Ok(Transaction);
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }
    }
}
