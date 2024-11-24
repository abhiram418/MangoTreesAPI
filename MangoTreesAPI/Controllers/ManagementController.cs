using MangoTreesAPI.Models;
using MangoTreesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MangoTreesAPI.Models.ResponseMessages;

namespace MangoTreesAPI.Controllers
{
    [ApiController]
    public class ManagementController: ControllerBase
    {
        private readonly AuthService authService;
        private readonly CustomerService customerService;
        private readonly ProductService productService;
        public ManagementController(ProductService _productService, CustomerService _customerService, AuthService _authService)
        {
            authService = _authService;
            customerService = _customerService;
            productService = _productService;
        }

        [HttpPost("Product")]
        [Authorize(Roles = "management")]
        public async Task<ActionResult> PostProductData(ProductRequestModel productData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var productId = await productService.PostProductDataAsync(productData);
                return Ok(new {ProductId = productId});
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("roles")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<RolesModel>> PostRole(string RoleName)
        {
            try
            {
                var role = await authService.AddRoleDataAsync(RoleName);
                return role;
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("PromotionCode")]
        //[Authorize(Roles = "management")]
        public async Task<ActionResult> PostPromotionCode(PromotionModel promotionData)
        {
            try
            {
                await productService.PostPromotionCodeAsync(promotionData);
                return Ok(new { Message = PromotionCodeResponse.SuccessProperty2 });
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }
    }
}
