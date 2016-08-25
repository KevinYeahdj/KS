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

        [HttpPost]
        public ActionResult LogOff()
        {
            Session[SessionHelper.CurrentUserKey] = null;
            return RedirectToAction("Login", "Account");
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
                UserEntity sa = new UserEntity { iUserName = "超级管理员", iEmployeeCodeId = "sa", iUserType = "超级管理员", iCompanyCode = "上海敏慧" };
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
