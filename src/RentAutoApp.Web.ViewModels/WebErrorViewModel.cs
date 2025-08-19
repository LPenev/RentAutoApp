namespace RentAutoApp.Web.ViewModels;

public sealed class WebErrorViewModel
{
    public int Code { get; set; }
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public string? Details { get; set; }

    public string HomeUrl { get; set; } = "/";
    public string? ContactUrl { get; set; } = "/Contact";
    public bool ShowBackButton { get; set; } = true;


    public string? OriginalPath { get; set; }
    public string? OriginalQuery { get; set; }
    public string? ExceptionPath { get; set; }
    public string? ExceptionMessage { get; set; }
}
