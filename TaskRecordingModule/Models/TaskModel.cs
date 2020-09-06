using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskRecordingModule.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Assign User")]
        public string UserId { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
    }
}