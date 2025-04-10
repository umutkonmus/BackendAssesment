namespace DirectoryService.Utils.Response
{
    public sealed class Response<T>
    {
        public T Data { get; set; }
        public int Status { get; set; }
        public bool IsSuccessful { get; set; }
        public List<string>? Errors { get; set; }

        public static Response<T> Success(T data, int status)
        {
            return new Response<T> { Data = data, Status = status, IsSuccessful = true };
        }
        public static Response<T> Success(int status)
        {
            return new Response<T> { Data = default(T), Status = status, IsSuccessful = true };
        }
        public static Response<T> Fail(List<string> errors, int status)
        {
            return new Response<T>
            {
                Errors = errors,
                Status = status,
                IsSuccessful = false
            };
        }
        public static Response<T> Fail(string error, int status)
        {
            return new Response<T> { Errors = new List<string> { error }, Status = status, IsSuccessful = false };
        }

    }
}
