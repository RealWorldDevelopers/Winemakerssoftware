

namespace WMS.Domain
{

   /// <inheritdoc cref="IUnitOfMeasure"/>
   public class UnitOfMeasure : IUnitOfMeasure
   {
      /// <inheritdoc cref="Id"/>
      public int? Id { get; set; }

      /// <inheritdoc cref="Abbreviation"/>
      public string? Abbreviation { get; set; }

      /// <inheritdoc cref="Name"/>
      public string? Name { get; set; }

      /// <inheritdoc cref="Description"/>
      public string? Description { get; set; }

      /// <inheritdoc cref="Enabled"/>
      public bool? Enabled { get; set; }

      
   }
}
