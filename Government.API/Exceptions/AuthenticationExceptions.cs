namespace Government.API.Exceptions
{
    /// <summary>
    /// Exception thrown when authentication fails
    /// </summary>
    public class AuthenticationException : AppException
    {
        public AuthenticationException(string message = "Authentication failed")
            : base(message, "AUTHENTICATION_FAILED", 401)
        {
        }

        public AuthenticationException(string message, Exception innerException)
            : base(message, innerException, "AUTHENTICATION_FAILED", 401)
        {
        }
    }

    /// <summary>
    /// Exception thrown when user credentials are invalid
    /// </summary>
    public class InvalidCredentialsException : AuthenticationException
    {
        public InvalidCredentialsException(string message = "Invalid email or password")
            : base(message)
        {
            ErrorCode = "INVALID_CREDENTIALS";
        }
    }

    /// <summary>
    /// Exception thrown when user is not found
    /// </summary>
    public class UserNotFoundException : AuthenticationException
    {
        public int? UserId { get; set; }
        public string Email { get; set; }

        public UserNotFoundException(string message = "User not found")
            : base(message)
        {
            ErrorCode = "USER_NOT_FOUND";
        }

        public UserNotFoundException(string message, int userId)
            : base(message)
        {
            ErrorCode = "USER_NOT_FOUND";
            UserId = userId;
        }

        public UserNotFoundException(string message, string email)
            : base(message)
        {
            ErrorCode = "USER_NOT_FOUND";
            Email = email;
        }
    }

    /// <summary>
    /// Exception thrown when user already exists
    /// </summary>
    public class UserAlreadyExistsException : AppException
    {
        public string Email { get; set; }

        public UserAlreadyExistsException(string message = "User already exists", string email = null)
            : base(message, "USER_ALREADY_EXISTS", 400)
        {
            Email = email;
        }
    }

    /// <summary>
    /// Exception thrown when password validation fails
    /// </summary>
    public class InvalidPasswordException : AppException
    {
        public InvalidPasswordException(string message = "Password does not meet security requirements")
            : base(message, "INVALID_PASSWORD", 400)
        {
        }
    }

    /// <summary>
    /// Exception thrown when JWT token is invalid or expired
    /// </summary>
    public class InvalidTokenException : AuthenticationException
    {
        public InvalidTokenException(string message = "Invalid or expired token")
            : base(message)
        {
            ErrorCode = "INVALID_TOKEN";
        }

        public InvalidTokenException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = "INVALID_TOKEN";
        }
    }

    /// <summary>
    /// Exception thrown when user is not authorized to access a resource
    /// </summary>
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "You are not authorized to access this resource")
            : base(message, "UNAUTHORIZED", 403)
        {
        }
    }
}
