using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BeltTemplate.ViewModels;
namespace BeltTemplate.ViewModels
{ 
    public class RegisterViewModel
    {
        [Required(ErrorMessage="First Name is required.")]
        [MinLength(2, ErrorMessage="First Name must be at least 2 characters.")]
        [RegularExpression("^((?![0-9]).)*$", ErrorMessage="First Name may only contain non-numeric characters")]
        public string FirstName {get;set;}
        [Required(ErrorMessage="Last Name is required.")]
        [MinLength(2, ErrorMessage="Last Name must be at least 2 characters.")]
        [RegularExpression("^((?![0-9]).)*$", ErrorMessage="Last Name may only contain non-numeric characters")]
        public string LastName {get;set;}
        [Required(ErrorMessage="Email is required.")]
        [EmailAddress(ErrorMessage="Must be a valid email.")]
        public string Email {get;set;}
        [Required(ErrorMessage="Password is required.")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters.")]
        [MaxLength(15, ErrorMessage="Password must be less than 16 characters.")]
        [RegularExpression(@"(^(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$)", ErrorMessage="Password must contain one letter, one number, and one special character")]
        public string Password {get;set;}
        [Required(ErrorMessage="Password Confirm is required.")]
        [Compare("Password", ErrorMessage="Passwords do not match.")]
        public string ConfirmPassword {get;set;}
    }
}