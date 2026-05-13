namespace Government.API.Exceptions
{
    /// <summary>
    /// Exception thrown when external service call fails
    /// </summary>
    public class ExternalServiceException : AppException
    {
        public string ServiceName { get; set; }
        public string ServiceUrl { get; set; }

        public ExternalServiceException(string message, string serviceName = null, string serviceUrl = null)
            : base(message, "EXTERNAL_SERVICE_ERROR", 502)
        {
            ServiceName = serviceName;
            ServiceUrl = serviceUrl;
        }

        public ExternalServiceException(string message, Exception innerException, string serviceName = null, string serviceUrl = null)
            : base(message, innerException, "EXTERNAL_SERVICE_ERROR", 502)
        {
            ServiceName = serviceName;
            ServiceUrl = serviceUrl;
        }
    }

    /// <summary>
    /// Exception thrown when external service times out
    /// </summary>
    public class ServiceTimeoutException : ExternalServiceException
    {
        public int TimeoutMilliseconds { get; set; }

        public ServiceTimeoutException(string message, string serviceName = null, int timeoutMs = 0)
            : base(message, serviceName)
        {
            ErrorCode = "SERVICE_TIMEOUT";
            HttpStatusCode = 504;
            TimeoutMilliseconds = timeoutMs;
        }
    }

    /// <summary>
    /// Exception thrown when external service is unavailable
    /// </summary>
    public class ServiceUnavailableException : ExternalServiceException
    {
        public ServiceUnavailableException(string message, string serviceName = null)
            : base(message, serviceName)
        {
            ErrorCode = "SERVICE_UNAVAILABLE";
            HttpStatusCode = 503;
        }
    }

    /// <summary>
    /// Exception thrown when file operation fails
    /// </summary>
    public class FileOperationException : AppException
    {
        public string FilePath { get; set; }
        public string OperationType { get; set; }

        public FileOperationException(string message, string filePath = null, string operationType = null)
            : base(message, "FILE_OPERATION_ERROR", 500)
        {
            FilePath = filePath;
            OperationType = operationType;
        }

        public FileOperationException(string message, Exception innerException, string filePath = null, string operationType = null)
            : base(message, innerException, "FILE_OPERATION_ERROR", 500)
        {
            FilePath = filePath;
            OperationType = operationType;
        }
    }
}
