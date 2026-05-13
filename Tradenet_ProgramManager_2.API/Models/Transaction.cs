namespace Tradenet_ProgramManager_2.API.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public TradeProgram? Program { get; set; }
    }
}
