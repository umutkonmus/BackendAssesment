namespace ReportService.Utils
{
    public sealed class ApiResponse<T>
    {
        public T Data { get; set; }
        public int Status { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
