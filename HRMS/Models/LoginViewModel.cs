using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.WEB.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

    }


    public class ChangePasswordViewModel
    {

        [Required(ErrorMessage = "旧密码不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "旧密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "新密码不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "确认新密码不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        public string RepeatNewPassword { get; set; }

    }
}