using AutoMapper;
using MangoTreesAPI.Models;
using MangoTreesAPI.Tables;

namespace MangoTreesAPI.DataAutoMapper
{
    public class CustomerDataMappingProfile: Profile
    {
        public CustomerDataMappingProfile()
        {

            CreateMap<UserRequestModel, UsersModels>();
            CreateMap<UsersModels, UserRequestModel>();

            CreateMap<UsersCollection, UsersModels>();
            CreateMap<UsersModels, UsersCollection>();

            CreateMap<CartModel, CartCollection>();
            CreateMap<CartCollection, CartModel>();

            CreateMap<AddressModel, AddressCollection>();
            CreateMap<AddressCollection, AddressModel>();

            CreateMap<AddressCollection, AddressResponseModel>();
            CreateMap<AddressResponseModel, AddressCollection>();

            CreateMap<AddressModel, AddressResponseModel>();
            CreateMap<AddressResponseModel, AddressModel>();

            CreateMap<UserAuthenticationModel, UsersCollection>();
            CreateMap<UsersCollection, UserAuthenticationModel> ();

            CreateMap<OrderRequestModel, OrderModel>();
            CreateMap<OrderModel, OrderRequestModel>();

            CreateMap<OrderResponseModel, OrderModel>()
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
            CreateMap<OrderModel, OrderResponseModel>()
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());


            CreateMap<OrderModel, OrderCollection>();
            CreateMap<OrderCollection, OrderModel>();

            CreateMap<DeliveryMethodModel, DeliveryMethodCollection>();
            CreateMap<DeliveryMethodCollection, DeliveryMethodModel>();

            CreateMap<OrderItemModel, OrderItemCollection>();
            CreateMap<OrderItemCollection, OrderItemModel>();

        }
    }
}
