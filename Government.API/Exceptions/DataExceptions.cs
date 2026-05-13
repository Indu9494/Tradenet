namespace Government.API.Exceptions
{
    /// <summary>
    /// Exception thrown when validation fails
    /// </summary>
    public class ValidationException : AppException
    {
        public List<ValidationError> ValidationErrors { get; set; }

        public ValidationException(string message = "Validation failed")
            : base(message, "VALIDATION_FAILED", 400)
        {
            ValidationErrors = new List<ValidationError>();
        }

        public ValidationException(string message, List<ValidationError> errors)
            : base(message, "VALIDATION_FAILED", 400)
        {
            ValidationErrors = errors ?? new List<ValidationError>();
        }

        public void AddValidationError(string field, string message)
        {
            ValidationErrors.Add(new ValidationError { Field = field, Message = message });
        }
    }

    /// <summary>
    /// Represents a single validation error
    /// </summary>
    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Exception thrown when a required resource is not found
    /// </summary>
    public class ResourceNotFoundException : AppException
    {
        public string ResourceType { get; set; }
        public string ResourceIdentifier { get; set; }

        public ResourceNotFoundException(string message, string resourceType = null, string resourceId = null)
            : base(message, "RESOURCE_NOT_FOUND", 404)
        {
            ResourceType = resourceType;
            ResourceIdentifier = resourceId;
        }
    }

    /// <summary>
    /// Exception thrown when a resource already exists
    /// </summary>
    public class ResourceAlreadyExistsException : AppException
    {
        public string ResourceType { get; set; }

        public ResourceAlreadyExistsException(string message, string resourceType = null)
            : base(message, "RESOURCE_ALREADY_EXISTS", 409)
        {
            ResourceType = resourceType;
        }
    }

    /// <summary>
    /// Exception thrown when invalid operation is attempted
    /// </summary>
    public class InvalidOperationException : AppException
    {
        public InvalidOperationException(string message)
            : base(message, "INVALID_OPERATION", 400)
        {
        }
    }

    /// <summary>
    /// Exception thrown when a business rule is violated
    /// </summary>
    public class BusinessRuleViolationException : AppException
    {
        public string RuleName { get; set; }

        public BusinessRuleViolationException(string message, string ruleName = null)
            : base(message, "BUSINESS_RULE_VIOLATION", 422)
        {
            RuleName = ruleName;
        }
    }
}
