using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WMS.Ui.Models.Validation
{
   /// <summary>
   /// Conditional on another property's value not being Null.   
   /// <para> [RequiredIf("otherPropertyName")]</para>
   /// </summary>
   /// <remarks>Attribute is only permitted on properties and only a single instance of the attribute may appear on each property.</remarks>
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
   public sealed class RequiredIfAttribute : ValidationAttribute, IClientModelValidator
   {
      /// <summary>
      /// Default Error Message is passed into the constructor of the base class.
      /// </summary>
      private const string DefaultErrorMessageFormatString = "{0} field is required.";

    
      public string DependentProperty { get; private set; }
      public Comparison Comparison { get; private set; }
      public object Value { get; private set; }

      
      /// <param name="dependentProperty">Property to Compare</param>
      /// <param name="comparison">Equal To or Differs From</param>
      /// <param name="value">Case Sensitive Value</param>
      public RequiredIfAttribute(string dependentProperty, Comparison comparison, object value)
      {
         if (string.IsNullOrEmpty(dependentProperty))
            throw new ArgumentNullException(nameof(dependentProperty));

         DependentProperty = dependentProperty;
         Comparison = comparison;
         Value = value;

         ErrorMessage = DefaultErrorMessageFormatString;
      }
              
      private bool IsRequired(object actualPropertyValue)
      {
         switch (Comparison)
         {
            case Comparison.IsNotEqualTo:
               return actualPropertyValue != null && !actualPropertyValue.Equals(Value);
            default:
               return actualPropertyValue != null && actualPropertyValue.Equals(Value);
         }
      }

      /// <summary>
      /// Validation Logic                 
      /// </summary>
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
         if (validationContext == null)
            throw new ArgumentNullException(nameof(validationContext));

         if (value == null)
         {
            var propInfo = validationContext.ObjectInstance.GetType().GetProperty(DependentProperty);
            var propValue = propInfo.GetValue(validationContext.ObjectInstance, null);
            if (IsRequired(propValue))
               return new ValidationResult(string.Format(CultureInfo.CurrentCulture, ErrorMessageString, validationContext.DisplayName));
         }
         return ValidationResult.Success;
      }

      /// <summary>
      /// Method for Client Side Validation
      /// </summary>
      public void AddValidation(ClientModelValidationContext context)
      {
         if (context == null)
            throw new ArgumentNullException(nameof(context));

         MergeAttribute(context.Attributes, "data-val", "true");
         MergeAttribute(context.Attributes, "data-val-requiredif", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
         MergeAttribute(context.Attributes, "data-val-requiredif-dependentproperty", DependentProperty);
         MergeAttribute(context.Attributes, "data-val-requiredif-comparison", Comparison.ToString().ToLower(CultureInfo.CurrentCulture));
         MergeAttribute(context.Attributes, "data-val-requiredif-dependentvalue", Value.ToString());
      }

      private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
      {
         if (attributes.ContainsKey(key))
            return false;

         attributes.Add(key, value);
         return true;
      }

   }
}
