namespace WMS.Business.Common
{
   /// <inheritdoc cref="ICodeDto"/>
   public class CodeDto : ICodeDto
   {
      /// <inheritdoc cref="ICodeDto.Id"/>
      public int? Id { get; set; }

      /// <inheritdoc cref="ICodeDto.ParentId"/>
      public int? ParentId { get; set; }

      /// <inheritdoc cref="ICodeDto.Literal"/>
      public string? Literal { get; set; }

      /// <inheritdoc cref="ICodeDto.Enabled"/>
      public bool? Enabled { get; set; }

      /// <inheritdoc cref="ICodeDto.Description"/>
      public string? Description { get; set; }
   }
}
