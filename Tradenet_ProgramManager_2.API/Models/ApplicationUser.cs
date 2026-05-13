using Microsoft.AspNetCore.Identity;

namespace Tradenet_ProgramManager_2.API.Models
{
    /// <summary>
    /// Extended IdentityUser to store professional information
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string EmployeeID { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
    }
}
