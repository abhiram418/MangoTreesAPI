using AutoMapper;
using MangoTreesAPI.Models;
using MangoTreesAPI.Tables;

namespace MangoTreesAPI.DataAutoMapper
{
    public class ManagementDataMappingProfile: Profile
    {
        public ManagementDataMappingProfile()
        {
            CreateMap<PromotionModel, PromotionCollection>();
            CreateMap<PromotionCollection, PromotionModel>();

            CreateMap<TransactionModel, TransactionCollection>();
            CreateMap<TransactionCollection, TransactionModel>();

            CreateMap<InformationModel, InformationCollection>();
            CreateMap<InformationCollection, InformationModel>();
        }
    }
}
