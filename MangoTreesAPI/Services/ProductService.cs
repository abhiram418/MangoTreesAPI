using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using MangoTreesAPI.Models;
using MangoTreesAPI.Tables;

namespace MangoTreesAPI.Services
{
    public class ProductService 
    {
        private readonly AwsS3BucketService awsS3;
        private readonly IDynamoDBContext context;
        private readonly IMapper mapper;
        public ProductService(AwsS3BucketService _awsS3, IDynamoDBContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            awsS3 = _awsS3;
        }

        public async Task<string> PostProductReviewAsync(ProductReviewsModel productReviewData, string productId)
        {
            var review = mapper.Map<ProductReviewsCollection>(productReviewData);
            var productData = await GetProductDataAsync(productId);
            productData.ProductReviews.Add(review.ProductReviewsId);
            await UpdateProductDataAsync(productData, productId);
            await context.SaveAsync(review);
            return review.ProductReviewsId;
        }
        public async Task<List<ProductReviewsModel>> GetProductReviewsAsync(string[] reviewsIds)
        {
            var reviewsListData = new List<ProductReviewsModel>();
            foreach (var id in reviewsIds)
            {
                var review = await GetProductReviewAsync(id);
                if (review != null)
                {
                    reviewsListData.Add(review);
                }
            }

            return reviewsListData;
        }
        private async Task<ProductReviewsModel> GetProductReviewAsync(string reviewId)
        {
            var reviewData = await context.LoadAsync<ProductReviewsCollection>(reviewId);
            var review = mapper.Map<ProductReviewsModel>(reviewData);
            return review;
        }

        public async Task<string> PostProductInfoDataAsync(ProductInfoModel productInfoData)
        {
            var product = mapper.Map<ProductInfoCollection>(productInfoData);
            await context.SaveAsync(product);
            return product.ProductInfoId;
        }
        public async Task<ProductInfoModel> GetProductInfoDataAsync(string productInfoId)
        {
            var productInfoData = await context.LoadAsync<ProductInfoCollection>(productInfoId);
            var productInfo = mapper.Map<ProductInfoModel>(productInfoData);
            return productInfo;
        }
        public async Task<ProductInfoResponceModel[]> GetProductInfoListDataAsync(string[] productIdsList)
        {
            var productInfoListData = new List<ProductInfoResponceModel>();
            foreach (var id in productIdsList)
            {
                var productData = await GetProductDataAsync(id);
                var productInfoData = await GetProductInfoDataAsync(productData.ProductInfo);
                var productInfo = mapper.Map<ProductInfoResponceModel>(productInfoData);
                productInfo.ProductId = productData.ProductId!;
                productInfoListData.Add(productInfo);
            }
            return productInfoListData.ToArray();
        }

        private async Task<string[]> GetNutritionFactsAsync(string productId)
        {
            var productData = await GetProductDataAsync(productId);
            return [.. productData.NutritionFacts];
        }

        public async Task<string> PostProductDataAsync(ProductRequestModel productRequestData)
        {
            var productData = mapper.Map<ProductModel>(productRequestData);
            productData.ProductInfo = await PostProductInfoDataAsync(productRequestData.ProductInfo);
            var product = mapper.Map<ProductCollection>(productData);
            product.Images = await awsS3.PostTheImagesAndGetUrls(product.ProductId, productRequestData.Images);
            product.InventoryId = await PostInventoryDataAsync(productRequestData.Inventory, product.ProductId);
            await context.SaveAsync(product);
            return product.ProductId;
        }
        private async Task UpdateProductDataAsync(ProductModel productData, string productId)
        {
            var product = mapper.Map<ProductCollection>(productData);
            product.ProductId = productId;
            await context.SaveAsync(product);
        }
        private async Task UpdateProductToOutOfStockDataAsync(string productId)
        {
            var productData = await GetProductDataAsync(productId);
            productData.Availability = false;
            await UpdateProductDataAsync(productData, productId);
        }
        public async Task<List<ProductModel>> GetProductListDataAsync(string[] productIds)
        {
            var productListData = new List<ProductModel>();
            foreach (var id in productIds)
            {
                var productData = await GetProductDataAsync(id);
                if (productData != null)
                {
                    productListData.Add(productData);
                }
            }

            return productListData;
        }
        public async Task<ProductModel> GetProductDataAsync(string productId)
        {
            var productData = await context.LoadAsync<ProductCollection>(productId);
            var product = mapper.Map<ProductModel>(productData);
            return product;
        }
        public async Task<List<ProductModel>> GetAllProductsDataAsync()
        {
            var scanConditions = new List<ScanCondition>
            {
                new ScanCondition("IsInUse", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, true)
            };
            var allProductsData = await context.ScanAsync<ProductCollection>(scanConditions).GetRemainingAsync();
            var allProducts = mapper.Map<List<ProductModel>>(allProductsData);
            return allProducts;
        }

        private async Task<string> PostInventoryDataAsync(InventoryRequestModel inventoryRequestData, string productId)
        {
            var inventoryData = mapper.Map<InventoryModel>(inventoryRequestData);
            inventoryData.ProductId = productId;
            inventoryData.StockAddedDate = DateTime.UtcNow;
            var inventory = mapper.Map<InventoryCollection>(inventoryData);
            await context.SaveAsync(inventory);
            return inventory.InventoryId;
        }
        public async Task UpdateInventoryDataAsync(string productId, int quantity)
        {
            var productData = await GetProductDataAsync(productId);
            var inventoryId = productData.InventoryId;
            var inventoryData = await context.LoadAsync<InventoryCollection>(inventoryId);
            if (inventoryData.StockQuantity > quantity)
            {
                inventoryData.StockQuantity = inventoryData.StockQuantity - quantity;
            }
            else
            {
                inventoryData.StockQuantity = 0;
                await UpdateProductToOutOfStockDataAsync(productId);
            }
            await context.SaveAsync(inventoryData);
        }
        public async Task<InventoryModel> GetInventoryDataAsync(string inventoryId)
        {
            var inventoryData = await context.LoadAsync<InventoryCollection>(inventoryId);
            var inventory = mapper.Map<InventoryModel>(inventoryData);
            return inventory;
        }
        public async Task<InventoryModel[]> GetInventoryListDataAsync(string[] inventoryIdList)
        {
            var InventoryList = new List<InventoryModel>();
            foreach (var inventoryId in inventoryIdList)
            {
                var inventory = await GetInventoryDataAsync(inventoryId);
                InventoryList.Add(inventory);
            }
            return InventoryList.ToArray();
        }

        public async Task<int> ValidatePromotionCodeAsync(string promotionCode, decimal amount)
        {
            var promotionCodeData = await context.LoadAsync<PromotionCollection>(promotionCode);
            if(promotionCodeData != null && promotionCodeData.IsActive && promotionCodeData.ValidFrom < DateTime.Now && promotionCodeData.ValidUntil > DateTime.Now) {
                if(promotionCodeData.ApplicablePrice > amount)
                {
                    return 0;
                }
                else
                {
                    return promotionCodeData.DiscountPercentage;
                }
            }
            else
            {
                return -1;
            }
        }
        public async Task PostPromotionCodeAsync(PromotionModel promotionData)
        {
            var promotion = mapper.Map<PromotionCollection>(promotionData);
            await context.SaveAsync(promotion);
        }

        public async Task<ChargesModel> GetDeliveryAndPackagingCostDataAsync(int pincode)
        {
            var ChargesData = await context.LoadAsync<ChargesCollection>(pincode);
            var charges = mapper.Map<ChargesModel>(ChargesData);
            return charges;
        }
    }
}
    