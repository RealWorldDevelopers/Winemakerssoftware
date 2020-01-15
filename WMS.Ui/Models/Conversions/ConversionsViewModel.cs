
using System.ComponentModel;

namespace WMS.Ui.Models.Conversions
{
    public class ConversionsViewModel
    {
        [DisplayName("milligrams")]
        public decimal Milligrams { get; set; }

        [DisplayName("grams")]
        public decimal Grams { get; set; }

        [DisplayName("kilograms")]
        public decimal Kilograms { get; set; }

        [DisplayName("ounces")]
        public decimal Ounces { get; set; }

        [DisplayName("pounds")]
        public decimal Pounds { get; set; }

        [DisplayName("milliliters")]
        public decimal Milliliters { get; set; }

        [DisplayName("liters")]
        public decimal Liters { get; set; }

        [DisplayName("gallons")]
        public decimal Gallons { get; set; }

        [DisplayName("fluid ounces")]
        public decimal FluidOunces { get; set; }

        [DisplayName("cups")]
        public decimal Cups { get; set; }

        [DisplayName("pints")]
        public decimal Pints { get; set; }

        [DisplayName("quarts")]
        public decimal Quarts { get; set; }

        [DisplayName("tablespoons")]
        public decimal Tablespoons { get; set; }

        [DisplayName("teaspoons")]
        public decimal Teaspoons { get; set; }

        [DisplayName("Fahrenheit")]
        public decimal Fahrenheit { get; set; }

        [DisplayName("Celsius")]
        public decimal Celsius { get; set; }

    }
}
