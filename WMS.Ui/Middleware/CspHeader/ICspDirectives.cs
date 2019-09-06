namespace WMS.Ui.Middleware.CspHeader
{
    public interface ICspDirectives
    {
        IDirective Connect_Src { get; set; }
        IDirective Default_Src { get; set; }
        IDirective Font_Src { get; set; }
        IDirective Frame_Ancestors { get; set; }
        IDirective Img_Src { get; set; }
        IDirective Media_Src { get; set; }
        IDirective Object_Src { get; set; }
        IDirective Script_Src { get; set; }
        IDirective Style_Src { get; set; }
        string ReportUri { get; set; }
    }
}