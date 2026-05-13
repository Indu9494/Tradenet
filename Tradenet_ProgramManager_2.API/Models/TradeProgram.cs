namespace Tradenet_ProgramManager_2.API.Models
{
    public class TradeProgram
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
