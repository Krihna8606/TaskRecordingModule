using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskRecordingModule.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Display(Name = "Full Name")]



        public string Name { get; set; }






        [Display(Name = "Contact Number")]

        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string UserType { get; set; }


        public Nullable<System.DateTime> CreatedDate { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]


        [EmailAddress]
        public string Email { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]


        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]



        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is required")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

    }
    public class RegisterModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]




       
        public string Name { get; set; }
        [Display(Name = "Contact Number")]




        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid Contact Number.")]
        public string ContactNumber { get; set; }


        public string Address { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [EmailAddress]



        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]



        [StringLength(10, MinimumLength = 4, ErrorMessage = "Invalid Password Length.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]


        //[Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is required")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }



        public string UserType { get; set; }

    }
}