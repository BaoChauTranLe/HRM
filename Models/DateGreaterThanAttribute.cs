﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace HRM.Models
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TimeGreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        public string otherPropertyName;
        public TimeGreaterThanAttribute() { }
        public TimeGreaterThanAttribute(string otherPropertyName, string ErrorMessage)
            : base(ErrorMessage)
        {
            this.otherPropertyName = otherPropertyName;
            this.ErrorMessage = ErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                // Using reflection we can get a reference to the other time property, in this example the project start time
                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(this.otherPropertyName);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue == null)
                {
                    //validationResult = new ValidationResult("Start Time is empty");
                    return validationResult;
                }
                var datatype = extensionValue.GetType();

                //var otherPropertyInfo = validationContext.ObjectInstance.GetType().GetProperty(this.otherPropertyName);
                if (field == null)
                    return new ValidationResult(String.Format("Unknown property: {0}.", otherPropertyName));

                DateTime toValidate = (DateTime)value;
                DateTime referenceProperty = (DateTime)field.GetValue(validationContext.ObjectInstance, null);
                // if the end time is lower than the start time, than the validationResult will be set to false and return
                // a properly formatted error message
                
                //Starttime hour greater than endtime hour
                if (toValidate.Hour < referenceProperty.Hour)
                {
                    validationResult = new ValidationResult(ErrorMessage);
                }
                //Starttime minute greater than endtime minute
                if (toValidate.Hour == referenceProperty.Hour && toValidate.Minute <= referenceProperty.Minute)
                {
                    validationResult = new ValidationResult(ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "isgreater",
            };
            rule.ValidationParameters.Add("otherproperty", otherPropertyName);
            yield return rule;
        }
    }
}