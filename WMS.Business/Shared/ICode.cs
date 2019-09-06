namespace WMS.Business.Shared
{
    /// <summary>
    /// Data Transfer Object representing a Code Literal Type Table with an added optional foreign key property
    /// </summary>
    public interface ICode
    {
        /// <summary>
        /// Code Unique Identifier
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Code Foreign Key
        /// </summary>
        /// <remarks>Optional</remarks>
        int? ParentId { get; set; }

        /// <summary>
        /// Literal Name of Code
        /// </summary>
        string Literal { get; set; }

        /// <summary>
        /// Description of Code
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Enabled for Use
        /// </summary>
        bool Enabled { get; set; }

        
       
    }
}