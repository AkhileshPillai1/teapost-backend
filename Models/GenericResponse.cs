namespace TeaPost.Models
{
    public class GenericResponse
    {
        public bool isSuccess { get; set; } = true;
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
        public string detailedMessage { get; set; } = string.Empty;
    }
}
