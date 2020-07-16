
using System.Collections.Generic;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Business.Yeast.Dto;

namespace WMS.Business.Recipe.Dto
{
   /// <summary>
   /// Data Transfer Object representing a Recipe Table Entity
   /// </summary>
   public class RecipeDto
   {
      public RecipeDto()
      {
         ImageFiles = new List<ImageFileDto>();
      }

      /// <summary>
      /// Primary Key
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Foreign Key of associated User who submitted recipe
      /// </summary>
      public string SubmittedBy { get; set; }

      /// <summary>
      /// The name given to this recipe
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Associated Category Code Literal <see cref="ICode"/>
      /// </summary>
      public ICode Category { get; set; }

      /// <summary>
      /// Associated Variety Code Literal <see cref="ICode"/>
      /// </summary>
      public ICode Variety { get; set; }

      /// <summary>
      /// Yeast to Use
      /// </summary>
      public YeastDto Yeast { get; set; }

      /// <summary>
      /// Target Values
      /// </summary>
      public TargetDto Target { get; set; }

      /// <summary>
      /// Brief description of recipe
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Ingredients needed
      /// </summary>
      public string Ingredients { get; set; }

      /// <summary>
      /// How to instructions
      /// </summary>
      public string Instructions { get; set; }

      /// <summary>
      /// Website users approval rating
      /// </summary>
      public RatingDto Rating { get; set; }

      /// <summary>
      /// States if recipe able to be view and used
      /// </summary>
      public bool Enabled { get; set; }

      /// <summary>
      /// States if recipe needs to go through the approval process
      /// </summary>
      public bool NeedsApproved { get; set; }

      /// <summary>
      /// Number of time recipe has been viewed online
      /// </summary>
      public int Hits { get; set; }

      /// <summary>
      /// List of Image File Data <see cref="ImageFileDto"/>
      /// </summary>
      public List<ImageFileDto> ImageFiles { get; }

   }
}
