﻿using MangoTreesAPI.Models;
using MangoTreesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using static MangoTreesAPI.Models.ResponseMessages;
using static System.Net.WebRequestMethods;

namespace MangoTreesAPI.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController: ControllerBase
    {
        private readonly AuthService authService;
        private readonly CustomerService customerService;
        private readonly OtpService otpService;
        private readonly IHttpContextAccessor httpContext;
        public AuthenticationController(CustomerService _customerService, AuthService _authService, OtpService _otpService, IConfiguration configuration, IHttpContextAccessor _httpContext)
        {
            otpService = _otpService;
            authService = _authService;
            customerService = _customerService;
            httpContext = _httpContext;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginRequestModel model)
        {

            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password) || !ModelState.IsValid) {
                return BadRequest(new { Message = LoginResponse.FailureProperty1 });
            }

            try
            {
                var user = await authService.GetUserLoginAsync(model);

                if (user == null || user.Password != model.Password)
                {
                    return Unauthorized(new { Message = LoginResponse.FailureProperty2 });
                }
                var role = await authService.GetRoleByIdAsync(user.Role);
                if (user.IsActive)
                {
                    var tokenString = await authService.GenerateTokenAsync(user.UserName, user.UserId!, role.RoleName!);
                    await authService.UpdateLastLogin(user);
                    return Ok(new { Token = tokenString });
                }
                else
                {
                    return Unauthorized(new { Message = LoginResponse.FailureProperty3 });
                }
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }

        }

        [HttpPost("Request/OTP")]
        public async Task<ActionResult<string>> RequestRegisterUser(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 10 || !ModelState.IsValid)
            {
                return BadRequest(new { Message = ResponseMessages.Response.Failure.ToString() });
            }
            try
            {
                var userData = await authService.GetUserAuthenticationDataByPhoneNumberAsync(phoneNumber);
                if (userData != null)
                {
                    return BadRequest(new { Message = SignupResponse.FailureProperty1 });
                }
                else
                {
                    await otpService.GenerateSignupOTP(phoneNumber);
                    return Ok(new { Message = SignupResponse.SuccessProperty2 });
                }
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("Signup")]
        public async Task<ActionResult<string>> RegisterUser([FromBody] UserRequestModel userData, string otp)
        {
            if (userData == null || otp == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var user = await customerService.AddUserDataAsync(userData, otp);
                await authService.DeleteUserOTPAsync(userData.PhoneNumber);
                return Ok(new { Message = SignupResponse.SuccessProperty1 });
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }

        [HttpPost("Password/Request/OTP")]
        public async Task<ActionResult<string>> RequestUpdateCustomerPassword(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 10 || !ModelState.IsValid)
            {
                return BadRequest(new { Message = ResponseMessages.Response.Failure.ToString() });
            }
            try
            {
                var userData = await authService.GetUserAuthenticationDataByPhoneNumberAsync(phoneNumber);
                if (userData != null)
                {
                    await otpService.GenerateSignupOTP(phoneNumber);
                    return Ok(new { Message = SignupResponse.SuccessProperty2 });
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

        [HttpPost("Password")]
        public async Task<ActionResult> UpdateCustomerPassword([FromBody] ResetPasswordModel resetPasswordData)
        {
            if (resetPasswordData == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await customerService.UpdateCustomerPasswordAsync(resetPasswordData);
                await authService.DeleteUserOTPAsync(resetPasswordData.PhoneNumber);
                return Ok(new { Message = UpdateCustomerDetails.SuccessProperty1 });
            }
            catch (Exception)
            {
                return NotFound(new { Message = ResponseMessages.Response.ResourceNotFound });
            }
        }
    }
}
