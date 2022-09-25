
using Newtonsoft.Json;

namespace WMS.Domain
{
    /// <summary>
    /// Data Transfer Object representing a Target Table Entity
    /// </summary>
    public class Target
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Target Fermentation Temperature
        /// </summary>
        public double? Temp { get; set; }

        /// <summary>
        /// Unit of Measure for Temp
        /// </summary>
        [JsonConverter(typeof(ConcreteConverter<UnitOfMeasure>))]
        public IUnitOfMeasure? TempUom { get; set; }

        /// <summary>
        /// Target pH
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Spelling Exception")]
        public double? pH { get; set; }

        /// <summary>
        /// Target Total Acidity
        /// </summary>
        public double? TA { get; set; }

        /// <summary>
        /// Target Starting Sugar
        /// </summary>
        public double? StartSugar { get; set; }

        /// <summary>
        /// Unit of Measure for Starting Sugar
        /// </summary>
        [JsonConverter(typeof(ConcreteConverter<UnitOfMeasure>))]
        public IUnitOfMeasure? StartSugarUom { get; set; }

        /// <summary>
        /// Target Ending Sugar
        /// </summary>
        public double? EndSugar { get; set; }

        /// <summary>
        /// Unit of Measure for Ending Sugar
        /// </summary>
        [JsonConverter(typeof(ConcreteConverter<UnitOfMeasure>))]
        public IUnitOfMeasure? EndSugarUom { get; set; }

    }
}
