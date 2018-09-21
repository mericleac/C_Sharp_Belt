using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BeltTemplate.ViewModels;
namespace BeltTemplate.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage="Please enter your email.")]
        public string Email {get;set;}
        [Required(ErrorMessage="Please enter your password.")]
        public string Password {get;set;}
    }
}