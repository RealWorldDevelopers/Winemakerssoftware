using WMS.Business.Shared;

namespace WMS.Business.Yeast.Dto
{
    /// <summary>
    /// Instance of <see cref="Dto"/> Factory
    /// </summary>
    /// <inheritdoc cref="IFactory"/>>
    public class Factory : IFactory
    {
        /// <inheritdoc cref="IFactory.CreateNewCode"/>>
        public Code CreateNewCode(int id, int parentId, string literal)
        {
            var dto = new Code
            {
                Id = id,
                Literal = literal,
                ParentId = parentId
            };
            return dto;
        }

        /// <inheritdoc cref="IFactory.CreateNewYeast"/>>
        public Yeast CreateNewYeast(int id, int totalVotes, Code brand, Code style, string trademark, int? tempMin, int? tempMax, double? alcohol, string note)
        {
            var dto = new Yeast
            {
                Id = id,
                Brand = brand,
                Style = style,
                Trademark = trademark,
                TempMin = tempMin,
                TempMax = tempMax,
                Alcohol = alcohol,
                Note = note
            };
            return dto;
        }



    }
}
