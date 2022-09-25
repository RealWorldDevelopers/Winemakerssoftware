
namespace WMS.Domain
{
    public class Category : ICode
    {
        /// <summary>
        /// Code Unique Identifier
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Code Foreign Key
        /// </summary>
        /// <remarks>Optional</remarks>
        public int? ParentId { get; set; }

        /// <summary>
        /// Literal Name of Code
        /// </summary>
        public string? Literal { get; set; }

        /// <summary>
        /// Description of Code
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Enabled for Use
        /// </summary>
        public bool? Enabled { get; set; }
    }

}
