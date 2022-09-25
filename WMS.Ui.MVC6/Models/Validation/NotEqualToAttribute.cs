using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WMS.Ui.Mvc6.Models.Validation
{
   /// <summary>
   /// NotEqualTo validator that simply checks that the value of one property does not equal the value of another.  
   /// <para> [NotEqualTo("otherPropertyName,otherPropertyName")]</para>
   /// </summary>
   /// <remarks>NotEqualToAttribute attribute is only permitted on properties and only a single instance of the attribute may appear on each property.</remarks>
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
   public sealed class NotEqualToAttribute : ValidationAttribute, IClientModelValidator
   {
      /// <summary>
      /// Default Error Message is passed into the constructor of the base class.
      /// </summary>
      private const string DefaultErrorMessage = "{0} cannot be the same as {1}.";

      /// <param name="dependentProperty">Property to Compare</param>
      public NotEqualToAttribute(string dependentProperty)
         : base(DefaultErrorMessage)
      {
         if (string.IsNullOrEmpty(dependentProperty))
            throw new ArgumentNullException(nameof(dependentProperty));

         DependentProperty = dependentProperty.Replace(" ", "", StringComparison.OrdinalIgnoreCase);
      }

      public string DependentProperty { get; private set; }

      /// <summary>
      /// Override FormatErrorMessage allowing us to add the two dynamic property names into the error message string.
      /// </summary>
      public override string FormatErrorMessage(string name)
      {
         var others = DependentProperty.Replace(",", " or ", StringComparison.OrdinalIgnoreCase);
         return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, others);
      }

      /// <summary>
      /// Validation Logic                 
      /// </summary>
      protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
      {
         if (validationContext == null)
            throw new ArgumentNullException(nameof(validationContext));

         if (value != null)
         {
            var otherPropNames = DependentProperty.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var otherPropName in otherPropNames)
            {
               var propInfo = validationContext.ObjectInstance.GetType().GetProperty(otherPropName);
               var propValue = propInfo?.GetValue(validationContext.ObjectInstance, null);
               if (value.Equals(propValue))
                  return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
         }
         return ValidationResult.Success!;
      }

      /// <summary>
      /// Method for Client Side Validation
      /// </summary>
      public void AddValidation(ClientModelValidationContext context)
      {
         if (context == null)
            throw new ArgumentNullException(nameof(context));

         MergeAttribute(context.Attributes, "data-val", "true");
         MergeAttribute(context.Attributes, "data-val-notequalto", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
         MergeAttribute(context.Attributes, "data-val-notequalto-dependentproperty", DependentProperty);
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
