namespace FileServer.Server.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? FailureReason { get; set; }

        public static ServiceResponse<T> Accept(T? data)
        {
            return new ServiceResponse<T> { Success = true, Data = data };
        }

        public static ServiceResponse<T> Reject(string reason)
        {
            return new ServiceResponse<T> { Success = false, FailureReason = reason };
        }
    }
}
