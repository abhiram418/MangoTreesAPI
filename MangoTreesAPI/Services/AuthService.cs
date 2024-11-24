using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using MangoTreesAPI.Models;
using MangoTreesAPI.Tables;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace MangoTreesAPI.Services
{
    public class AuthService
    {
        private readonly GeneralService generalService;
        private readonly AuthenticationOptions AuthOptions;
        private readonly IDynamoDBContext context;
        private readonly IMapper mapper;
        public AuthService(GeneralService _generalService,IDynamoDBContext _context,  IMapper _mapper, IOptions<AuthenticationOptions> _AuthOptions)
        {
            AuthOptions = _AuthOptions.Value;
            context = _context;
            mapper = _mapper;
            generalService = _generalService;
        }

        public async Task<string> GenerateTokenAsync(string UserName, string UserId, string Role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (string.IsNullOrEmpty(AuthOptions.SecretKey))
            {
                throw new InvalidOperationException("JWT secret key is not configured");
            }

            var SecretKey_Bytes = Encoding.UTF8.GetBytes(AuthOptions.SecretKey);
            var SessionId = Guid.NewGuid().ToString();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", UserId),
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim("role_in_mangotrees", Role),
                    new Claim(JwtRegisteredClaimNames.Jti, SessionId)
                    }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecretKey_Bytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var SessionData = new SessionsCollection()
            {
                SessionId = SessionId,
                UserId = UserId,
                Token = tokenString,
                CreatedDate = DateTime.UtcNow,
                ExpiresDate = DateTime.UtcNow.AddDays(3),
            };
            await SaveSessionAsync(SessionData);

            return tokenString;
        }

        public async Task<bool> ValidateUserOTPAsync(string phoneNumber, string otp)
        {
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(otp))
            {
                return false;
            }

            var otpData = await context.LoadAsync<OTPCollection>(phoneNumber);

            if (otpData == null || otpData.ExpiresDate <= DateTime.Now)
            {
                return false;
            }

            if(otp == otpData.OTP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteUserOTPAsync(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            await context.DeleteAsync<OTPCollection>(phoneNumber);
            return true;
        }

        public async Task SaveSessionAsync(SessionsCollection SessionData)
        {
            var retryCount = 3;

            Start:
            try
            {
                await context.SaveAsync(SessionData);
            }
            catch (Exception)
            {
                if(retryCount == 1)
                {
                    throw new Exception("Communication error: Failed to save session after multiple attempts.");
                }
                else
                {
                    retryCount--;
                    await Task.Delay(1000);
                    goto Start;
                }
            }

            return;
        }
        public async Task<bool> IsSessionValidAsync(string SessionId)
        {
            var SessionData = await context.LoadAsync<SessionsCollection>(SessionId);
            if (SessionData == null || SessionData.ExpiresDate<=DateTime.Now)
            {
                return false;
            }
            return true;
        }

        public async Task<RolesModel> AddRoleDataAsync(string RoleName)
        {
            var roleData = new RolesCollection() { RoleName = RoleName };
            var role = mapper.Map<RolesModel>(roleData);
            await context.SaveAsync(roleData);
            
            return role;
        }
        public async Task<RolesModel> GetRoleByIdAsync(string RoleId)
        {
            var rolesCollectionData = await context.LoadAsync<RolesCollection>(RoleId);
            var data = mapper.Map<RolesModel>(rolesCollectionData);
            return data;
        }

        public async Task AddUserAuthenticationDataAsync(UserAuthenticationModel userData)
        {
            var user = mapper.Map<UserAuthenticationCollection>(userData);
            await context.SaveAsync(user);
        }
        public async Task<UserAuthenticationModel> GetUserLoginAsync(LoginRequestModel model)
        {
            UserAuthenticationModel? user;
            if (generalService.IsPhoneNumber(model.UserName))
            {
                user = await GetUserAuthenticationDataByPhoneNumberAsync(model.UserName);
            }
            else
            {
                user = await GetUserAuthenticationDataAsync(model.UserName);
            }

            return user;
        }
        public async Task<UserAuthenticationModel> GetUserAuthenticationDataAsync(string UserName)
        {
            var userData = await context.LoadAsync<UserAuthenticationCollection>(UserName);
            var user = mapper.Map<UserAuthenticationModel>(userData);
            return user;
        }
        public async Task<UserAuthenticationModel> GetUserAuthenticationDataByPhoneNumberAsync(string phoneNumber)
        {
            var scanConditions = new List<ScanCondition>
            {
                new ScanCondition("PhoneNumber", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, phoneNumber)
            };
            var searchResults = await context.ScanAsync<UserAuthenticationCollection>(scanConditions).GetRemainingAsync();
            var userData = searchResults.FirstOrDefault();

            var user = mapper.Map<UserAuthenticationModel>(userData);
            return user;
        }
        public async Task UpdateLastLogin(UserAuthenticationModel userData)
        {
            userData.LastLogin = DateTime.UtcNow;
            var user = mapper.Map<UserAuthenticationCollection>(userData);
            await context.SaveAsync(user);
        }
    }
}