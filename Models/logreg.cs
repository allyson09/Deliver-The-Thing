using System.ComponentModel.DataAnnotations;

namespace DeliverTheThing.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class Register
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Must put valid email.")]
        public string Email { get; set; }
        [Required]
        [MinLength(6,  ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [DataType(DataType.Password)]
        public string ConPassword { get; set; }
    }
    public class LogRegModels 
    {
        public Register Reg{get;set;}
        public Login Log{get;set;}
    }
}