using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class Scores
    {
        public int Id { get; set; }
        public int? BatchId { get; set; }
        public double? AppearanceScore { get; set; }
        public string AppearanceNote { get; set; }
        public double? AromaScore { get; set; }
        public string AromaNote { get; set; }
        public double? TasteScore { get; set; }
        public string TasteNote { get; set; }
        public string Dryness { get; set; }
        public string Sweetness { get; set; }
        public double? AfterTasteScore { get; set; }
        public string AfterTasteNote { get; set; }
        public double? OverallScore { get; set; }
        public string Comments { get; set; }
        public string Contest { get; set; }
        public string Medal { get; set; }
        public DateTime? DateScored { get; set; }

        public virtual Batches Batch { get; set; }
    }
}
