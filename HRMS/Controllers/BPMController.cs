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
using HRMS.Data;
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
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;

            var confirmUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "确认").Replace(",", ";").Replace("，", ";");
            var managerUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "经理").Replace(",", ";").Replace("，", ";");
            var bossUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "高管").Replace(",", ";").Replace("，", ";");
            var tellerUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "出纳").Replace(",", ";").Replace("，", ";");
            //var recordUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "登记").Replace(",", ";").Replace("，", ";");
            ViewBag.confirmUsers = confirmUsers;
            ViewBag.managerUsers = managerUsers;
            ViewBag.bossUsers = bossUsers;
            ViewBag.tellerUsers = tellerUsers;
            //ViewBag.recordUsers = recordUsers;
            return View();
        }

        public ActionResult JournalReApplication()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult JournalApprove()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }

        public ActionResult JournalView()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }

        public ActionResult ReturnFeeApplication()
        {
            DicManager dm = new DicManager();
            var confirmUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "确认").Replace(",", ";").Replace("，", ";");
            var managerUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "经理").Replace(",", ";").Replace("，", ";");
            var bossUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "高管").Replace(",", ";").Replace("，", ";");
            var tellerUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "出纳").Replace(",", ";").Replace("，", ";");
            //var recordUsers = dm.GetUsersByFlowAndRole(SessionHelper.CurrentUser.CurrentCompany, "流水账申请", "登记").Replace(",", ";").Replace("，", ";");
            ViewBag.confirmUsers = confirmUsers;
            ViewBag.managerUsers = managerUsers;
            ViewBag.bossUsers = bossUsers;
            ViewBag.tellerUsers = tellerUsers;
            //ViewBag.recordUsers = recordUsers;
            return View();
        }

        public ActionResult ReturnFeeReApplication()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult ReturnFeeApprove()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }

        public ActionResult ReturnFeeView()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }

        public ActionResult SalaryApplication()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }

        public ActionResult SalaryReApplication()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult SalaryApprove()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }

        public ActionResult SalaryView()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllValidCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllValidProjects();
            ViewBag.Projects = projects;
            return View();
        }

        public ActionResult AdvanceFundApplication()
        {
            return View();
        }
        public ActionResult AdvanceFundReApplication()
        {
            return View();
        }
        public ActionResult AdvanceFundApprove()
        {
            return View();
        }
        public ActionResult AdvanceFundView()
        {
            return View();
        }

        public ActionResult AdvanceFundReturnApplication()
        {
            return View();
        }
        public ActionResult AdvanceFundReturnReApplication()
        {
            return View();
        }
        public ActionResult AdvanceFundReturnApprove()
        {
            return View();
        }
        public ActionResult AdvanceFundReturnView()
        {
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
                bizParaDic.Add("iAppNo", HttpContext.Request.Params["iAppNo"]);
                bizParaDic.Add("iApproveDate", HttpContext.Request.Params["iApproveDate"]);
                bizParaDic.Add("iApproveDate2", HttpContext.Request.Params["iApproveDate2"]);

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
                initiator.Conditions.Add("SysCurrentCompany", SessionHelper.CurrentUser.CurrentCompany); //将发起人公司添加到流程变量里
                DicManager dm = new DicManager();
                initiator.Conditions["sys_summary"] += "申请公司:" + dm.CompanyFirstOrDefault(SessionHelper.CurrentUser.CurrentCompany).iName;


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
                initiator.Conditions.Add("SysCurrentCompany", SessionHelper.CurrentUser.CurrentCompany); //将发起人公司添加到流程变量里
                DicManager dm = new DicManager();
                initiator.Conditions["sys_summary"] += "申请公司:" + dm.CompanyFirstOrDefault(SessionHelper.CurrentUser.CurrentCompany).iName;
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
                        UpdateSysSummary(initiator);
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
                //保存业务数据
                if (SaveBusinessDataForBPMApprove(runner))
                {
                    return new JsonResult { Data = new { success = true, msg = "审批成功!" } };
                }
                else return new JsonResult { Data = new { success = false, msg = "审批成功，但数据出错!" } };

            }
            catch (Exception e)
            {
                ClinBrain.Data.LogFileHelper.ErrorLog(e.ToString());
                return new JsonResult { Data = new { success = false, msg = "审批出错!" } };
            }
        }

        public JsonResult CancelApplication(WfAppRunner runner)
        {
            try
            {
                IWorkflowService service = new WorkflowService();
                string result = service.CancelApplication(runner);
                if (result == "success")
                {
                    SaveBusinessDataForCancelApplication(runner.ProcessGUID, runner.AppInstanceID);  //撤销时对业务数据的处理
                    return new JsonResult { Data = new { success = true, msg = "撤销成功!" } };
                }
                return new JsonResult { Data = new { success = false, msg = result } };
            }
            catch (Exception e)
            {
                ClinBrain.Data.LogFileHelper.ErrorLog(e.ToString());
                return new JsonResult { Data = new { success = false, msg = "撤销出错!" } };
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
        private void UpdateSysSummary(WfAppRunner runner)
        {
            BPMManager service = new BPMManager();
            service.UpdateSysSummary(runner.Conditions["sys_summary"], runner.AppInstanceID);
        }

        private bool SaveBusinessDataForBPM(string buzJson, string pguid, string appNo)
        {
            try
            {
                bool result = false;
                if (pguid == "09e8624f-ff2d-cc98-0eaa-6a11f3f7d9bc") //流水账
                {
                    if (buzJson.StartsWith("|"))
                    {
                        string amount = buzJson.Split('|')[1];
                        AdvanceFundManager afService = new AdvanceFundManager();
                        var af = afService.FirstOrDefault(appNo);
                        if (af != null)
                        {
                            af.iAmount = decimal.Parse(amount);
                            afService.Update(af);
                        }
                        else
                        {
                            AdvanceFundEntity afNew = new AdvanceFundEntity();
                            afNew.iAmount = decimal.Parse(amount);
                            afNew.iApplicant = SessionHelper.CurrentUser.UserName + "(" + SessionHelper.CurrentUser.UserId + ")";
                            afNew.iCompanyId = SessionHelper.CurrentUser.CurrentCompany;
                            afNew.iProjectId = SessionHelper.CurrentUser.CurrentProject;
                            afNew.iAppNo = appNo;
                            afNew.iRecordNote = "流水账备用金核销";
                            afService.Insert(afNew);
                        }
                        buzJson = buzJson.Substring(amount.Length + 2);
                    }
                    else
                    {
                        AdvanceFundManager afService = new AdvanceFundManager();
                        var af = afService.FirstOrDefault(appNo);
                        if (af != null)  //从备用金核销到现金
                        {
                            af.iIsDeleted = 1;
                            afService.Update(af);
                        }
                    }
                    JournalManager service = new JournalManager();
                    JsonSerializerSettings st = new JsonSerializerSettings();
                    st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    List<JournalEntity> entities = JsonConvert.DeserializeObject<List<JournalEntity>>(buzJson, st);
                    service.BatchUpdate(entities, appNo);
                    result = true;
                }
                else if (pguid == "eb2844bd-0ffd-9eaa-6068-910b66fad9d9") //返费
                {
                    if (buzJson.StartsWith("|"))
                    {
                        string amount = buzJson.Split('|')[1];
                        AdvanceFundManager afService = new AdvanceFundManager();
                        var af = afService.FirstOrDefault(appNo);
                        if (af != null)
                        {
                            af.iAmount = decimal.Parse(amount);
                            afService.Update(af);
                        }
                        else
                        {
                            AdvanceFundEntity afNew = new AdvanceFundEntity();
                            afNew.iAmount = decimal.Parse(amount);
                            afNew.iApplicant = SessionHelper.CurrentUser.UserName + "(" + SessionHelper.CurrentUser.UserId + ")";
                            afNew.iCompanyId = SessionHelper.CurrentUser.CurrentCompany;
                            afNew.iProjectId = SessionHelper.CurrentUser.CurrentProject;
                            afNew.iAppNo = appNo;
                            afNew.iRecordNote = "返费备用金核销";
                            afService.Insert(afNew);
                        }
                        buzJson = buzJson.Substring(amount.Length + 2);
                    }
                    else
                    {
                        AdvanceFundManager afService = new AdvanceFundManager();
                        var af = afService.FirstOrDefault(appNo);
                        if (af != null)  //从备用金核销到现金
                        {
                            af.iIsDeleted = 1;
                            afService.Update(af);
                        }
                    }
                    ReturnFeeManager service = new ReturnFeeManager();
                    JsonSerializerSettings st = new JsonSerializerSettings();
                    st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    List<ReturnFeeHistoryEntity> entities = JsonConvert.DeserializeObject<List<ReturnFeeHistoryEntity>>(buzJson, st);

                    foreach (var entity in entities)
                    {
                        entity.iGuid = Guid.NewGuid().ToString();
                        entity.iCreatedBy = SessionHelper.CurrentUser.UserName;
                        entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                        entity.iReturnFeeAppNo = appNo;
                    }
                    entities.RemoveAll(i => string.IsNullOrEmpty(i.iReturnFeeGuid));
                    service.BatchInsertReturnFeeHistoryApplication(entities);
                    result = true;
                }
                else if (pguid == "c6f4e09b-e355-b8cd-ac9f-ec7995c23f6f")   //工资
                {
                    SalaryManager service = new SalaryManager();
                    JsonSerializerSettings st = new JsonSerializerSettings();
                    st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    List<SalaryEntity> entities = JsonConvert.DeserializeObject<List<SalaryEntity>>(buzJson, st);
                    service.BatchUpdate4Flow(entities, appNo);
                    result = true;

                }
                else if (pguid == "843f9b74-2264-8c32-fb36-15e8efb94959")   //备用金
                {
                    AdvanceFundManager service = new AdvanceFundManager();
                    JsonSerializerSettings st = new JsonSerializerSettings();
                    st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    AdvanceFundEntity entity = JsonConvert.DeserializeObject<AdvanceFundEntity>(buzJson, st);
                    entity.iApplicant = SessionHelper.CurrentUser.UserName + "(" + SessionHelper.CurrentUser.UserId + ")";
                    entity.iCompanyId = SessionHelper.CurrentUser.CurrentCompany;
                    entity.iProjectId = SessionHelper.CurrentUser.CurrentProject;
                    entity.iAppNo = appNo;
                    entity.iRecordNote = "备用金申请";
                    if (string.IsNullOrEmpty(entity.iGuid))
                    {
                        service.Insert(entity);
                    }
                    else
                    {
                        service.Update(entity);
                    }
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error("保存流程业务数据出错！", ex);
                return false;

            }
        }

        private bool SaveBusinessDataForCancelApplication(string pguid, string appNo)
        {
            try
            {
                bool result = false;
                if (pguid == "09e8624f-ff2d-cc98-0eaa-6a11f3f7d9bc") //流水账
                {
                    JournalManager service = new JournalManager();
                    List<JournalEntity> entities = new List<JournalEntity>();//清空所有关联项
                    service.BatchUpdate(entities, appNo);
                    result = true;
                }
                else if (pguid == "eb2844bd-0ffd-9eaa-6068-910b66fad9d9") //返费
                {
                    ReturnFeeManager service = new ReturnFeeManager();
                    List<ReturnFeeHistoryEntity> entities = new List<ReturnFeeHistoryEntity>();
                    service.ResetValidReturnFeeList(appNo);
                    result = true;
                }
                else if (pguid == "c6f4e09b-e355-b8cd-ac9f-ec7995c23f6f")   //工资
                {
                    SalaryManager service = new SalaryManager();
                    service.BatchUpdate4Flow(null, appNo);
                    result = true;

                }
                else if (pguid == "843f9b74-2264-8c32-fb36-15e8efb94959")   //备用金
                {
                    AdvanceFundManager service = new AdvanceFundManager();
                    JsonSerializerSettings st = new JsonSerializerSettings();
                    st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    AdvanceFundEntity entity = service.FirstOrDefault(appNo);
                    entity.iIsDeleted = 1;
                    service.Update(entity);
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error("撤销流程业务数据出错！", ex);
                return false;

            }
        }

        private bool SaveBusinessDataForBPMApprove(WfAppRunner runner)
        {
            try
            {
                //if (runner.ProcessGUID == "09e8624f-ff2d-cc98-0eaa-6a11f3f7d9bc")  //临时记录排错
                //{
                //    LogFileHelper.ErrorLog(runner.Other + "--" + runner.AppInstanceID + "--" + runner.Conditions["sys_feedback"]);
                //}

                bool result = true;
                if (runner.ProcessGUID == "09e8624f-ff2d-cc98-0eaa-6a11f3f7d9bc" && runner.Other == "出纳") //流水账 将财务意见备注到Note里
                {
                    JournalManager service = new JournalManager();
                    result = service.UpdateNoteByFlow(runner.Conditions["sys_feedback"], runner.AppInstanceID);
                }
                return result;
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error("保存流程业务数据出错！", ex);
                return false;

            }
        }

    }
}
