namespace Goverment.Models
{
    public class Business
    {
        public int BusinessID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Trader/Exporter/Importer
        public string Address { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
