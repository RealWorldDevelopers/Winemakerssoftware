namespace WMS.Business.Common
{  
    /// <inheritdoc cref="ICode"/>
    public class Code : ICode
    {
        /// <inheritdoc cref="ICode.Id"/>
        public int Id { get; set; }

        /// <inheritdoc cref="ICode.ParentId"/>
        public int? ParentId { get; set; }

        /// <inheritdoc cref="ICode.Literal"/>
        public string Literal { get; set; }

        /// <inheritdoc cref="ICode.Enabled"/>
        public bool Enabled { get; set; }

        /// <inheritdoc cref="ICode.Description"/>
        public string Description { get; set; }
    }
}
