using System;
using System.ComponentModel.DataAnnotations;

namespace BeltTemplate.ViewModels 
{
    public class ActivityViewModel
    {
        [Key]
        public int ActivityId {get;set;}
        [Required(ErrorMessage="Title is required.")]
        [MinLength(2, ErrorMessage="Title must be at least 2 characters.")]
        public string Title {get;set;}
        [Required(ErrorMessage="Date is required.")]
        public DateTime Date {get;set;}
        [Required(ErrorMessage="Time is required.")]
        public DateTime Time {get;set;}
        [Required(ErrorMessage="Duration is required.")]
        public int DurationInt {get;set;}
        [Required(ErrorMessage="Duration unit is required.")]
        public string DurationString {get;set;}
        [Required(ErrorMessage="Description is required.")]
        [MinLength(10, ErrorMessage="Description must be at least 10 characters.")]
        public string Description {get;set;}
    }
}