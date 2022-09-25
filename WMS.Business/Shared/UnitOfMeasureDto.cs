

namespace WMS.Business.Common
{

    /// <inheritdoc cref="IUnitOfMeasureDto"/>
    public class UnitOfMeasureDto : IUnitOfMeasureDto
    {
        /// <inheritdoc cref="IUnitOfMeasureDto.Id"/>
        public int? Id { get; set; }

        /// <inheritdoc cref="IUnitOfMeasureDto.Abbreviation"/>
        public string? Abbreviation { get; set; }

        /// <inheritdoc cref="IUnitOfMeasureDto.Name"/>
        public string? Name { get; set; }

        /// <inheritdoc cref="IUnitOfMeasureDto.Description"/>
        public string? Description { get; set; }

        /// <inheritdoc cref="IUnitOfMeasureDto.Enabled"/>
        public bool? Enabled { get; set; }


    }
}
