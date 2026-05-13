namespace Government.API.Exceptions
{
    /// <summary>
    /// Exception thrown when database operation fails
    /// </summary>
    public class DatabaseException : AppException
    {
        public DatabaseException(string message = "Database operation failed")
            : base(message, "DATABASE_ERROR", 500)
        {
        }

        public DatabaseException(string message, Exception innerException)
            : base(message, innerException, "DATABASE_ERROR", 500)
        {
        }
    }

    /// <summary>
    /// Exception thrown when database connection fails
    /// </summary>
    public class DatabaseConnectionException : DatabaseException
    {
        public DatabaseConnectionException(string message = "Failed to connect to database")
            : base(message)
        {
            ErrorCode = "DATABASE_CONNECTION_ERROR";
        }

        public DatabaseConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = "DATABASE_CONNECTION_ERROR";
        }
    }

    /// <summary>
    /// Exception thrown when database transaction fails
    /// </summary>
    public class DatabaseTransactionException : DatabaseException
    {
        public DatabaseTransactionException(string message = "Database transaction failed")
            : base(message)
        {
            ErrorCode = "TRANSACTION_ERROR";
        }

        public DatabaseTransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = "TRANSACTION_ERROR";
        }
    }

    /// <summary>
    /// Exception thrown when database concurrency conflict occurs
    /// </summary>
    public class ConcurrencyException : DatabaseException
    {
        public ConcurrencyException(string message = "The record has been modified by another process")
            : base(message)
        {
            ErrorCode = "CONCURRENCY_CONFLICT";
            HttpStatusCode = 409;
        }
    }

    /// <summary>
    /// Exception thrown when a migration fails
    /// </summary>
    public class MigrationException : DatabaseException
    {
        public MigrationException(string message = "Database migration failed")
            : base(message)
        {
            ErrorCode = "MIGRATION_ERROR";
        }

        public MigrationException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = "MIGRATION_ERROR";
        }
    }
}
