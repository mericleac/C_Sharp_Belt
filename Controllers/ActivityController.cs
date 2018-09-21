using System;
using Microsoft.AspNetCore.Mvc;
using BeltTemplate.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BeltTemplate.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BeltTemplate.Controllers
{
    public class ActivityController : Controller
    {

        private UserContext _context;

        public ActivityController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("LoggedInUser") == null) {
                return RedirectToAction("LoginReg", "User");
            }
            ViewBag.currUser = _context.Users.SingleOrDefault(x => x.Id == HttpContext.Session.GetInt32("LoggedInUser"));
            List<Activity> allActivities = _context.Activities.ToList();
            foreach(Activity act in allActivities) {
                if (act.StartTime < DateTime.Now) {
                    _context.Activities.Remove(act);
                }
            }
            _context.SaveChanges();
            ViewBag.allActivities = _context.Activities
                .Include(x => x.Participants)
                .ThenInclude(x => x.User)
                .Include(x => x.User)
                .OrderBy(x => x.StartTime).ToList();
            return View();
        }

        [HttpGet]
        [Route("activity/{id}")]
        public IActionResult ShowActivity(int id)
        {
            if (HttpContext.Session.GetInt32("LoggedInUser") == null) {
                return RedirectToAction("LoginReg", "User");
            }
            ViewBag.currUser = _context.Users.SingleOrDefault(x => x.Id == HttpContext.Session.GetInt32("LoggedInUser"));
            Activity activity = _context.Activities
                .Include(x => x.Participants)
                .ThenInclude(x => x.User)
                .SingleOrDefault(x => x.ActivityId == id);
            return View("ShowActivity", activity);
        }

        [HttpGet]
        [Route("new")]
        public IActionResult NewActivity()
        {
            if (HttpContext.Session.GetInt32("LoggedInUser") == null) {
                return RedirectToAction("LoginReg", "User");
            }
            ViewBag.currUser = _context.Users.SingleOrDefault(x => x.Id == HttpContext.Session.GetInt32("LoggedInUser"));
            return View();
        }

        [HttpPost]
        [Route("AddActivity")]
        public IActionResult AddActivity(ActivityViewModel model)
        {
            if (HttpContext.Session.GetInt32("LoggedInUser") == null) {
                return RedirectToAction("LoginReg", "User");
            }
            if (ModelState.IsValid)
            {
                DateTime start = model.Date.AddTicks(model.Time.TimeOfDay.Ticks);
                if (start > DateTime.Now) {
                    Activity act = new Activity() {
                        Title = model.Title,
                        Description = model.Description,
                        StartTime = model.Date.AddTicks(model.Time.TimeOfDay.Ticks),
                        Duration = model.DurationInt.ToString() + " " + model.DurationString,
                        UserId = (int)HttpContext.Session.GetInt32("LoggedInUser")
                    };
                    _context.Activities.Add(act);
                    Participant par = new Participant() {
                        UserId = (int)HttpContext.Session.GetInt32("LoggedInUser"),
                        ActivityId = act.ActivityId
                    };
                    _context.Participants.Add(par);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Date", "Invalid date and time combination, must not be a past date and time.");
            }
            ViewBag.currUser = _context.Users.SingleOrDefault(x => x.Id == HttpContext.Session.GetInt32("LoggedInUser"));
            return View("NewActivity");
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult DeleteActivity(int id)
        {
            if (HttpContext.Session.GetInt32("LoggedInUser") == null) {
                return RedirectToAction("LoginReg", "User");
            }
            Activity act = _context.Activities.SingleOrDefault(x => x.ActivityId == id);
            _context.Activities.Remove(act);
            _context.SaveChanges();
            return RedirectToAction("Index");
            
        }

        [HttpGet]
        [Route("join/{id}/{route?}")]
        public IActionResult JoinActivity(int id, string route = "Index")
        {
            if (HttpContext.Session.GetInt32("LoggedInUser") == null) {
                return RedirectToAction("LoginReg", "User");
            }
            User currUser = _context.Users
                .Include(x => x.Participantings)
                .ThenInclude(x => x.Activity)
                .SingleOrDefault(x => x.Id == HttpContext.Session.GetInt32("LoggedInUser"));
            Activity act = _context.Activities.SingleOrDefault(x => x.ActivityId == id);
            foreach(Participant par in currUser.Participantings)
            {
                DateTime end = DateTime.Now;
                string dur = par.Activity.Duration;
                string temp = new String(dur.Where(Char.IsDigit).ToArray());
                int diff = Int32.Parse(temp);
                if (par.Activity.Duration.Contains("Minute")) {
                    end = par.Activity.StartTime.AddMinutes(diff);
                }
                else if (par.Activity.Duration.Contains("Hour")) {
                    end = par.Activity.StartTime.AddHours(diff);
                }
                else {
                    end = par.Activity.StartTime.AddDays(diff);
                }

                DateTime actEnd = DateTime.Now;
                dur = act.Duration;
                temp = new String(dur.Where(Char.IsDigit).ToArray());
                diff = Int32.Parse(temp);
                if (act.Duration.Contains("Minute")) {
                    actEnd = act.StartTime.AddMinutes(diff);
                }
                else if (act.Duration.Contains("Hour")) {
                    actEnd = act.StartTime.AddHours(diff);
                }
                else {
                    actEnd = act.StartTime.AddDays(diff);
                }

                if (act.StartTime >= par.Activity.StartTime && act.StartTime <= end
                || actEnd >= par.Activity.StartTime && actEnd <= end
                || act.StartTime <= par.Activity.StartTime && actEnd >= end) {
                    TempData["JoinError"] = "Cannot Join activity, time conflict with existing event.";
                    if (route == "showActivity") {
                        return RedirectToAction("ShowActivity", act.ActivityId);
                    }
                    return RedirectToAction(route);
                }
            }
            Participant newPar = new Participant() {
                UserId = currUser.Id,
                ActivityId = act.ActivityId
            };
            _context.Participants.Add(newPar);
            _context.SaveChanges();
            if (route == "showActivity") {
                return RedirectToAction("ShowActivity", act.ActivityId);
            }
            return RedirectToAction(route);
        }

        [HttpGet]
        [Route("leave/{id}/{route?}")]
        public IActionResult LeaveActivity(int id, string route = "Index")
        {
            if (HttpContext.Session.GetInt32("LoggedInUser") == null) {
                return RedirectToAction("LoginReg", "User");
            }
            User currUser = _context.Users.SingleOrDefault(x => x.Id == HttpContext.Session.GetInt32("LoggedInUser"));
            Activity act = _context.Activities.SingleOrDefault(x => x.ActivityId == id);
            Participant par = _context.Participants.SingleOrDefault(x => x.UserId == currUser.Id && x.ActivityId == act.ActivityId);
            _context.Participants.Remove(par);
            _context.SaveChanges();
            if (route == "showActivity") {
                return RedirectToAction("ShowActivity", act.ActivityId);
            }
            return RedirectToAction(route);
        }
    }
}