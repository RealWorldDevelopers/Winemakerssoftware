namespace WMS.Ui.Middleware.CspHeader
{
    public sealed class CspDirectives : ICspDirectives
    {
        public IDirective Default_Src { get; set; } = new Directive { Header = "default-src" };
        public IDirective Script_Src { get; set; } = new Directive { Header = "script-src" };
        public IDirective Style_Src { get; set; } = new Directive { Header = "style-src" };
        public IDirective Img_Src { get; set; } = new Directive { Header = "img-src" };
        public IDirective Font_Src { get; set; } = new Directive { Header = "font-src" };
        public IDirective Media_Src { get; set; } = new Directive { Header = "media-src" };
        public IDirective Object_Src { get; set; } = new Directive { Header = "object-src" };
        public IDirective Connect_Src { get; set; } = new Directive { Header = "connect-src" };
        public IDirective Frame_Ancestors { get; set; } = new Directive { Header = "frame-ancestors" };
        public string ReportUri { get; set; }
    }

}
