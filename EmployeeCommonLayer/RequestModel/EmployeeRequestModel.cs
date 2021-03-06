﻿//-----------------------------------------------------------------------
// <copyright file="EmployeeRequestModel.cs" company="BridgeLabz Solution">
//  Copyright (c) BridgeLabz Solution. All rights reserved.
// </copyright>
// <author>Mehboob Shaikh</author>
//-----------------------------------------------------------------------
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Reviewed.")]

namespace EmployeeCommonLayer.RequestModel
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// This is Model Class For Employee
    /// </summary>
    public class EmployeeRequestModel
    {
        /// <summary>
        /// Gets or sets the First name
        /// </summary>
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "First Name is not valid")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last name
        /// </summary>
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Last Name is not valid")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Gender
        /// </summary>
        [Required]
        [RegularExpression("(?:m|M|male|Male|f|F|female|Female)$", ErrorMessage = "Gender is not valid")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the EmailId
        /// </summary>
        [Required]
        [RegularExpression("^[0-9a-zA-Z]+([._+-][0-9a-zA-Z]+)*@[0-9a-zA-Z]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$", ErrorMessage = "EmailId is not valid")]
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the Phone Number
        /// </summary>
        [RegularExpression("([1-9]{1}[0-9]{9})$", ErrorMessage = "Phone number is not valid")]
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the City
        /// </summary>
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "City is not valid")]
        public string City { get; set; }
    }
}
