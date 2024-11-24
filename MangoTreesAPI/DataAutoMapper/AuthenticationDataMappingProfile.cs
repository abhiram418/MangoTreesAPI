using AutoMapper;
using MangoTreesAPI.Models;
using MangoTreesAPI.Tables;

namespace MangoTreesAPI.DataAutoMapper
{
    public class AuthenticationDataMappingProfile : Profile
    {
        public AuthenticationDataMappingProfile() {

            CreateMap<RolesModel, RolesCollection>();
            CreateMap<RolesCollection, RolesModel>();

            CreateMap<UserAuthenticationCollection, UserAuthenticationModel>();
            CreateMap<UserAuthenticationModel, UserAuthenticationCollection>();

            CreateMap<OTPCollection, OTPModel>();
            CreateMap<OTPModel, OTPCollection>();

        }
    }
}
