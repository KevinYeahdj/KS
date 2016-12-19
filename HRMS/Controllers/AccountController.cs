using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HRMS.Data.Entity;
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
            if (SessionHelper.CurrentUser.UserId == "sa")
            {
                ModelState.AddModelError("", "您无权修改超级管理员的密码!");
                return View(model);
            }
            else
            {
                if (model.NewPassword == model.RepeatNewPassword && model.OldPassword == SessionHelper.CurrentUser.PassWord)
                {
                    SessionHelper.CurrentUser.PassWord = model.NewPassword;
                    UserManager um = new UserManager();
                    UserEntity ue = um.GetUser(SessionHelper.CurrentUser.UserId);
                    ue.iPassWord = model.NewPassword;
                    ue.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    um.Update(ue);

                    LoginUserInfo userinfo = SessionHelper.CurrentUser;
                    userinfo.PassWord = model.NewPassword;
                    Session[SessionHelper.CurrentUserKey] = userinfo;

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
                LoginUserInfo sa = new LoginUserInfo { UserId = "sa", UserName = "超级管理员", UserType = "超级管理员", CurrentCompany = "-", CurrentProject = "-", PassWord = "password" };
                Session[SessionHelper.CurrentUserKey] = sa;
                return RedirectToLocal(returnUrl);
            }
            else
            {
                string userName = model.UserName;
                UserManager um = new UserManager();
                UserEntity ui = um.GetUser(userName);
                if (ui == null)
                {
                    ModelState.AddModelError("", "用户名不存在!");
                    return View(model);
                }
                else
                {
                    if (ui.iPassWord == model.Password)
                    {
                        LoginUserInfo user = new LoginUserInfo { CurrentCompany = "-", CurrentProject = "-", UserId = ui.iEmployeeCodeId, UserName = ui.iUserName, UserType = ui.iUserType, PassWord = ui.iPassWord };
                        Session[SessionHelper.CurrentUserKey] = user;
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
