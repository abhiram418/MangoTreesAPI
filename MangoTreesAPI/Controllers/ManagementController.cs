using MangoTreesAPI.Models;
using MangoTreesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MangoTreesAPI.Models.ResponseMessages;

namespace MangoTreesAPI.Controllers
{
    [ApiController]
    [Route("Management")]
    public class ManagementController: ControllerBase
    {
        private readonly AuthService authService;
        private readonly ProductService productService;
        private readonly ManagementService managementService;
        public ManagementController(ManagementService _managementService, ProductService _productService, AuthService _authService)
        {
            authService = _authService;
            productService = _productService;
            managementService = _managementService;
        }

        [HttpPost("Product")]
        [Authorize(Roles = "management,admin")]
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
        [Authorize(Roles = "management,admin")]
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

        [HttpPost("Information")]
        public async Task<IActionResult> PostInformationData(InformationModel informationData)
        {
            try
            {
                var InformationID = await managementService.PostInformationDataAsync(informationData);
                return Ok(InformationID);
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpGet("Information")]
        public async Task<IActionResult> GetInformationData(string informationID)
        {
            try
            {
                var Information = await managementService.GetInformationDataAsync(informationID);
                if(Information !=  null)
                {
                    return Ok(Information);
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
    }
}
