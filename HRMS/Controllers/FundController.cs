using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClinBrain.Data.Entity;
using HRMS.Common;
using HRMS.Data.Manager;
using HRMS.WEB.Models;
using HRMS.WEB.Utils;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HRMS.Controllers
{
    public class FundController : BaseController
    {
        public ActionResult SocialSecurityIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            ViewBag.Companies = companies;
            var projects = dm.GetDicByType("项目");
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult SocialSecurityDetail()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            ViewBag.Companies = companies;
            var projects = dm.GetDicByType("项目");
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult ProvidentFundIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            ViewBag.Companies = companies;
            var projects = dm.GetDicByType("项目");
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult ProvidentFundDetail()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            ViewBag.Companies = companies;
            var projects = dm.GetDicByType("项目");
            ViewBag.Projects = projects;
            return View();
        }
    }

    public class FundAjaxController : Controller
    {
        public void GetAllSocialSecurity()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                string searchKey = HttpContext.Request.Params["search"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("search", searchKey);
                Dictionary<string, string> bizParaDicTemp = new Dictionary<string, string>();

                foreach (string para in HttpContext.Request.Params.Keys)
                {
                    if (para.StartsWith("s") && (SocialSecurityManager.SocialSecurityViewDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && SocialSecurityManager.SocialSecurityViewDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
                    {
                        bizParaDicTemp.Add("i" + para.Substring(1, para.Length - 1), HttpContext.Request.Params[para]);
                    }
                }
                foreach (var item in bizParaDicTemp)
                {
                    if (item.Key.EndsWith("2"))
                        continue;
                    if (bizParaDicTemp.ContainsKey(item.Key + "2"))
                    {
                        bizParaDic.Add(item.Key + "[d]", item.Value + "§" + bizParaDicTemp[item.Key + "2"]);
                    }
                    else
                    {
                        bizParaDic.Add(item.Key, item.Value);
                    }
                }

                int total = 0;
                SocialSecurityManager service = new SocialSecurityManager();
                List<SocialSecurityModel> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);

                DicManager dm = new DicManager();
                var companies = dm.GetDicByType("公司");
                var projects = dm.GetDicByType("项目");
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iKey, i => i.iValue);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iKey, i => i.iValue);
                foreach (var item in list)
                {
                    item.iCompany = comDic[item.iCompany];
                    item.iItemName = proDic[item.iItemName];
                }

                //给分页实体赋值  
                PageModels<SocialSecurityModel> model = new PageModels<SocialSecurityModel>();
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

        public void GetAllSocialSecurityDetail()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                string searchKey = HttpContext.Request.Params["search"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("search", searchKey);
                Dictionary<string, string> bizParaDicTemp = new Dictionary<string, string>();

                foreach (string para in HttpContext.Request.Params.Keys)
                {
                    if (para.StartsWith("s") && (SocialSecurityManager.SocialSecurityDetailViewDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && SocialSecurityManager.SocialSecurityDetailViewDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
                    {
                        bizParaDicTemp.Add("i" + para.Substring(1, para.Length - 1), HttpContext.Request.Params[para]);
                    }
                }
                foreach (var item in bizParaDicTemp)
                {
                    if (item.Key.EndsWith("2"))
                        continue;
                    if (bizParaDicTemp.ContainsKey(item.Key + "2"))
                    {
                        bizParaDic.Add(item.Key + "[i]", item.Value + "§" + bizParaDicTemp[item.Key + "2"]);
                    }
                    else
                    {
                        bizParaDic.Add(item.Key, item.Value);
                    }
                }

                int total = 0;
                SocialSecurityManager service = new SocialSecurityManager();
                List<SocialSecurityDetailModel> list = service.GetDetailSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);

                DicManager dm = new DicManager();
                var companies = dm.GetDicByType("公司");
                var projects = dm.GetDicByType("项目");
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iKey, i => i.iValue);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iKey, i => i.iValue);
                foreach (var item in list)
                {
                    item.iCompany = comDic[item.iCompany];
                    item.iItemName = proDic[item.iItemName];
                }

                //给分页实体赋值  
                PageModels<SocialSecurityDetailModel> model = new PageModels<SocialSecurityDetailModel>();
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
        public void GetAllProvidentFund()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                string searchKey = HttpContext.Request.Params["search"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("search", searchKey);
                Dictionary<string, string> bizParaDicTemp = new Dictionary<string, string>();

                foreach (string para in HttpContext.Request.Params.Keys)
                {
                    if (para.StartsWith("s") && (SocialSecurityManager.SocialSecurityViewDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && SocialSecurityManager.SocialSecurityViewDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
                    {
                        bizParaDicTemp.Add("i" + para.Substring(1, para.Length - 1), HttpContext.Request.Params[para]);
                    }
                }
                foreach (var item in bizParaDicTemp)
                {
                    if (item.Key.EndsWith("2"))
                        continue;
                    if (bizParaDicTemp.ContainsKey(item.Key + "2"))
                    {
                        bizParaDic.Add(item.Key + "[d]", item.Value + "§" + bizParaDicTemp[item.Key + "2"]);
                    }
                    else
                    {
                        bizParaDic.Add(item.Key, item.Value);
                    }
                }

                int total = 0;
                ProvidentFundManager service = new ProvidentFundManager();
                List<ProvidentFundModel> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);

                DicManager dm = new DicManager();
                var companies = dm.GetDicByType("公司");
                var projects = dm.GetDicByType("项目");
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iKey, i => i.iValue);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iKey, i => i.iValue);
                foreach (var item in list)
                {
                    item.iCompany = comDic[item.iCompany];
                    item.iItemName = proDic[item.iItemName];
                }

                //给分页实体赋值  
                PageModels<ProvidentFundModel> model = new PageModels<ProvidentFundModel>();
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

        public void GetAllProvidentFundDetail()
        {
            try
            {
                //用于序列化实体类的对象  
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                //请求中携带的条件  
                string order = HttpContext.Request.Params["order"];
                string sort = HttpContext.Request.Params["sort"];
                string searchKey = HttpContext.Request.Params["search"];
                int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
                int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("search", searchKey);
                Dictionary<string, string> bizParaDicTemp = new Dictionary<string, string>();

                foreach (string para in HttpContext.Request.Params.Keys)
                {
                    if (para.StartsWith("s") && (SocialSecurityManager.SocialSecurityViewDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && SocialSecurityManager.SocialSecurityViewDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
                    {
                        bizParaDicTemp.Add("i" + para.Substring(1, para.Length - 1), HttpContext.Request.Params[para]);
                    }
                }
                foreach (var item in bizParaDicTemp)
                {
                    if (item.Key.EndsWith("2"))
                        continue;
                    if (bizParaDicTemp.ContainsKey(item.Key + "2"))
                    {
                        bizParaDic.Add(item.Key + "[i]", item.Value + "§" + bizParaDicTemp[item.Key + "2"]);
                    }
                    else
                    {
                        bizParaDic.Add(item.Key, item.Value);
                    }
                }

                int total = 0;
                ProvidentFundManager service = new ProvidentFundManager();
                List<ProvidentFundDetailModel> list = service.GetDetailSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);

                DicManager dm = new DicManager();
                var companies = dm.GetDicByType("公司");
                var projects = dm.GetDicByType("项目");
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iKey, i => i.iValue);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iKey, i => i.iValue);
                foreach (var item in list)
                {
                    item.iCompany = comDic[item.iCompany];
                    item.iItemName = proDic[item.iItemName];
                }

                //给分页实体赋值  
                PageModels<ProvidentFundDetailModel> model = new PageModels<ProvidentFundDetailModel>();
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
        public JsonResult GetSocialSecurity(string hrguid)
        {
            try
            {
                SocialSecurityManager service = new SocialSecurityManager();
                SocialSecurityModel entity = service.GetFirstOrDefault(hrguid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = entity }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult GetSocialSecurityDetail(string iguid)
        {
            try
            {
                SocialSecurityManager service = new SocialSecurityManager();
                SocialSecurityDetailModel entity = service.GetDetailFirstOrDefault(iguid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = entity }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult GetProvidentFund(string hrguid)
        {
            try
            {
                ProvidentFundManager service = new ProvidentFundManager();
                ProvidentFundModel entity = service.GetFirstOrDefault(hrguid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = entity }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult GetProvidentFundDetail(string iguid)
        {
            try
            {
                ProvidentFundManager service = new ProvidentFundManager();
                ProvidentFundDetailModel entity = service.GetDetailFirstOrDefault(iguid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = entity }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public string SocialSecuritySaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                SocialSecurityModel entity = JsonConvert.DeserializeObject<SocialSecurityModel>(jsonString, st);
                SocialSecurityManager service = new SocialSecurityManager();
                HRInfoManager hrService = new HRInfoManager();
                HRInfoEntity hrEntity = hrService.GetFirstOrDefault(entity.iHRInfoGuid2);
                hrEntity.iEmployeeDate = entity.iEmployeeDate;
                hrEntity.iResignDate = entity.iResignDate;
                hrEntity.iEmployeeStatus = entity.iEmployeeStatus;
                hrEntity.iResidenceProperty = entity.iResidenceProperty;
                hrEntity.iIsSocialInsurancePaid = entity.iIsSocialInsurancePaid;
                hrService.Update(hrEntity);

                SocialSecurityEntity ssEntity = JsonConvert.DeserializeObject<SocialSecurityEntity>(jsonString, st);
                if (string.IsNullOrEmpty(entity.iGuid))
                {
                    ssEntity.iHRInfoGuid = entity.iHRInfoGuid2;
                    ssEntity.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    ssEntity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Insert(ssEntity);
                }
                else
                {
                    ssEntity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Update(ssEntity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        public string SocialSecurityDetailSaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                SocialSecurityDetailEntity entity = JsonConvert.DeserializeObject<SocialSecurityDetailEntity>(jsonString, st);
                if (string.IsNullOrEmpty(entity.iGuid))
                {
                    //不可能
                }
                else
                {
                    SocialSecurityManager service = new SocialSecurityManager();
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.UpdateDetail(entity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        public string ProvidentFundSaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                ProvidentFundModel entity = JsonConvert.DeserializeObject<ProvidentFundModel>(jsonString, st);
                ProvidentFundManager service = new ProvidentFundManager();
                HRInfoManager hrService = new HRInfoManager();
                HRInfoEntity hrEntity = hrService.GetFirstOrDefault(entity.iHRInfoGuid2);
                hrEntity.iEmployeeDate = entity.iEmployeeDate;
                hrEntity.iResignDate = entity.iResignDate;
                hrEntity.iEmployeeStatus = entity.iEmployeeStatus;
                hrEntity.iResidenceProperty = entity.iResidenceProperty;
                hrEntity.iIsProvidentPaid = entity.iIsProvidentPaid;
                hrEntity.iIsCommercialInsurancePaid = entity.iIsCommercialInsurancePaid;
                hrService.Update(hrEntity);

                ProvidentFundEntity _entity = JsonConvert.DeserializeObject<ProvidentFundEntity>(jsonString, st);
                if (string.IsNullOrEmpty(entity.iGuid))
                {
                    _entity.iHRInfoGuid = entity.iHRInfoGuid2;
                    _entity.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    _entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Insert(_entity);
                }
                else
                {
                    _entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Update(_entity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        public string ProvidentFundDetailSaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                ProvidentFundDetailEntity entity = JsonConvert.DeserializeObject<ProvidentFundDetailEntity>(jsonString, st);
                if (string.IsNullOrEmpty(entity.iGuid))
                {
                    //不可能
                }
                else
                {
                    ProvidentFundManager service = new ProvidentFundManager();
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.UpdateDetail(entity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public JsonResult ImportSocialSecurity()
        {
            try
            {
                var oFile = HttpContext.Request.Files["txt_file"];
                //保存文件
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + oFile.FileName;
                string path = Server.MapPath("~") + "UploadFiles\\" + newFile;
                oFile.SaveAs(path);


                IWorkbook workbook = null;
                if (oFile.FileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(oFile.InputStream);
                else if (oFile.FileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(oFile.InputStream);
                if (workbook == null)
                {
                    return new JsonResult { Data = new { success = false, msg = "不是标准的Excel文件!" } };
                }
                string errorLog = "";
                List<SocialSecurityModel> list = ExcelSheetToEntityList(workbook.GetSheetAt(0), ref errorLog);

                if (string.IsNullOrEmpty(errorLog))
                {
                    MergeToDB(list);
                    return new JsonResult { Data = new { success = true, msg = "success" } };
                }
                else
                {
                    return new JsonResult { Data = new { success = false, msg = errorLog } };

                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, msg = ex.ToString() } };

            }
        }

        public JsonResult ImportProvidentFund()
        {
            try
            {
                var oFile = HttpContext.Request.Files["txt_file"];
                //保存文件
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + oFile.FileName;
                string path = Server.MapPath("~") + "UploadFiles\\" + newFile;
                oFile.SaveAs(path);


                IWorkbook workbook = null;
                if (oFile.FileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(oFile.InputStream);
                else if (oFile.FileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(oFile.InputStream);
                if (workbook == null)
                {
                    return new JsonResult { Data = new { success = false, msg = "不是标准的Excel文件!" } };
                }
                string errorLog = "";
                List<ProvidentFundModel> list = ExcelSheetToProvidentFundList(workbook.GetSheetAt(0), ref errorLog);

                if (string.IsNullOrEmpty(errorLog))
                {
                    MergeProvidentFundToDB(list);
                    return new JsonResult { Data = new { success = true, msg = "success" } };
                }
                else
                {
                    return new JsonResult { Data = new { success = false, msg = errorLog } };

                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, msg = ex.ToString() } };

            }
        }

        private List<SocialSecurityModel> ExcelSheetToEntityList(ISheet sheet, ref string errorLog)
        {
            //需要验证权限，如果是普通用户，不能导入已存在的返回信息。
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            var projects = dm.GetDicByType("项目");
            SocialSecurityManager service = new SocialSecurityManager();

            List<SocialSecurityModel> list = new List<SocialSecurityModel>();

            Dictionary<string, string> keycolumnp = SocialSecurityManager.SocialSecurityViewDic;
            Dictionary<string, int> keycolumns = new Dictionary<string, int>();
            for (int co = 0; co < sheet.GetRow(0).LastCellNum; co++)
            {
                if (keycolumnp.ContainsKey(sheet.GetRow(0).GetCell(co).ToString().Trim()))
                    keycolumns.Add(keycolumnp[sheet.GetRow(0).GetCell(co).ToString().Trim()], co);
            }
            keycolumns.Remove("iTotal");  //总计自动计算处理，不用接受,也不存入数据库,这里去掉以免读取时报错
            //遍历数据行
            for (int i = (sheet.FirstRowNum + 1), len = sheet.LastRowNum + 1; i < len; i++)
            {

                DicEntity currentCompany = null;
                DicEntity currentProject = null;
                SocialSecurityModel en = new SocialSecurityModel();
                try
                {
                    string project = sheet.GetRow(i).GetCell(keycolumns["iItemName"]).ToString().Trim();
                    string company = sheet.GetRow(i).GetCell(keycolumns["iCompany"]).ToString().Trim();
                    string empcode = sheet.GetRow(i).GetCell(keycolumns["iEmpNo"]).ToString().Trim();
                    string idcard = sheet.GetRow(i).GetCell(keycolumns["iIdCard"]).ToString().Trim();
                    if (string.IsNullOrEmpty(empcode))
                    {
                        errorLog += "第【" + (i + 1).ToString() + "】行工号不能为空,临时工用-；";
                    }
                    if (!CheckIDCard18(idcard))
                    {
                        errorLog += "第【" + (i + 1).ToString() + "】行身份证号不合法；";
                    }
                    currentCompany = companies.FirstOrDefault(pj => pj.iValue == company);
                    currentProject = projects.FirstOrDefault(pj => pj.iValue == project);

                    if (currentCompany == null || currentProject == null)
                    {
                        if (currentCompany == null)
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行公司名称不存在；";
                        }
                        if (currentProject == null)
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行项目名称不存在；";
                        }
                        if (SessionHelper.CurrentUser.UserType == "普通用户" && currentProject.iValue != SessionHelper.CurrentUser.CurrentProject)
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行只能导入您当前所在的项目；";
                        }
                    }
                    else
                    {
                        string hrId = service.GetValidSocialSecurityHrId(currentCompany.iKey, empcode, idcard, currentProject.iKey);
                        if (string.IsNullOrEmpty(hrId))
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行在人事里的当前项目中没有给出返费信息；";
                        }
                        else
                        {
                            en = service.GetFirstOrDefault(hrId);
                            if (en.iGuid == null) //en 不可能为空
                            {
                                en.iHRInfoGuid = hrId;
                            }
                            else
                            {
                                if (SessionHelper.CurrentUser.UserType == "普通用户")
                                {
                                    //普通用户过期不能再导入编辑了 mark todo
                                    //errorLog += "第【" + (i + 1).ToString() + "】行已编辑过，您无权限再修改，请联系管理员！；";
                                }
                            }
                        }
                    }
                }
                catch
                {
                    errorLog += "第【" + (i + 1).ToString() + "】行所在公司，工号，身份证号不能有缺失；";
                }
                foreach (var kvp in keycolumns)
                {
                    if (sheet.GetRow(i).GetCell(kvp.Value) == null || sheet.GetRow(i).GetCell(kvp.Value).ToString() == "" || en.GetType().GetProperty(kvp.Key) == null)
                    {
                        //en.GetType().GetProperty(kvp.Key).SetValue(en, null, null);
                        //空的不填写，保持原数据不变  ,没有该属性就忽略
                    }
                    else
                    {
                        object value = null;
                        ICell cell = sheet.GetRow(i).GetCell(kvp.Value);
                        if (cell.CellType == CellType.Blank)
                            value = "";
                        else
                        {
                            string propertyName = en.GetType().GetProperty(kvp.Key).PropertyType.FullName.ToLower();
                            if (cell.CellType == CellType.Numeric && HSSFDateUtil.IsCellDateFormatted(cell))
                            {
                                try
                                {
                                    value = sheet.GetRow(i).GetCell(kvp.Value).DateCellValue;
                                }
                                catch (Exception ex)
                                {
                                    errorLog += "第【" + (i + 1).ToString() + "】行,第【" + (kvp.Value + 1).ToString() + "】列不是标准日期格式；";
                                }
                                if (!propertyName.Contains("datetime") && value != null)
                                {
                                    value = ((DateTime)value).ToString("yyyy-MM-dd");
                                }
                            }
                            else
                            {
                                value = sheet.GetRow(i).GetCell(kvp.Value).ToString().Trim();
                                if (propertyName.Contains("datetime") && value != null)
                                {
                                    DateTime dt = DateTime.Now;
                                    if (DateTime.TryParse(value.ToString(), out dt))
                                    {
                                        value = dt;
                                    }
                                    else
                                    {
                                        errorLog += "第【" + (i + 1).ToString() + "】行,第【" + (kvp.Value + 1).ToString() + "】列不是标准日期格式；";
                                    }
                                }
                                else if (propertyName.Contains("decimal"))
                                {
                                    if (string.IsNullOrEmpty(value.ToString()))
                                        value = null;
                                    else
                                    {
                                        Decimal de = new Decimal();
                                        if (Decimal.TryParse(value.ToString(), out de))
                                        {
                                            value = de;
                                        }
                                        else
                                        {
                                            errorLog += "第【" + (i + 1).ToString() + "】行,第【" + (kvp.Value + 1).ToString() + "】列不是标准数字格式；";
                                        }
                                    }
                                }
                                else if (propertyName.Contains("int"))
                                {
                                    if (string.IsNullOrEmpty(value.ToString()))
                                        value = null;
                                    else
                                    {
                                        int va = new int();
                                        if (int.TryParse(value.ToString(), out va))
                                        {
                                            value = va;
                                        }
                                        else
                                        {
                                            errorLog += "第【" + (i + 1).ToString() + "】行,第【" + (kvp.Value + 1).ToString() + "】列不是标准整数数字格式；";
                                        }
                                    }
                                }
                            }
                        }
                        en.GetType().GetProperty(kvp.Key).SetValue(en, value, null);
                    }
                }



                string[] residencePropertyArray = { "农业户口", "非农业户口" };
                string[] empstaArray = { "在职", "离职" };
                string[] yesnoArray = { "是", "否" };
                Dictionary<string, string[]> checkdic = new Dictionary<string, string[]>();
                checkdic.Add("员工意愿$iEmployeeWilling|是否缴纳$iIsSocialInsurancePaid", yesnoArray);
                checkdic.Add("户籍类型$iResidenceProperty", residencePropertyArray);
                checkdic.Add("员工状态$iEmployeeStatus", empstaArray);


                //是否有效校验

                foreach (var item in checkdic)
                {
                    foreach (var subitem in item.Key.Split('|'))
                    {
                        PropertyInfo property = en.GetType().GetProperty(subitem.Split('$')[1]);
                        if (property.GetValue(en, null) != null && property.GetValue(en, null).ToString() != "" && !item.Value.Contains(property.GetValue(en, null)))
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行" + subitem.Split('$')[0] + "不合法，只能输入【";
                            foreach (var value in item.Value)
                            {
                                errorLog += value + "，";
                            }
                            errorLog = errorLog.TrimEnd('，');
                            errorLog += "】；";
                        }
                    }
                }
                list.Add(en);
            }
            return list;
        }

        private List<ProvidentFundModel> ExcelSheetToProvidentFundList(ISheet sheet, ref string errorLog)
        {
            //需要验证权限，如果是普通用户，不能导入已存在的返回信息。
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            var projects = dm.GetDicByType("项目");
            ProvidentFundManager service = new ProvidentFundManager();

            List<ProvidentFundModel> list = new List<ProvidentFundModel>();

            Dictionary<string, string> keycolumnp = ProvidentFundManager.ProvidentFundViewDic;
            Dictionary<string, int> keycolumns = new Dictionary<string, int>();
            for (int co = 0; co < sheet.GetRow(0).LastCellNum; co++)
            {
                if (keycolumnp.ContainsKey(sheet.GetRow(0).GetCell(co).ToString().Trim()))
                    keycolumns.Add(keycolumnp[sheet.GetRow(0).GetCell(co).ToString().Trim()], co);
            }
            keycolumns.Remove("iTotal");  //总计自动计算处理，不用接受,也不存入数据库,这里去掉以免读取时报错
            //遍历数据行
            for (int i = (sheet.FirstRowNum + 1), len = sheet.LastRowNum + 1; i < len; i++)
            {

                DicEntity currentCompany = null;
                DicEntity currentProject = null;
                ProvidentFundModel en = new ProvidentFundModel();
                try
                {
                    string project = sheet.GetRow(i).GetCell(keycolumns["iItemName"]).ToString().Trim();
                    string company = sheet.GetRow(i).GetCell(keycolumns["iCompany"]).ToString().Trim();
                    string empcode = sheet.GetRow(i).GetCell(keycolumns["iEmpNo"]).ToString().Trim();
                    string idcard = sheet.GetRow(i).GetCell(keycolumns["iIdCard"]).ToString().Trim();
                    if (string.IsNullOrEmpty(empcode))
                    {
                        errorLog += "第【" + (i + 1).ToString() + "】行工号不能为空,临时工用-；";
                    }
                    if (!CheckIDCard18(idcard))
                    {
                        errorLog += "第【" + (i + 1).ToString() + "】行身份证号不合法；";
                    }
                    currentCompany = companies.FirstOrDefault(pj => pj.iValue == company);
                    currentProject = projects.FirstOrDefault(pj => pj.iValue == project);

                    if (currentCompany == null || currentProject == null)
                    {
                        if (currentCompany == null)
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行公司名称不存在；";
                        }
                        if (currentProject == null)
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行项目名称不存在；";
                        }
                        if (SessionHelper.CurrentUser.UserType == "普通用户" && currentProject.iValue != SessionHelper.CurrentUser.CurrentProject)
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行只能导入您当前所在的项目；";
                        }
                    }
                    else
                    {
                        string hrId = service.GetValidProvidentFundHrId(currentCompany.iKey, empcode, idcard, currentProject.iKey);
                        if (string.IsNullOrEmpty(hrId))
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行在人事里的当前项目中没有给出返费信息；";
                        }
                        else
                        {
                            en = service.GetFirstOrDefault(hrId);
                            if (en.iGuid == null) //en 不可能为空
                            {
                                en.iHRInfoGuid = hrId;
                            }
                            else
                            {
                                if (SessionHelper.CurrentUser.UserType == "普通用户")
                                {
                                    //普通用户过期不能再导入编辑了 mark todo
                                    //errorLog += "第【" + (i + 1).ToString() + "】行已编辑过，您无权限再修改，请联系管理员！；";
                                }
                            }
                        }
                    }
                }
                catch
                {
                    errorLog += "第【" + (i + 1).ToString() + "】行所在公司，工号，身份证号不能有缺失；";
                }
                foreach (var kvp in keycolumns)
                {
                    if (sheet.GetRow(i).GetCell(kvp.Value) == null || sheet.GetRow(i).GetCell(kvp.Value).ToString() == "" || en.GetType().GetProperty(kvp.Key) == null)
                    {
                        //en.GetType().GetProperty(kvp.Key).SetValue(en, null, null);
                        //空的不填写，保持原数据不变  ,没有该属性就忽略
                    }
                    else
                    {
                        object value = null;
                        ICell cell = sheet.GetRow(i).GetCell(kvp.Value);
                        if (cell.CellType == CellType.Blank)
                            value = "";
                        else
                        {
                            string propertyName = en.GetType().GetProperty(kvp.Key).PropertyType.FullName.ToLower();
                            if (cell.CellType == CellType.Numeric && HSSFDateUtil.IsCellDateFormatted(cell))
                            {
                                try
                                {
                                    value = sheet.GetRow(i).GetCell(kvp.Value).DateCellValue;
                                }
                                catch (Exception ex)
                                {
                                    errorLog += "第【" + (i + 1).ToString() + "】行,第【" + (kvp.Value + 1).ToString() + "】列不是标准日期格式；";
                                }
                                if (!propertyName.Contains("datetime") && value != null)
                                {
                                    value = ((DateTime)value).ToString("yyyy-MM-dd");
                                }
                            }
                            else
                            {
                                value = sheet.GetRow(i).GetCell(kvp.Value).ToString().Trim();
                                if (propertyName.Contains("datetime") && value != null)
                                {
                                    DateTime dt = DateTime.Now;
                                    if (DateTime.TryParse(value.ToString(), out dt))
                                    {
                                        value = dt;
                                    }
                                    else
                                    {
                                        errorLog += "第【" + (i + 1).ToString() + "】行,第【" + (kvp.Value + 1).ToString() + "】列不是标准日期格式；";
                                    }
                                }
                                else if (propertyName.Contains("decimal"))
                                {
                                    if (string.IsNullOrEmpty(value.ToString()))
                                        value = null;
                                    else
                                    {
                                        Decimal de = new Decimal();
                                        if (Decimal.TryParse(value.ToString(), out de))
                                        {
                                            value = de;
                                        }
                                        else
                                        {
                                            errorLog += "第【" + (i + 1).ToString() + "】行,第【" + (kvp.Value + 1).ToString() + "】列不是标准数字格式；";
                                        }

                                    }

                                }
                                else if (propertyName.Contains("int"))
                                {
                                    if (string.IsNullOrEmpty(value.ToString()))
                                        value = null;
                                    else
                                    {
                                        int va = new int();
                                        if (int.TryParse(value.ToString(), out va))
                                        {
                                            value = va;
                                        }
                                        else
                                        {
                                            errorLog += "第【" + (i + 1).ToString() + "】行,第【" + (kvp.Value + 1).ToString() + "】列不是标准整数数字格式；";
                                        }
                                    }
                                }
                            }
                        }
                        en.GetType().GetProperty(kvp.Key).SetValue(en, value, null);
                    }
                }



                string[] residencePropertyArray = { "农业户口", "非农业户口" };
                string[] empstaArray = { "在职", "离职" };
                string[] yesnoArray = { "是", "否" };
                string[] commericalInsuranceCompanyArray = { "敏慧", "睿琼" };
                Dictionary<string, string[]> checkdic = new Dictionary<string, string[]>();
                checkdic.Add("员工意愿$iEmployeeWilling|是否缴纳$iIsProvidentPaid|是否缴纳商业保险$iIsCommercialInsurancePaid", yesnoArray);
                checkdic.Add("户籍类型$iResidenceProperty", residencePropertyArray);
                checkdic.Add("员工状态$iEmployeeStatus", empstaArray);
                checkdic.Add("商业保险缴纳公司$iCommercialInsurancePaidCompany", commericalInsuranceCompanyArray);


                //是否有效校验

                foreach (var item in checkdic)
                {
                    foreach (var subitem in item.Key.Split('|'))
                    {
                        PropertyInfo property = en.GetType().GetProperty(subitem.Split('$')[1]);
                        if (property.GetValue(en, null) != null && property.GetValue(en, null).ToString() != "" && !item.Value.Contains(property.GetValue(en, null)))
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行" + subitem.Split('$')[0] + "不合法，只能输入【";
                            foreach (var value in item.Value)
                            {
                                errorLog += value + "，";
                            }
                            errorLog = errorLog.TrimEnd('，');
                            errorLog += "】；";
                        }
                    }
                }
                list.Add(en);
            }
            return list;
        }
        private bool CheckIDCard18(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }
        private void MergeToDB(List<SocialSecurityModel> list)
        {
            SocialSecurityManager service = new SocialSecurityManager();
            HRInfoManager hrService = new HRInfoManager();

            foreach (var item in list)
            {
                HRInfoEntity hrEntity = hrService.GetFirstOrDefault(item.iHRInfoGuid2);
                if (item.iEmployeeDate != null)
                    hrEntity.iEmployeeDate = item.iEmployeeDate;
                if (item.iResignDate != null)
                    hrEntity.iResignDate = item.iResignDate;
                if (item.iEmployeeStatus != null)
                    hrEntity.iEmployeeStatus = item.iEmployeeStatus;
                if (item.iResidenceProperty != null)
                    hrEntity.iResidenceProperty = item.iResidenceProperty;
                if (item.iIsSocialInsurancePaid != null)
                    hrEntity.iIsSocialInsurancePaid = item.iIsSocialInsurancePaid;
                hrService.Update(hrEntity);

                SocialSecurityEntity en = new SocialSecurityEntity();
                foreach (System.Reflection.PropertyInfo info in en.GetType().GetProperties())
                {
                    en.GetType().GetProperty(info.Name).SetValue(en, item.GetType().GetProperty(info.Name).GetValue(item), null);
                }
                en.iUpdatedBy = SessionHelper.CurrentUser.UserName;

                if (string.IsNullOrEmpty(item.iGuid))
                {
                    en.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    service.Insert(en);
                }
                else
                {
                    service.Update(en);
                }
            }

        }

        private void MergeProvidentFundToDB(List<ProvidentFundModel> list)
        {
            ProvidentFundManager service = new ProvidentFundManager();
            HRInfoManager hrService = new HRInfoManager();

            foreach (var item in list)
            {
                HRInfoEntity hrEntity = hrService.GetFirstOrDefault(item.iHRInfoGuid2);
                if (item.iEmployeeDate != null)
                    hrEntity.iEmployeeDate = item.iEmployeeDate;
                if (item.iResignDate != null)
                    hrEntity.iResignDate = item.iResignDate;
                if (item.iEmployeeStatus != null)
                    hrEntity.iEmployeeStatus = item.iEmployeeStatus;
                if (item.iResidenceProperty != null)
                    hrEntity.iResidenceProperty = item.iResidenceProperty;
                if (item.iIsProvidentPaid != null)
                    hrEntity.iIsSocialInsurancePaid = item.iIsProvidentPaid;
                if (item.iIsCommercialInsurancePaid != null)
                    hrEntity.iIsCommercialInsurancePaid = item.iIsCommercialInsurancePaid;
                hrService.Update(hrEntity);

                ProvidentFundEntity en = new ProvidentFundEntity();
                foreach (System.Reflection.PropertyInfo info in en.GetType().GetProperties())
                {
                    en.GetType().GetProperty(info.Name).SetValue(en, item.GetType().GetProperty(info.Name).GetValue(item), null);
                }
                en.iUpdatedBy = SessionHelper.CurrentUser.UserName;

                if (string.IsNullOrEmpty(item.iGuid))
                {
                    en.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    service.Insert(en);
                }
                else
                {
                    service.Update(en);
                }
            }

        }


        public void ExportSocialSecurity()
        {
            string path = "社保信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            ExExcel<SocialSecurityModel>(GetExportData(), path, ConvertHelper.DicConvert(SocialSecurityManager.SocialSecurityViewDic));
        }

        public void ExportProvidentFund()
        {
            string path = "公积金信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            ExExcel<ProvidentFundModel>(GetProvidentFundExportData(), path, ConvertHelper.DicConvert(ProvidentFundManager.ProvidentFundViewDic));
        }

        private List<SocialSecurityModel> GetExportData()
        {
            string paraString = Request.Params["searchpara"];
            Dictionary<string, string> paraDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(paraString);
            //查询使用的是bootstrap table 的parameters， 导出一个para反序列出来的参数
            Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
            bizParaDic.Add("search", paraDic["search"]);
            paraDic.Remove("search");
            foreach (var item in paraDic)
            {
                if (item.Key.EndsWith("2"))
                    continue;
                if (paraDic.ContainsKey(item.Key + "2"))
                {
                    bizParaDic.Add("i" + item.Key.Substring(1, item.Key.Length - 1), item.Value + "§" + paraDic[item.Key + "2"]);
                }
                else
                {
                    bizParaDic.Add("i" + item.Key.Substring(1, item.Key.Length - 1), item.Value);
                }
            }
            SocialSecurityManager service = new SocialSecurityManager();
            List<SocialSecurityModel> list = service.GetSearchAll(SessionHelper.CurrentUser.UserType, bizParaDic);
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            var projects = dm.GetDicByType("项目");
            Dictionary<string, string> comDic = companies.ToDictionary(i => i.iKey, i => i.iValue);
            Dictionary<string, string> proDic = projects.ToDictionary(i => i.iKey, i => i.iValue);
            foreach (var item in list)
            {
                item.iCompany = comDic[item.iCompany];
                item.iItemName = proDic[item.iItemName];
            }
            return list;

        }

        private List<ProvidentFundModel> GetProvidentFundExportData()
        {
            string paraString = Request.Params["searchpara"];
            Dictionary<string, string> paraDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(paraString);
            //查询使用的是bootstrap table 的parameters， 导出一个para反序列出来的参数
            Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
            bizParaDic.Add("search", paraDic["search"]);
            paraDic.Remove("search");
            foreach (var item in paraDic)
            {
                if (item.Key.EndsWith("2"))
                    continue;
                if (paraDic.ContainsKey(item.Key + "2"))
                {
                    bizParaDic.Add("i" + item.Key.Substring(1, item.Key.Length - 1), item.Value + "§" + paraDic[item.Key + "2"]);
                }
                else
                {
                    bizParaDic.Add("i" + item.Key.Substring(1, item.Key.Length - 1), item.Value);
                }
            }
            ProvidentFundManager service = new ProvidentFundManager();
            List<ProvidentFundModel> list = service.GetSearchAll(SessionHelper.CurrentUser.UserType, bizParaDic);
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            var projects = dm.GetDicByType("项目");
            Dictionary<string, string> comDic = companies.ToDictionary(i => i.iKey, i => i.iValue);
            Dictionary<string, string> proDic = projects.ToDictionary(i => i.iKey, i => i.iValue);
            foreach (var item in list)
            {
                item.iCompany = comDic[item.iCompany];
                item.iItemName = proDic[item.iItemName];
            }
            return list;

        }


        /// <summary> 
        /// 将一组对象导出成EXCEL 
        /// </summary> 
        /// <typeparam name="T">要导出对象的类型</typeparam> 
        /// <param name="objList">一组对象</param> 
        /// <param name="FileName">导出后的文件名</param> 
        /// <param name="columnInfo">列名信息</param> 
        private void ExExcel<T>(List<T> objList, string FileName, Dictionary<string, string> columnInfo)
        {
            if (columnInfo.Count == 0) { return; }
            if (objList.Count == 0) { return; }

            XSSFWorkbook workbook = new XSSFWorkbook();  //excel 2007版本
            ISheet sheet = workbook.CreateSheet("数据");
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");


            Type myType = objList[0].GetType();
            //根据反射从传递进来的属性名信息得到要显示的属性 
            Dictionary<System.Reflection.PropertyInfo, int> proDic = new Dictionary<System.Reflection.PropertyInfo, int>();
            IRow row = sheet.CreateRow(0);
            int index = 0;
            foreach (string cName in columnInfo.Keys)
            {
                System.Reflection.PropertyInfo p = myType.GetProperty(cName);
                if (p != null)
                {
                    proDic.Add(p, index);
                    ICell cell = row.CreateCell(index, CellType.String);
                    cell.SetCellValue(columnInfo[cName]);
                    index++;
                }
            }
            //如果没有找到可用的属性则结束 
            if (proDic.Count == 0) { return; }
            int rowIndex = 1;
            foreach (T obj in objList)
            {
                IRow rowInner = sheet.CreateRow(rowIndex);
                foreach (var p in proDic)
                {
                    ICell cell = rowInner.CreateCell(p.Value, CellType.String);
                    cell.CellStyle = cellStyle;
                    if (p.Key.PropertyType.FullName.ToLower().Contains("datetime"))
                    {
                        cell.SetCellValue(p.Key.GetValue(obj, null) == null ? "" : ((DateTime)p.Key.GetValue(obj, null)).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        cell.SetCellValue(p.Key.GetValue(obj, null) == null ? "" : p.Key.GetValue(obj, null).ToString());
                    }
                }
                rowIndex++;
            }

            Response.Clear();
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            byte[] data = ms.ToArray();
            Response.BinaryWrite(data);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string encodefileName = System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
            Response.AddHeader("content-disposition", "attachment;  filename=" + encodefileName);
            Response.Flush();
            Response.End();

        }
    }
}
