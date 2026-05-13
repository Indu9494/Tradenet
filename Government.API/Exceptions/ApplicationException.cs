namespace Government.API.Exceptions
{
    /// <summary>
    /// Base custom exception class for all application-specific exceptions
    /// </summary>
    public class AppException : Exception
    {
        public string ErrorCode { get; set; }
        public int? HttpStatusCode { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }

        public AppException(string message, string errorCode = null, int? httpStatusCode = 500)
            : base(message)
        {
            ErrorCode = errorCode ?? "INTERNAL_ERROR";
            HttpStatusCode = httpStatusCode;
            AdditionalData = new Dictionary<string, object>();
        }

        public AppException(string message, Exception innerException, string errorCode = null, int? httpStatusCode = 500)
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? "INTERNAL_ERROR";
            HttpStatusCode = httpStatusCode;
            AdditionalData = new Dictionary<string, object>();
        }
    }
}
