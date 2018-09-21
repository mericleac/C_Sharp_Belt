using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeltTemplate.Models 
{
    public class Activity
    {
        [Key]
        public int ActivityId {get;set;}
        public string Title {get;set;}
        public DateTime StartTime {get;set;}
        public string Duration {get;set;}
        public string Description {get;set;}

        public int UserId {get;set;}
        public User User {get;set;}

        public List<Participant> Participants {get;set;}

        public Activity()
        {
            Participants = new List<Participant>();
        }
    }
}