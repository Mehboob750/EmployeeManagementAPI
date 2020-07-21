﻿//-----------------------------------------------------------------------
// <copyright file="UserBuiseness.cs" company="BridgeLabz Solution">
//  Copyright (c) BridgeLabz Solution. All rights reserved.
// </copyright>
// <author>Mehboob Shaikh</author>
//-----------------------------------------------------------------------
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Reviewed.")]

namespace EmployeeBuisenessLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EmployeeBuisenessLayer.Interface;
    using EmployeeCommonLayer;
    using EmployeeCommonLayer.Model;
    using EmployeeCommonLayer.RequestModel;
    using EmployeeRepositoryLayer.Interface;

    /// <summary>
    /// This Class is used to implement the methods of interface
    /// </summary>
    public class UserBuiseness : IUserBuiseness
    {
        /// <summary>
        /// Created the Reference of IUserRepository
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBuiseness"/> class.
        /// </summary>
        /// <param name="userRepository">It contains the object IUserRepository</param>
        public UserBuiseness(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// This Method is used to User Registration
        /// </summary>
        /// <param name="userModel">It contains the Object of User Model</param>
        /// <returns>If User Registered Successfully it returns true</returns>
        public async Task<bool> UserRegistration(RegistrationModel registrationModel)
        {
            try
            {
                if (registrationModel != null)
                {
                    // Call the User Registration Method of User Repository Class
                    var response = await this.userRepository.UserRegistration(registrationModel);

                    // check response if equal returns true
                    if (response == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// This Method is used to User Login
        /// </summary>
        /// <param name="userModel">It contains the Object of User Model</param>
        /// <returns>If User Login Successfully it returns true</returns>
        public IList<LoginModel> UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                // Call the User Login Method of User Repository Class
                var response = this.userRepository.UserLogin(userLoginModel);
                return response;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }
        }
    }
}
