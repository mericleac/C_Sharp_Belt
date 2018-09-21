using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BeltTemplate.Models
{
    public class Participant
    {
        public int ParticipantId {get;set;}

        public int UserId {get;set;}
        public User User {get;set;}

        public int ActivityId {get;set;}
        public Activity Activity {get;set;}
    }
}