using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClinBrain.Data.Service;
using ClinBrain.OC;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Business.Manager;
using ClinBrain.WorkFlowEngine.Common;
using ClinBrain.WorkFlowEngine.Service;
using HRMS.Data.Entity;
using HRMS.Data.Manager;
using HRMS.WEB.Models;
using HRMS.WEB.Utils;
using Newtonsoft.Json;

namespace HRMS.Controllers
{
    public class BPMController : BaseController
    {
        //
        // GET: /BPM/
        /// <summary>
        /// 待办
        /// </summary>
        /// <returns></returns>
        public ActionResult Todo()
        {
            return View();
        }
        /// <summary>
        /// 完成
        /// </summary>
        /// <returns></returns>
        public ActionResult Done()
        {
            return View();
        }
        /// <summary>
        /// 我的申请
        /// </summary>
        /// <returns></returns>
        public ActionResult Myapp()
        {
            return View();
        }

        public ActionResult JournalApplication()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;

            var financeUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "财务").Replace(",", ";").Replace("，", ";");
            var managerUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "高管").Replace(",", ";").Replace("，", ";");
            ViewBag.managerUser = managerUsers;
            ViewBag.financeUser = financeUsers;
            return View();
        }

        public ActionResult JournalReApplication()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult JournalApprove()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }

        public ActionResult JournalView()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }

    }

    public class BPMAjaxController : Controller
    {
        public void GetMyTodoList()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("currentUserId", SessionHelper.CurrentUser.UserId);

                int total = 0;
                BPMManager service = new BPMManager();
                List<TodoViewModel> list = service.GetMyTodoList(bizParaDic, sort, order, offset, pageSize, out total);

                //给分页实体赋值  
                PageModels<TodoViewModel> model = new PageModels<TodoViewModel>();
                model.total = total;
                if (total % pageSize == 0)
                    model.page = total / pageSize;
                else
                    model.page = (total / pageSize) + 1;

                model.rows = list;

                //将查询结果返回  
                HttpContext.Response.Write(jss.Serialize(model));
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
            }
        }

        public void GetMyDoneList()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("currentUserId", SessionHelper.CurrentUser.UserId);

                int total = 0;
                BPMManager service = new BPMManager();
                List<DoneViewModel> list = service.GetMyDoneList(bizParaDic, sort, order, offset, pageSize, out total);

                //给分页实体赋值  
                PageModels<DoneViewModel> model = new PageModels<DoneViewModel>();
                model.total = total;
                if (total % pageSize == 0)
                    model.page = total / pageSize;
                else
                    model.page = (total / pageSize) + 1;

                model.rows = list;

                //将查询结果返回  
                HttpContext.Response.Write(jss.Serialize(model));
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
            }
        }

        public void GetMyApplicationList()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("currentUserId", SessionHelper.CurrentUser.UserId);

                int total = 0;
                BPMManager service = new BPMManager();
                List<ApplicationViewModel> list = service.GetMyApplicationList(bizParaDic, sort, order, offset, pageSize, out total);

                //给分页实体赋值  
                PageModels<ApplicationViewModel> model = new PageModels<ApplicationViewModel>();
                model.total = total;
                if (total % pageSize == 0)
                    model.page = total / pageSize;
                else
                    model.page = (total / pageSize) + 1;

                model.rows = list;

                //将查询结果返回  
                HttpContext.Response.Write(jss.Serialize(model));
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
            }
        }

        public void GetFlowLogs()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);
                string appno = HttpContext.Request.Params["appno"];

                ApproveInfoManager appma = new ApproveInfoManager();
                List<ApproveInfo> approveinfos = appma.GetAllList(appno);
                List<ApproveInfo> activeSteps = appma.GetActiveSteps(appno);
                List<ApproveInfo> list = new List<ApproveInfo>();
                if (activeSteps != null)
                    list.AddRange(activeSteps);
                if (approveinfos != null)
                    list.AddRange(approveinfos);

                int total = list.Count();
                //给分页实体赋值  
                PageModels<ApproveInfo> model = new PageModels<ApproveInfo>();
                model.total = total;
                if (total % pageSize == 0)
                    model.page = total / pageSize;
                else
                    model.page = (total / pageSize) + 1;

                model.rows = list;

                //将查询结果返回  
                HttpContext.Response.Write(jss.Serialize(model));
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
            }
        }

        public JsonResult StartApplication(WfAppRunner initiator)
        {
            string appNo = "";
            try
            {
                OrganizationService oc = new OrganizationService();
                UserInfo ur = oc.GetUserInfoByLoginName(initiator.UserID);
                initiator.UserName = ur == null ? "" : ur.Name;

                IWorkflowService service = new WorkflowService();
                appNo = service.StartApplication(initiator);
                if (appNo == "fail")
                {
                    return new JsonResult { Data = new { success = false, msg = "申请出错!" } };
                }
                else
                {

                    SaveApproveInfo(initiator);
                    //保存业务数据
                    if (SaveBusinessDataForBPM(initiator.Other, initiator.ProcessGUID, appNo))
                    {
                        return new JsonResult { Data = new { success = true, msg = appNo } };
                    }
                    else
                    {
                        initiator.AppInstanceID = appNo;
                        string result = service.CancelApplication(initiator); //保存业务数据失败就回退
                        if (result != "success")
                        {
                            log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                            log.Error(result);
                        }
                        return new JsonResult { Data = new { success = false, msg = "申请出错!" } };
                    }
                }
            }
            catch (Exception e)
            {
                return new JsonResult { Data = new { success = false, msg = "申请出错!" } };
            }
        }

        public JsonResult ReStartApplication(WfAppRunner initiator)
        {
            try
            {
                //保存业务数据
                if (SaveBusinessDataForBPM(initiator.Other, initiator.ProcessGUID, initiator.AppInstanceID))
                {
                    OrganizationService oc = new OrganizationService();
                    UserInfo ur = oc.GetUserInfoByLoginName(initiator.UserID);
                    initiator.UserName = ur == null ? "" : ur.Name;

                    IWorkflowService service = new WorkflowService();
                    string result = service.StartApproval(initiator);
                    if (result == "fail")
                    {
                        return new JsonResult { Data = new { success = false, msg = "提交出错!" } };
                    }
                    else
                    {
                        SaveApproveInfo(initiator);
                        return new JsonResult { Data = new { success = true, msg = "提交成功!" } };
                    }
                }
                else
                {
                    return new JsonResult { Data = new { success = false, msg = "提交出错!" } };
                }
            }
            catch (Exception e)
            {
                return new JsonResult { Data = new { success = false, msg = "提交出错!" } };
            }
        }

        public JsonResult StartApproval(WfAppRunner runner)
        {
            try
            {
                OrganizationService oc = new OrganizationService();
                UserInfo ur = oc.GetUserInfoByLoginName(runner.UserID);
                runner.UserName = ur == null ? "" : ur.Name;

                IWorkflowService service = new WorkflowService();
                string result = service.StartApproval(runner);
                if (result == "fail")
                {
                    return new JsonResult { Data = new { success = false, msg = "审批出错!" } };
                }
                SaveApproveInfo(runner);
                return new JsonResult { Data = new { success = true, msg = "审批成功!" } };
            }
            catch (Exception e)
            {
                return new JsonResult { Data = new { success = false, msg = "审批出错!" } };
            }
        }

        private void SaveApproveInfo(WfAppRunner runner)
        {
            string approveType = "";
            string approveTypeName = "";
            string feedback = "";
            if (runner.Conditions.ContainsKey("sys_approve"))
            {
                approveType = runner.Conditions["sys_approve"];
            }
            if (runner.Conditions.ContainsKey("sys_approvecn"))
            {
                approveTypeName = runner.Conditions["sys_approvecn"];
            }
            if (runner.Conditions.ContainsKey("sys_feedback"))
            {
                feedback = runner.Conditions["sys_feedback"];
            }

            ApproveInfo approveInfo = new ApproveInfo();
            approveInfo.TaskID = runner.TaskID;
            approveInfo.AppNo = runner.AppInstanceID;
            approveInfo.ApproverId = runner.UserID;
            approveInfo.ApproverName = runner.UserName;
            approveInfo.ApproveType = approveType;
            approveInfo.ApproveTypeName = approveTypeName;
            approveInfo.CreateTime = DateTime.Now;
            approveInfo.FeedBack = feedback;
            approveInfo.ProcessName = runner.AppName;
            approveInfo.StepName = runner.CurrentStepName;
            ApproveInfoManager apim = new ApproveInfoManager();
            apim.Insert(approveInfo);
        }

        private bool SaveBusinessDataForBPM(string buzJson, string pguid, string appNo)
        {
            bool result = false;
            if (pguid == "09e8624f-ff2d-cc98-0eaa-6a11f3f7d9bc")
            {
                JournalManager service = new JournalManager();
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                List<JournalEntity> entities = JsonConvert.DeserializeObject<List<JournalEntity>>(buzJson, st);

                foreach (var entity in entities)
                {
                    entity.iGuid = Guid.NewGuid().ToString();
                    entity.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    entity.iAppNo = appNo;
                }
                service.BatchInsert(entities);
                result = true;
            }
            return result;
        }

    }
}
