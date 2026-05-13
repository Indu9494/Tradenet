namespace Tradenet_ProgramManager_2.API.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalPrograms { get; set; }
        public decimal BudgetUsed { get; set; }
        public string MarketHealth { get; set; } = "Good";
        public decimal TotalSales { get; set; }
        public decimal TotalPurchases { get; set; }
        public decimal NetBalance { get; set; }
    }
}
