namespace KartApplication.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }  // Forespørsels-ID (RequestId)

        // Viser forespørsels-ID hvis den ikke er tom
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
