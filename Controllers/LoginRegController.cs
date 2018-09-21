using Microsoft.AspNetCore.Mvc;
using BeltTemplate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BeltTemplate.ViewModels;

namespace BeltTemplate.Controllers
{
    public class UserController : Controller
    {
        private UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult LoginReg()
        {
            return View("Index");
        }

        [HttpPost("NewUser")]
        public IActionResult NewUser(LoginRegViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User() {
                    FirstName = model.Register.FirstName,
                    LastName = model.Register.LastName,
                    Email = model.Register.Email
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, model.Register.Password);
                _context.Add(user);
                _context.SaveChanges();
                User currUser = _context.Users.SingleOrDefault(p => p.Email == user.Email);
                HttpContext.Session.SetInt32("LoggedInUser", currUser.Id);
                return RedirectToAction("Index", "Activity");
            }
            return View("Index");
        }

        [HttpPost("LoginUser")]
        public IActionResult LoginUser(LoginRegViewModel model)
        {
            var user = _context.Users.SingleOrDefault(p => p.Email == model.Login.Email);
            if (ModelState.IsValid)
            {
                if (user != null) {
                    var Hasher = new PasswordHasher<User>();
                    var result = Hasher.VerifyHashedPassword(user, user.Password, model.Login.Password);
                    if (result != 0)
                    {
                        HttpContext.Session.SetInt32("LoggedInUser", user.Id);
                        return RedirectToAction("Index", "Activity");
                    }
                }
                ModelState.AddModelError("Login.Email", "User could not be logged in.");
            }
            return View("Index");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginReg");
        }
    }
}