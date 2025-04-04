﻿using MangoTreesAPI.Models;
using MangoTreesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MangoTreesAPI.Models.ResponseMessages;

namespace MangoTreesAPI.Controllers
{
    [ApiController]
    [Route("Product")]
    public class ProductController: ControllerBase
    {
        private readonly ProductService productService;
        private readonly IHttpContextAccessor httpContext;
        private readonly CustomerService customerService;
        public ProductController(ProductService _productService, CustomerService _customerService, IHttpContextAccessor _httpContext)
        {
            productService = _productService;
            customerService = _customerService;
            httpContext = _httpContext;
        }

        [HttpGet("All")]
        public async Task<ActionResult<List<ProductModel>>> GetAllProductData()
        {
            try
            {
                var products = await productService.GetAllProductsDataAsync();
                return Ok(products);
            }
            catch (Exception)
            {
                return BadRequest(new { Message = ResponseMessages.Response.TryAgainLater });
            }
        }

        [HttpPost("Review")]
        [Authorize(Roles = "customer,management,admin")]
        public async Task<ActionResult> PostReviewData([FromBody] ProductReviewsModel reviewData, string productId, string orderItemId)
        {
            var userId = httpContext.HttpContext?.User.FindFirst("userId")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token");

            if (!ModelState.IsValid || string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(orderItemId) || string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            try
            {
                var Reviewer = await customerService.GetUserDataAsync(userId);
                reviewData.ReviewerName = Reviewer.FirstName;
                var productData = await productService.PostProductReviewAsync(reviewData, productId);
                if(productData != null)
                {
                    await customerService.UpdateOrderItemAsync(orderItemId, productId, true);
                    return Ok(new { Message = ResponseMessages.Response.Success.ToString() });
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

        [HttpPost("GetReviews")]
        public async Task<ActionResult<ProductReviewsModel[]>> GetAllReviewsData([FromBody] string[] reviewIds)
        {
            try
            {
                var reviews = await productService.GetProductReviewsAsync(reviewIds);
                return Ok(reviews);
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpGet("ProductInfo")]
        public async Task<ActionResult> GetProductInfoData(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest();
            }
            try
            {
                var productData = await productService.GetProductDataAsync(productId);
                var productInfoData = await productService.GetProductInfoDataAsync(productData.ProductInfo);
                var productReviewData = await productService.GetProductReviewsAsync(productData.ProductReviews.ToArray());
                return Ok(new { ProductData = productData, ProductInfo = productInfoData, ProductReviewData = productReviewData });
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("ProductInfoList")]
        public async Task<ActionResult> GetProductInfoListData([FromBody] string[] productIds)
        {
            if (productIds.Length < 1)
            {
                return BadRequest();
            }
            try
            {
                var data = await productService.GetProductInfoListDataAsync(productIds);
                return Ok(data);
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpGet("Data")]
        public async Task<ActionResult> GetProductData(string productInfoId)
        {
            if (string.IsNullOrEmpty(productInfoId))
            {
                return BadRequest();
            }
            try
            {
                var productData = await productService.GetProductDataAsync(productInfoId);
                if(productData.IsInUse == false)
                {
                    productData.Availability = false;
                }
                return Ok(productData);
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpGet("Charges")]
        public async Task<ActionResult> GetDeliveryAndPackagingCostData(int Pincode)
        {
            if (Pincode <= 0 || Pincode.ToString().Length != 6)
            {
                return BadRequest();
            }
            try
            {
                var productData = await productService.GetDeliveryAndPackagingCostDataAsync(Pincode);
                if (productData != null)
                {
                    return Ok(productData);
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

        [HttpPost("Inventory")]
        public async Task<ActionResult> GetProductInventoryData([FromBody] string[] inventoryIdList)
        {
            if (inventoryIdList == null)
            {
                return BadRequest();
            }
            try
            {
                var inventoryData = await productService.GetInventoryListDataAsync(inventoryIdList);
                return Ok(inventoryData);
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpGet("PromotionCode")]
        [Authorize(Roles = "customer,management,admin")]
        public async Task<IActionResult> ValidatePromotionCode(string promotionCode, decimal amount)
        {
            if (string.IsNullOrEmpty(promotionCode) || amount<0)
            {
                return BadRequest();
            }
            try
            {
                var discountPercentage = await productService.ValidatePromotionCodeAsync(promotionCode, amount);
                if (discountPercentage > 0)
                {
                    return Ok(new { Message = PromotionCodeResponse.SuccessProperty1, DiscountPercentage = discountPercentage });
                }
                if (discountPercentage == -1)
                {
                    return Ok(new { Message = PromotionCodeResponse.FailureProperty1 });
                }
                else
                {
                    return Ok(new { Message = PromotionCodeResponse.FailureProperty2 });
                }
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }
    }
}
