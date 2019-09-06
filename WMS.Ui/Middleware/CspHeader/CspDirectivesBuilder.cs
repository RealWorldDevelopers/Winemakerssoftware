namespace WMS.Ui.Middleware.CspHeader
{
    public sealed class CspDirectivesBuilder
    {
        private readonly ICspDirectives _directives = new CspDirectives();

        internal CspDirectivesBuilder() { }

        public CspSourceBuilder Default_Src { get; set; } = new CspSourceBuilder();
        public CspSourceBuilder Scripts_Src { get; set; } = new CspSourceBuilder();
        public CspSourceBuilder Styles_Src { get; set; } = new CspSourceBuilder();
        public CspSourceBuilder Imgs_Src { get; set; } = new CspSourceBuilder();
        public CspSourceBuilder Fonts_Src { get; set; } = new CspSourceBuilder();
        public CspSourceBuilder Medias_Src { get; set; } = new CspSourceBuilder();
        public CspSourceBuilder Object_Src { get; set; } = new CspSourceBuilder();
        public CspSourceBuilder Connect_Src { get; set; } = new CspSourceBuilder();
        public CspSourceBuilder Frame_Ancestors { get; set; } = new CspSourceBuilder();
        public string ReportUri { get; set; }
        internal ICspDirectives Build()
        {
            _directives.Default_Src.Sources = Default_Src.Sources;
            _directives.Script_Src.Sources = Scripts_Src.Sources;
            _directives.Style_Src.Sources = Styles_Src.Sources;
            _directives.Img_Src.Sources = Imgs_Src.Sources;
            _directives.Font_Src.Sources = Fonts_Src.Sources;
            _directives.Media_Src.Sources = Medias_Src.Sources;
            _directives.Connect_Src.Sources = Connect_Src.Sources;
            _directives.Object_Src.Sources = Object_Src.Sources;
            _directives.Frame_Ancestors.Sources = Frame_Ancestors.Sources;
            _directives.ReportUri = ReportUri;           

            return _directives;
        }
    }

}
