namespace Goverment.Models
{
    public class Resource
    {
        public int ResourceID { get; set; }
        public int ProgramID { get; set; }
        public string Type { get; set; } = string.Empty; // Funds/Materials
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;

        public TradeProgram? TradeProgram { get; set; }
    }
}
