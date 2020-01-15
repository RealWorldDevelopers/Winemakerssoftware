using WMS.Business.Common;

namespace WMS.Business.Yeast.Dto
{
    /// <summary>
    /// Data Transfer Object representing a Code Literal Type Table with an added optional foreign key property
    /// </summary>
    public class YeastDto
    {
        public int Id { get; set; }
        public ICode Brand { get; set; }
        public ICode Style { get; set; }
        public string Trademark { get; set; }
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public string Note { get; set; }
    }
}
