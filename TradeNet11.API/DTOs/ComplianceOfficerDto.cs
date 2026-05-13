namespace TradeNet11.API.DTOs
{
    /// <summary>
    /// Compliance Officer Data Transfer Object for API responses
    /// </summary>
    public class ComplianceOfficerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for creating a compliance officer
    /// </summary>
    public class CreateComplianceOfficerRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
}
