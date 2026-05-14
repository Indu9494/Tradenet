namespace Government.API.Models
{
    public class TradeProgram
    {
        public int ProgramID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; } = string.Empty;

        public ICollection<Resource> Resources { get; set; } = new List<Resource>();
    }
}

