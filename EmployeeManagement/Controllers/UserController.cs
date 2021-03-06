﻿//-----------------------------------------------------------------------
// <copyright file="UserController.cs" company="BridgeLabz Solution">
//  Copyright (c) BridgeLabz Solution. All rights reserved.
// </copyright>
// <author>Mehboob Shaikh</author>
//-----------------------------------------------------------------------
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Reviewed.")]

namespace EmployeeManagementApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using EmployeeBuisenessLayer.Interface;
    using EmployeeBuisenessLayer.Services;
    using EmployeeCommonLayer.RequestModel;
    using EmployeeManagement.Sender;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// User Controller class contains the API for registration and login
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        IConfiguration configuration;

        Sender sender = new Sender();
        private IUserBL userBuiseness;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userBusiness">It is an object of IUser Business</param>
        public UserController(IUserBL userBuiseness, IConfiguration configuration)
        {
            this.userBuiseness = userBuiseness;
            this.configuration = configuration;
        }

        /// <summary>
        /// This Method is used for User Registration
        /// </summary>
        /// <param name="registrationModel">It is an object of User Model</param>
        /// <returns>Returns the result in SMD format</returns>
        [HttpPost]
        [Route("Registration")]
        [AllowAnonymous]
        public IActionResult UserRegistration(RegistrationRequestModel registrationModel)
        {
            try
            {
                if (registrationModel.FirstName == null || registrationModel.LastName == null || registrationModel.City == null || registrationModel.EmailId == null || registrationModel.PhoneNumber == null || registrationModel.Gender == null || registrationModel.Password == null)
                {
                    throw new EmployeeManagementException(EmployeeManagementException.ExceptionType.NULL_FIELD_EXCEPTION, "Null Variable Field");
                }
                else if (registrationModel.FirstName == "" || registrationModel.LastName == "" || registrationModel.City == "" || registrationModel.EmailId == "" || registrationModel.PhoneNumber == "" || registrationModel.Gender == "" || registrationModel.Password == "")
                {
                    throw new EmployeeManagementException(EmployeeManagementException.ExceptionType.EMPTY_FIELD_EXCEPTION, "Empty Variable Field");
                }

                // Call the User Registration Method of User Business classs
                var response = this.userBuiseness.UserRegistration(registrationModel);

                // check if response is equal to true
                if (!response.Id.Equals(0))
                {
                    bool status = true;
                    var message = "User Registered Successfully";
                    string msmqData = Convert.ToString(registrationModel.FirstName)+" "+ Convert.ToString(registrationModel.LastName) + "\n" + message + "\n Email : " + Convert.ToString(registrationModel.EmailId) + "\n Password : " + Convert.ToString(registrationModel.Password);
                    sender.Message(msmqData);
                    return this.Ok(new { status, message, data = response });
                }
                else
                {
                    bool status = false;
                    var message = "User already Present";
                    return this.Conflict(new { status, message });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = false, message = e.Message});
            }
        }

        /// <summary>
        /// This Method is used for User Login
        /// </summary>
        /// <param name="userLoginModel">It is an object of User Model</param>
        /// <returns>Returns the result in SMD format</returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult UserLogin(LoginRequestModel userLoginModel)
        {
            try
            {
                // Call the User Login Method of User Business classs
                var response =  this.userBuiseness.UserLogin(userLoginModel);
                // check if response count is equal to 1
                if (!response.Id.Equals(0))
                {
                    string token = this.CreateToken(userLoginModel, "authenticate user");
                    response.Token = token;
                    bool status = true;
                    var message = "Login Successfully";
                    return this.Ok(new { status, message, data = response });
                }
                else
                {
                    bool status = false;
                    var message = "Fail To Login";
                    return this.NotFound(new { status, message});
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { status = false, message = e.Message});
            }
            
        }

        /// <summary>
        /// This Method is used for Forget Password
        /// </summary>
        /// <param name="forgotPassword">It is an object of Forgot Password Model</param>
        /// <returns>Returns the result in SMD format</returns>
        [HttpPost]
        [Route("ForgetPassword")]
        [AllowAnonymous]
        public IActionResult ForgetPassword(ForgotPasswordModel forgotPassword)
        {
            try
            {
                // Call the Forget Password Method of User Business classs
                var data = this.userBuiseness.ForgetPassword(forgotPassword);
                if (data != null)
                {
                    return this.Ok(new { status = "True", message = "Reset Password Link Has Been Sent To Your Email" });
                }
                else
                {
                    return this.NotFound(new { status = "False", message = "Email Is Not Correct" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// This Method is used for Reset Password
        /// </summary>
        /// <param name="resetModel">It is an object of Reset Password Model</param>
        /// <returns>Returns the result in SMD format</returns>
        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]

        public IActionResult ResetPassword(ResetPasswordModel resetModel)
        {
            try
            {
                // Call the Reset Password Method of User Business classs
                var data = this.userBuiseness.ResetPassword(resetModel);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Password Reset Successfully" });
                }
                else
                {
                    return this.BadRequest(new { status = "False", message = "Failed To Reset Password" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// This Method is used to generate JWT Token
        /// </summary>
        /// <param name="userLoginModel">It is an object of Login Request Model</param>
        /// <param name="type">It Contains token type</param>
        /// <returns>Returns JWT Token</returns>
        private string CreateToken(LoginRequestModel userLoginModel, string type)
        {
            try
            {
                var symmetricSecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var signingCreds = new SigningCredentials(symmetricSecuritykey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>();
                claims.Add(new Claim("EmailId", userLoginModel.EmailId.ToString()));
                claims.Add(new Claim("TokenType", type));
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signingCreds);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
