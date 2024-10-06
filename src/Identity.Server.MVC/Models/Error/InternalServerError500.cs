namespace Identity.Server.MVC.Models.Error;

public class InternalServerError500
{
    public string TraceId { get; set; } = string.Empty;
    public bool ShowTraceId => !string.IsNullOrEmpty(TraceId);
    public string? Message { get; set; }
}