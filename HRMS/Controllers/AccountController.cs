using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClinBrain.Data.Entity;
using HRMS.Data.Manager;
using HRMS.WEB.Models;
using HRMS.WEB.Utils;
using Newtonsoft.Json;

namespace HRMS.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            Session[SessionHelper.CurrentUserKey] = null;
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (SessionHelper.CurrentUser.iUserName == "sa")
            {
                ModelState.AddModelError("", "您无权修改超级管理员的密码!");
                return View(model);
            }
            else
            {
                if(model.NewPassword == model.RepeatNewPassword && model.OldPassword == SessionHelper.CurrentUser.iPassWord)
                {
                    SessionHelper.CurrentUser.iPassWord = model.NewPassword;
                    UserManager um = new UserManager();
                    um.Update(SessionHelper.CurrentUser);
                    ModelState.AddModelError("", "修改成功，即将进入系统！");
                    Response.Write("<script language='javascript'>setTimeout(\"window.opener=null;window.location.href=\'/Home/Index'\",1500);</script>");
                    //return RedirectToAction("Index", "Home");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", "旧密码错误!");
                    return View(model);
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.UserName == "sa" && model.Password == "password")
            {
                UserEntity sa = new UserEntity { iUserName = "超级管理员", iEmployeeCodeId = "sa", iUserType = "超级管理员", iCompanyCode = "-" };
                Session[SessionHelper.CurrentUserKey] = sa;
                return RedirectToLocal(returnUrl);
            }
            else
            {
                string userName = model.UserName;
                UserManager um = new UserManager();
                UserEntity ui = um.GetUser(userName);
                Session[SessionHelper.CurrentUserKey] = ui;
                if (ui == null)
                {
                    ModelState.AddModelError("", "用户名不存在!");
                    return View(model);
                }
                else
                {
                    if (ui.iPassWord == model.Password)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "密码错误!");
                        return View(model);
                    }
                }
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
