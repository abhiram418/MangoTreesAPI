using AutoMapper;
using MangoTreesAPI.Models;
using MangoTreesAPI.Tables;

namespace MangoTreesAPI.DataAutoMapper
{
    public class ProductDataMappingProfile: Profile
    {
        public ProductDataMappingProfile()
        {
            CreateMap<ProductModel, ProductRequestModel>();
            CreateMap<ProductRequestModel, ProductModel>();

            CreateMap<ProductModel, ProductCollection>();
            CreateMap<ProductCollection, ProductModel>();

            CreateMap<ProductInfoModel, ProductInfoCollection>();
            CreateMap<ProductInfoCollection, ProductInfoModel>();

            CreateMap<ProductInfoModel, ProductInfoResponceModel>();
            CreateMap<ProductInfoResponceModel, ProductInfoModel>();

            CreateMap<ProductReviewsModel, ProductReviewsCollection>();
            CreateMap<ProductReviewsCollection, ProductReviewsModel>();

            CreateMap<InventoryModel, InventoryRequestModel>();
            CreateMap<InventoryRequestModel, InventoryModel>();

            CreateMap<InventoryModel, InventoryCollection>();
            CreateMap<InventoryCollection, InventoryModel>();

            CreateMap<ChargesModel, ChargesCollection>();
            CreateMap<ChargesCollection, ChargesModel>();
        }
    }
}
