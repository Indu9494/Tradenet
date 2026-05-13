using System.ComponentModel.DataAnnotations;

namespace Tradenet_ProgramManager_2.API.Models.ViewModels
{
    /// <summary>
    /// Register view model for creating new user accounts with professional information
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Employee ID is required")]
        [StringLength(20, ErrorMessage = "Employee ID cannot exceed 20 characters")]
        [Display(Name = "Employee ID")]
        public string EmployeeID { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters")]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [StringLength(50, ErrorMessage = "Designation cannot exceed 50 characters")]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Login view model for authenticating existing users
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Login response containing JWT token and user information
    /// </summary>
    public class LoginResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
