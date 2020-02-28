using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WMS.Ui.Models.Validation
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public sealed class RangeIfAttribute : RangeAttribute, IClientModelValidator
   {
      public string DependentProperty { get; private set; }
      public Comparison Comparison { get; private set; }
      public object Value { get; private set; }

      /// <param name="minimum"></param>
      /// <param name="maximum"></param>
      /// <param name="dependentProperty">Property to Compare</param>
      /// <param name="comparison"></param>
      /// <param name="value"></param>
      public RangeIfAttribute(double minimum, double maximum, string dependentProperty, Comparison comparison, object value)
          : base(minimum, maximum)
      {
         DependentProperty = dependentProperty;
         Comparison = comparison;
         Value = value;
      }

      public RangeIfAttribute(int minimum, int maximum, string dependentProperty, Comparison comparison, object value)
          : base(minimum, maximum)
      {
         DependentProperty = dependentProperty;
         Comparison = comparison;
         Value = value;
      }

      private bool IsdependantMatched(object actualPropertyValue)
      {
         switch (Comparison)
         {
            case Comparison.IsNotEqualTo:
               return actualPropertyValue == null || !actualPropertyValue.Equals(Value);
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

         var propInfo = validationContext.ObjectInstance.GetType().GetProperty(DependentProperty);
         var propValue = propInfo.GetValue(validationContext.ObjectInstance, null);
         var dependantMatched = IsdependantMatched(propValue);

         if (dependantMatched)
         {
            return base.IsValid(value, validationContext);
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
         MergeAttribute(context.Attributes, "data-val-rangeif", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
         MergeAttribute(context.Attributes, "data-val-rangeif-dependentproperty", DependentProperty);
         MergeAttribute(context.Attributes, "data-val-rangeif-comparison", Comparison.ToString().ToLower(CultureInfo.CurrentCulture));
         MergeAttribute(context.Attributes, "data-val-rangeif-dependentvalue", Value.ToString());
         MergeAttribute(context.Attributes, "data-val-rangeif-min", Minimum.ToString());
         MergeAttribute(context.Attributes, "data-val-rangeif-max", Maximum.ToString());
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
