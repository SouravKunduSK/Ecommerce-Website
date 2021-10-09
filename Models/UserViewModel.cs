using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shopping.Models
{
    public class UserViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 Characters Required..")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password doesn't match!")]
        public string ConfirmPassword { get; set; }
        public Nullable<int> RoleID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Current Password is Required")]
        [DataType(DataType.Password)]
        
        public string OldPassword { get; set; }



    }
}