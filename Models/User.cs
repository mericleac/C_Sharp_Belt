using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BeltTemplate.Models
{
    public class User
    {
        [Key]
        public int Id {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}

        public List<Activity> Activities {get;set;}

        public List<Participant> Participantings {get;set;}

        public User()
        {
            Activities = new List<Activity>();
            Participantings = new List<Participant>();
        }
    }
}