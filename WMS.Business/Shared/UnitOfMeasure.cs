

namespace WMS.Business.Common
{

   /// <inheritdoc cref="IUnitOfMeasure"/>
   public class UnitOfMeasure : IUnitOfMeasure
   {
      /// <inheritdoc cref="IUnitOfMeasure.Id"/>
      public int Id { get; set; }

      /// <inheritdoc cref="IUnitOfMeasure.Abbreviation"/>
      public string Abbreviation { get; set; }

      /// <inheritdoc cref="IUnitOfMeasure.UnitOfMeasure"/>
      string IUnitOfMeasure.UnitOfMeasure { get; set; }

      /// <inheritdoc cref="IUnitOfMeasure.Description"/>
      public string Description { get; set; }

      /// <inheritdoc cref="IUnitOfMeasure.Enabled"/>
      public bool Enabled { get; set; }

      
   }
}
