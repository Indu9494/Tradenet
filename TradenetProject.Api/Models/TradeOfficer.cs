using System.ComponentModel.DataAnnotations;

namespace TradeNetProject.Models
{
    public class TradeOfficer
    {
        [Key]
        public int OfficerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }
        public string Region { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Status { get; set; }
    }
}
