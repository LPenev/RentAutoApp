namespace RentAutoApp.Web.ViewModels;

public sealed class WebErrorViewModel
{
    public int Code { get; set; }
    public string ResourceKeyPrefix { get; set; } = String.Empty;
    public string? Details { get; set; }

    public string HomeUrl { get; set; } = "/";
    public string? ContactUrl { get; set; } = String.Empty;
    public bool ShowBackButton { get; set; } = true;


    public string? OriginalPath { get; set; }
    public string? OriginalQuery { get; set; }
    public string? ExceptionPath { get; set; }
    public string? ExceptionMessage { get; set; }
}
