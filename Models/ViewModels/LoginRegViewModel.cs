using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BeltTemplate.ViewModels;
namespace BeltTemplate.ViewModels
{
    public class LoginRegViewModel
    {
        public RegisterViewModel Register {get;set;}
        public LoginViewModel Login {get;set;}
    }
}