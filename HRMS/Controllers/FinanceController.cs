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
using HRMS.Data.Entity;
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
    public class FinanceController : BaseController
    {

        public ActionResult SalaryIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult JournalIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult ReturnFeeIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }
    }

    public class FinanceAjaxController : Controller
    {
        public void GetAllReturnFee()
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
                string editType = HttpContext.Request.Params["sEditType"];

                Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
                bizParaDic.Add("search", searchKey);
                bizParaDic.Add("editType", editType);
                Dictionary<string, string> bizParaDicTemp = new Dictionary<string, string>();

                foreach (string para in HttpContext.Request.Params.Keys)
                {
                    if (para.StartsWith("s") && (ReturnFeeManager.ReturnFeeDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && ReturnFeeManager.ReturnFeeDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
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
                ReturnFeeManager service = new ReturnFeeManager();
                List<ReturnFeeModel> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);

                DicManager dm = new DicManager();
                var companies = dm.GetAllCompanies();
                var projects = dm.GetAllProjects();
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iGuid, i => i.iName);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iGuid, i => i.iName);
                foreach (var item in list)
                {
                    item.iCompany = comDic[item.iCompany];
                    item.iItemName = proDic[item.iItemName];
                }

                //给分页实体赋值  
                PageModels<ReturnFeeModel> model = new PageModels<ReturnFeeModel>();
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

        public JsonResult GetReturnFee(string hrguid)
        {
            try
            {
                ReturnFeeManager service = new ReturnFeeManager();
                ReturnFeeModel entity = service.GetFirstOrDefault(hrguid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = entity }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public string SaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                ReturnFeeEntity entity = JsonConvert.DeserializeObject<ReturnFeeEntity>(jsonString, st);
                ReturnFeeManager service = new ReturnFeeManager();
                if (string.IsNullOrEmpty(entity.iGuid))
                {
                    entity.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Insert(entity);
                }
                else
                {
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Update(entity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public JsonResult ImportReturnFee()
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
                List<ReturnFeeEntity> list = ExcelSheetToEntityList(workbook.GetSheetAt(0), ref errorLog);

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

        private List<ReturnFeeEntity> ExcelSheetToEntityList(ISheet sheet, ref string errorLog)
        {
            //需要验证权限，如果是普通用户，不能导入已存在的返回信息。
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            var projects = dm.GetAllProjects();
            ReturnFeeManager service = new ReturnFeeManager();

            List<ReturnFeeEntity> list = new List<ReturnFeeEntity>();

            Dictionary<string, string> keycolumnp = ReturnFeeManager.ReturnFeeDic;
            Dictionary<string, int> keycolumns = new Dictionary<string, int>();
            for (int co = 0; co < sheet.GetRow(0).LastCellNum; co++)
            {
                if (keycolumnp.ContainsKey(sheet.GetRow(0).GetCell(co).ToString().Trim()))
                    keycolumns.Add(keycolumnp[sheet.GetRow(0).GetCell(co).ToString().Trim()], co);
            }
            //遍历数据行
            for (int i = (sheet.FirstRowNum + 1), len = sheet.LastRowNum + 1; i < len; i++)
            {

                CompanyEntity currentCompany = null;
                ProjectEntity currentProject = null;
                ReturnFeeEntity en = new ReturnFeeEntity();
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
                    currentCompany = companies.FirstOrDefault(pj => pj.iName == company);
                    currentProject = projects.FirstOrDefault(pj => pj.iName == project);

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
                        if (SessionHelper.CurrentUser.UserType == "普通用户" && currentProject.iGuid != SessionHelper.CurrentUser.CurrentProject)
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行只能导入您当前所在的项目；";
                        }
                    }
                    else
                    {
                        string hrId = service.GetValidReturnFeeHrId(currentCompany.iGuid, empcode, idcard, currentProject.iGuid);
                        if (string.IsNullOrEmpty(hrId))
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行在人事里的当前项目中没有给出返费信息；";
                        }
                        else
                        {
                            en = service.FirstOrDefault(hrId);
                            if (en == null)
                            {
                                en = new ReturnFeeEntity();
                                en.iHRInfoGuid = hrId;
                            }
                            else
                            {
                                if (SessionHelper.CurrentUser.UserType == "普通用户")
                                {
                                    errorLog += "第【" + (i + 1).ToString() + "】行已编辑过，您无权限再修改，请联系管理员！；";
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
                            }
                        }
                        en.GetType().GetProperty(kvp.Key).SetValue(en, value, null);
                    }
                }


                string[] paidArray = { "已付", "未付" };
                Dictionary<string, string[]> checkdic = new Dictionary<string, string[]>();
                checkdic.Add("一级付款情况$iFirstReturnFeePayment", paidArray);
                checkdic.Add("二级付款情况$iSecondReturnFeePayment", paidArray);
                checkdic.Add("三级付款情况$iThirdReturnFeePayment", paidArray);
                checkdic.Add("四级付款情况$iFourthReturnFeePayment", paidArray);
                checkdic.Add("五级付款情况$iFifthReturnFeePayment", paidArray);


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
        private void MergeToDB(List<ReturnFeeEntity> list)
        {
            ReturnFeeManager service = new ReturnFeeManager();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.iGuid))
                {
                    item.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    item.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Insert(item);
                }
                else
                {
                    item.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Update(item);
                }
            }

        }


        public void ExportReturnFee()
        {
            string path = "返费信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            ExExcel<ReturnFeeModel>(GetExportData(), path, ConvertHelper.DicConvert(ReturnFeeManager.ReturnFeeDic));
        }

        private List<ReturnFeeModel> GetExportData()
        {
            string paraString = Request.Params["searchpara"];
            Dictionary<string, string> paraDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(paraString);

            Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
            bizParaDic.Add("search", paraDic["search"]);
            paraDic.Remove("search");
            bizParaDic.Add("editType", paraDic["sEditType"]);
            paraDic.Remove("sEditType");

            foreach (var item in paraDic)
            {
                if (item.Key.EndsWith("2"))
                    continue;
                if (paraDic.ContainsKey(item.Key + "2"))
                {
                    bizParaDic.Add("i" + item.Key.Substring(1, item.Key.Length - 1) + "[d]", item.Value + "§" + paraDic[item.Key + "2"]);
                }
                else
                {
                    bizParaDic.Add("i" + item.Key.Substring(1, item.Key.Length - 1), item.Value);
                }
            }

            ReturnFeeManager service = new ReturnFeeManager();
            List<ReturnFeeModel> list = service.GetSearchAll(SessionHelper.CurrentUser.UserType, bizParaDic);

            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            var projects = dm.GetAllProjects();
            Dictionary<string, string> comDic = companies.ToDictionary(i => i.iGuid, i => i.iName);
            Dictionary<string, string> proDic = projects.ToDictionary(i => i.iGuid, i => i.iName);
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
            if (columnInfo.ContainsKey("iGuid"))  //不导出标识
            {
                columnInfo.Remove("iGuid");
            }
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


        #region Journal
        public void GetAllJournal()
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
                    if (para.StartsWith("s") && (JournalManager.JournalDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && JournalManager.JournalDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
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
                JournalManager service = new JournalManager();
                List<JournalEntity> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);
                decimal? sum = list.Last().iAmount;
                list.RemoveAt(list.Count - 1);
                DicManager dm = new DicManager();
                var companies = dm.GetAllCompanies();
                var projects = dm.GetAllProjects();
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iGuid, i => i.iName);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iGuid, i => i.iName);
                foreach (var item in list)
                {
                    item.iCompanyId = comDic[item.iCompanyId];
                    item.iProjectId = proDic[item.iProjectId];
                }

                //给分页实体赋值  
                PageModels2<JournalEntity> model = new PageModels2<JournalEntity>();
                model.total = total;
                if (total % pageSize == 0)
                    model.page = total / pageSize;
                else
                    model.page = (total / pageSize) + 1;

                model.rows = list;
                model.sum = sum ?? 0;

                //将查询结果返回  
                HttpContext.Response.Write(jss.Serialize(model));
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
            }
        }

        public void GetFlowJournal()
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
                    if (para.StartsWith("s") && (JournalManager.JournalDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && JournalManager.JournalDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
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
                JournalManager service = new JournalManager();
                List<JournalEntity> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);
                decimal? sum = list.Last().iAmount;
                list.RemoveAt(list.Count - 1);
                DicManager dm = new DicManager();
                var companies = dm.GetAllCompanies();
                var projects = dm.GetAllProjects();
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iGuid, i => i.iName);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iGuid, i => i.iName);
                List<Journal4Flow> result = new List<Journal4Flow>();
                foreach (var item in list)
                {
                    Journal4Flow jf = new Journal4Flow();
                    jf.iAmount = item.iAmount.ToString();
                    jf.iApplicant = item.iApplicant;
                    jf.iCompanyId = item.iCompanyId;
                    jf.iCompanyName = comDic[item.iCompanyId];
                    jf.iProjectId = item.iProjectId;
                    jf.iProjectName = proDic[item.iProjectId];
                    jf.iDate = ((DateTime)item.iDate).ToString("yyyy-MM-dd");
                    jf.iEvent = item.iEvent;
                    jf.iGuid = item.iGuid;
                    jf.iNote = item.iNote;
                    jf.iType = item.iType;
                    result.Add(jf);
                }
                //给分页实体赋值  
                PageModels2<Journal4Flow> model = new PageModels2<Journal4Flow>();
                model.total = total;
                if (total % pageSize == 0)
                    model.page = total / pageSize;
                else
                    model.page = (total / pageSize) + 1;

                model.rows = result;
                model.sum = sum ?? 0;

                //将查询结果返回  
                HttpContext.Response.Write(jss.Serialize(model));
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
            }
        }
        public JsonResult GetJournal(string iGuid)
        {
            try
            {
                JournalManager service = new JournalManager();
                JournalEntity entity = service.FirstOrDefault(iGuid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = entity }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public string JournalSaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                JournalEntity entity = JsonConvert.DeserializeObject<JournalEntity>(jsonString, st);
                JournalManager service = new JournalManager();
                if (string.IsNullOrEmpty(entity.iGuid))
                {
                    entity.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Insert(entity);
                }
                else
                {
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Update(entity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        #endregion-

        #region  //工资信息
        public void GetAllSalary()
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
                    if (para.StartsWith("s") && (SalaryManager.SalaryDic.ContainsValue("i" + para.Substring(1, para.Length - 1)) || (para.Length > 2 && SalaryManager.SalaryDic.ContainsValue("i" + para.Substring(1, para.Length - 2)))))
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
                        bizParaDic.Add(item.Key + "[d]", (string.IsNullOrEmpty(item.Value) ? "" : (item.Value + "-01")) + "§" + ((string.IsNullOrEmpty(bizParaDicTemp[item.Key + "2"])) ? "" : (bizParaDicTemp[item.Key + "2"] + "-01")));
                    }
                    else
                    {
                        bizParaDic.Add(item.Key, item.Value);
                    }
                }

                int total = 0;
                SalaryManager service = new SalaryManager();
                List<SalaryEntity> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);
                DicManager dm = new DicManager();
                var companies = dm.GetAllCompanies();
                var projects = dm.GetAllProjects();
                Dictionary<string, string> comDic = companies.ToDictionary(i => i.iGuid, i => i.iName);
                Dictionary<string, string> proDic = projects.ToDictionary(i => i.iGuid, i => i.iName);
                foreach (var item in list)
                {
                    item.iCompanyId = comDic[item.iCompanyId];
                    item.iProjectId = proDic[item.iProjectId];
                }

                //给分页实体赋值  
                PageModels<SalaryEntity> model = new PageModels<SalaryEntity>();
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

        public JsonResult UploadSalary()
        {
            try
            {
                var oFile = HttpContext.Request.Files["txt_file"];
                //保存文件
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + oFile.FileName;
                string path = Server.MapPath("~") + "UploadSalary\\" + newFile;
                oFile.SaveAs(path);

                string errorLog = "";

                if (string.IsNullOrEmpty(errorLog))
                {
                    return new JsonResult { Data = new { success = true, msg = newFile } };
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

        public string SalarySaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                SalaryEntity entity = JsonConvert.DeserializeObject<SalaryEntity>(jsonString, st);
                SalaryManager service = new SalaryManager();
                if (string.IsNullOrEmpty(entity.iGuid))
                {
                    entity.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    service.Insert(entity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string SalaryRemove(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                SalaryEntity entity = JsonConvert.DeserializeObject<SalaryEntity>(jsonString, st);
                SalaryManager service = new SalaryManager();
                if (!string.IsNullOrEmpty(entity.iGuid))
                {
                    entity = service.FirstOrDefault(entity.iGuid);
                    entity.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    entity.iIsDeleted = 1;
                    service.Update(entity);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        #endregion
    }

    public class Journal4Flow
    {
        public string iGuid { get; set; } //惟一标识
        public string iCompanyId { get; set; } //公司
        public string iProjectId { get; set; } //项目

        public string iCompanyName { get; set; } //公司
        public string iProjectName { get; set; } //项目
        public string iDate { get; set; } //日期      
        public string iEvent { get; set; } //事项
        public string iType { get; set; } //科目
        public string iApplicant { get; set; } //提报人
        public string iAmount { get; set; } //金额
        public string iNote { get; set; } //备注 

    }
}
