
using Newtonsoft.Json;
using System.Collections.Generic;


namespace WMS.Domain
{
    /// <summary>
    /// Data Transfer Object representing a Batch Table Entity
    /// </summary>
    public class Batch
    {
        public Batch()
        {
            Entries = new List<BatchEntry>();
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The name given to this Batch
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Brief description of Batch
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Size of Batch
        /// </summary>
        public double? Volume { get; set; }

        /// <summary>
        /// Unit of Measure for Volume
        /// </summary>
        [JsonConverter(typeof(ConcreteConverter<UnitOfMeasure>))] 
        public IUnitOfMeasure? VolumeUom { get; set; }

        /// <summary>
        /// Foreign Key of Associated User who Submitted Batch
        /// </summary>
        public string? SubmittedBy { get; set; }

        /// <summary>
        /// Year of Batch
        /// </summary>
        public int? Vintage { get; set; }

        /// <summary>
        /// Foreign Key of Variety for Batch
        /// </summary>
        [JsonConverter(typeof(ConcreteConverter<Code>))]
        public ICode? Variety { get; set; }

        /// <summary>
        /// Foreign Key of a Set of Targets for Batch
        /// </summary>
        public Target? Target { get; set; }

        /// <summary>
        /// Foreign Key of Related Recipe for Batch
        /// </summary>
        public int? RecipeId { get; set; }

        /// <summary>
        /// Foreign Key of Related Yeast for Batch
        /// </summary>
        public Yeast? Yeast { get; set; }

        /// <summary>
        /// Foreign Key of Related MLF Culture for Batch
        /// </summary>
        public int? MaloCultureId { get; set; }

        /// <summary>
        /// Is Batch Completed
        /// </summary>
        public bool? Complete { get; set; }

        public List<BatchEntry> Entries { get; }

    }
}
