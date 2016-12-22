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
    public class HRController : BaseController
    {

        public ActionResult BasicIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult AccountIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult PositionIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            ViewBag.Companies = companies;
            var projects = dm.GetAllProjects();
            ViewBag.Projects = projects;
            return View();
        }
        public ActionResult LogIndex()
        {
            return View();
        }
    }

    public class HRAjaxController : Controller
    {
        public void GetAllHRInfo()
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

                string itemName = HttpContext.Request.Params["iItemName"];
                string company = HttpContext.Request.Params["iCompany"];
                string idcard = HttpContext.Request.Params["iIdCard"];
                string name = HttpContext.Request.Params["iName"];
                string empId = HttpContext.Request.Params["iEmpNo"];
                string status = HttpContext.Request.Params["iEmployeeStatus"];
                string employeedate1 = HttpContext.Request.Params["iEmployeeDateFrom"];
                string employeedate2 = HttpContext.Request.Params["iEmployeeDateTo"];

                string contractdeadline1 = HttpContext.Request.Params["iContractDeadLineFrom"];
                string contractdeadline2 = HttpContext.Request.Params["iContractDeadLineTo"];

                string resigndate1 = HttpContext.Request.Params["iResignDateFrom"];
                string resigndate2 = HttpContext.Request.Params["iResignDateTo"];

                string modifyon1 = HttpContext.Request.Params["iModifyOnFrom"];
                string modifyon2 = HttpContext.Request.Params["iModifyOnTo"];

                bizParaDic.Add("iItemName", itemName);
                bizParaDic.Add("iCompany", company);
                bizParaDic.Add("iIdCard", idcard);
                bizParaDic.Add("iName", name);
                bizParaDic.Add("iEmpNo", empId);
                bizParaDic.Add("iEmployeeStatus", status);
                bizParaDic.Add("iEmployeeDate[d]", employeedate1 + "§" + employeedate2);
                bizParaDic.Add("iContractDeadLine[d]", contractdeadline1 + "§" + contractdeadline2);
                bizParaDic.Add("iResignDate[d]", resigndate1 + "§" + resigndate2);
                bizParaDic.Add("iUpdatedOn[d]", modifyon1 + "§" + modifyon2);

                int total = 0;
                HRInfoManager service = new HRInfoManager();
                List<HRInfoEntity> list = service.GetSearch(SessionHelper.CurrentUser.UserType, bizParaDic, sort, order, offset, pageSize, out total);

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
                PageModels<HRInfoEntity> model = new PageModels<HRInfoEntity>();
                model.total = total;
                if (total % pageSize == 0)
                    model.page = total / pageSize;
                else
                    model.page = (total / pageSize) + 1;

                model.rows = list;

                //将查询结果返回  
                HttpContext.Response.Write(jss.Serialize(model));
            }
            catch (Exception e)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(e);
            }
        }

        public JsonResult GetHRInfo(string guid)
        {
            try
            {
                HRInfoManager service = new HRInfoManager();
                HRInfoEntity entity = service.GetFirstOrDefault(guid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = entity }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public string SaveChanges(string jsonString)
        {

            try
            {
                JsonSerializerSettings st = new JsonSerializerSettings();
                st.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                HRInfoEntity entity = JsonConvert.DeserializeObject<HRInfoEntity>(jsonString, st);
                HRInfoManager service = new HRInfoManager();
                if (entity.iGuid == "")
                {
                    if (service.GetUniqueFirstOrDefault(entity.iItemName, entity.iCompany, entity.iEmpNo, entity.iIdCard) != null)
                    {
                        return "已存在相同记录！";
                    }
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

        public void GetHRInfoLog()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            string searchKey = HttpContext.Request.Params["recordid"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

            int total = 0;
            ModifyLogManager service = new ModifyLogManager();
            List<ModifyLogEntity> list = service.GetSearch(searchKey, sort, order, offset, pageSize, out total);

            //给分页实体赋值  
            PageModels<ModifyLogEntity> model = new PageModels<ModifyLogEntity>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = list;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }

        public JsonResult ImportHRInfo()
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
                List<HRInfoEntity> list = ExcelSheetToEntityList(workbook.GetSheetAt(0), ref errorLog);

                if (string.IsNullOrEmpty(errorLog))
                {
                    MergeToDBWithLog(list);
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

        private List<HRInfoEntity> ExcelSheetToEntityList(ISheet sheet, ref string errorLog)
        {
            string[] sexArray = { "男", "女" };
            string[] residencePropertyArray = { "农业户口", "非农业户口" };
            string[] bloodTypeArray = { "A", "B", "AB", "O" };
            string[] marriageArray = { "已婚", "未婚" };
            string[] yesnoArray = { "是", "否" };
            string[] poliArray = { "群众", "团员", "共产党员", "其它" };
            string[] eduArray = { "小学", "初中", "中专", "高中", "大专", "本科", "硕士", "博士" };
            string[] empstaArray = { "在职", "离职" };
            string[] contratypeArray = { "派遣", "外包" };
            string[] resigntypeArray = { "自离", "旷工", "辞职", "辞退", "开除" };
            string[] resignressonArray = { "身体原因", "工作原因", "家族原因", "其他" };
            string[] contractsignArray = { "一次签订", "二次签订", "三次签订", "否" };

            Dictionary<string, string[]> checkdic = new Dictionary<string, string[]>();
            checkdic.Add("性别$iSex", sexArray);
            checkdic.Add("户口性质$iResidenceProperty", residencePropertyArray);
            //checkdic.Add("血型$iBloodType", bloodTypeArray);
            checkdic.Add("婚姻状况$iMariage", marriageArray);
            checkdic.Add("体检$iHealthCheck|是否返费$iIsReturnFee|是否缴纳保险$iIsSocialInsurancePaid|是否缴纳公积金$iIsProvidentPaid|是否缴纳商业保险$iIsCommercialInsurancePaid", yesnoArray);
            checkdic.Add("合同签订情况$iContractSignStatus", contractsignArray); 
            checkdic.Add("政治面貌$iPolitical", poliArray);
            checkdic.Add("文化水平$iEducationLevel", eduArray);
            checkdic.Add("员工状态$iEmployeeStatus", empstaArray);
            checkdic.Add("合同类型$iContractType", contratypeArray);
            checkdic.Add("离职类型$iResignType", resigntypeArray);
            checkdic.Add("离职原因（公司）$iResignReason", resignressonArray);

            DicManager dm = new DicManager();
            var companies = dm.GetAllCompanies();
            var projects = dm.GetAllProjects();
            HRInfoManager service = new HRInfoManager();

            List<HRInfoEntity> list = new List<HRInfoEntity>();

            Dictionary<string, string> keycolumnp = HRInfoManager.hrDic;
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

                HRInfoEntity en = new HRInfoEntity();
                try
                {
                    if (keycolumns.ContainsKey("iGuid"))
                    {
                        string iguid = sheet.GetRow(i).GetCell(keycolumns["iGuid"]).ToString();
                        if (!string.IsNullOrEmpty(iguid))
                        {
                            en = service.GetFirstOrDefault(iguid);
                        }
                    }
                    if (string.IsNullOrEmpty(en.iGuid))
                    {
                        string project = sheet.GetRow(i).GetCell(keycolumns["iItemName"]).ToString().Trim();
                        string company = sheet.GetRow(i).GetCell(keycolumns["iCompany"]).ToString().Trim();
                        string empcode = sheet.GetRow(i).GetCell(keycolumns["iEmpNo"]).ToString().Trim();
                        string idcard = sheet.GetRow(i).GetCell(keycolumns["iIdCard"]).ToString().Trim();
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
                            if (SessionHelper.CurrentUser.UserType == "普通用户" && currentProject.iName != SessionHelper.CurrentUser.CurrentProject)
                            {
                                errorLog += "第【" + (i + 1).ToString() + "】行只能导入您当前所在的项目；";
                            }
                        }
                        else
                        {
                            en = service.GetUniqueFirstOrDefault(currentProject.iGuid, currentCompany.iGuid, empcode, idcard);
                            if (en == null)
                            {
                                en = new HRInfoEntity();
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
                    if (sheet.GetRow(i).GetCell(kvp.Value) == null || sheet.GetRow(i).GetCell(kvp.Value).ToString() == "")
                    {
                        //en.GetType().GetProperty(kvp.Key).SetValue(en, null, null);
                        //空的不填写，保持原数据不变
                    }
                    else
                    {
                        object value = null;
                        ICell cell = sheet.GetRow(i).GetCell(kvp.Value);
                        if (cell.CellType == CellType.Blank)  //此处有公式？
                        {
                            errorLog += "第【" + (i + 1).ToString() + "】行,第【" + (kvp.Value + 1).ToString() + "】列格式有误，不要使用公式；";
                            value = "";
                        }
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
                            en.GetType().GetProperty(kvp.Key).SetValue(en, value, null);
                        }
                    }
                }
                if (currentProject != null)
                {
                    en.iItemName = currentProject.iGuid;
                }
                if (currentCompany != null)
                {
                    en.iCompany = currentCompany.iGuid;
                }

                if (SessionHelper.CurrentUser.UserType == "普通用户" && en.iItemName != SessionHelper.CurrentUser.CurrentProject)
                {
                    errorLog += "第【" + (i + 1).ToString() + "】行项目名称不正确，只能导入当前项目；";
                }
                if (string.IsNullOrEmpty(en.iEmpNo))
                {
                    errorLog += "第【" + (i + 1).ToString() + "】行工号不能为空,临时工用-；";
                }
                if (string.IsNullOrEmpty(en.iName))
                {
                    errorLog += "第【" + (i + 1).ToString() + "】行姓名不能为空；";
                }
                if (!CheckIDCard18(en.iIdCard))
                {
                    errorLog += "第【" + (i + 1).ToString() + "】行身份证号不合法；";
                }
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

        private void MergeToDBWithLog(List<HRInfoEntity> list)
        {
            HRInfoManager service = new HRInfoManager();
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

        /// <summary>  
        /// 18位身份证号码验证  
        /// </summary>  
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

        public void ExportBasic()
        {
            string path = "人事基本信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            ExExcel<HRInfoEntity>(GetExportData(), path, ConvertHelper.DicConvert(HRInfoManager.hrBasicDic));
        }
        public void ExportAccount()
        {
            string path = "人事账户信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            ExExcel<HRInfoEntity>(GetExportData(), path, ConvertHelper.DicConvert(HRInfoManager.hrAccountDic));
        }
        public void ExportPosition()
        {
            string path = "人事职位信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            ExExcel<HRInfoEntity>(GetExportData(), path, ConvertHelper.DicConvert(HRInfoManager.hrPositionDic));
        }

        public void ExportFull()
        {
            string path = "人事完整信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            ExExcel<HRInfoEntity>(GetExportData(), path, ConvertHelper.DicConvert(HRInfoManager.hrFullDic));
        }

        private List<HRInfoEntity> GetExportData()
        {
            try
            {
                string paraString = Request.Params["searchpara"];
                Dictionary<string, string> paraDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(paraString);
                paraDic.Add("iEmployeeDate[d]", paraDic["iEmployeeDateFrom"] + "§" + paraDic["iEmployeeDateTo"]);
                paraDic.Add("iContractDeadLine[d]", paraDic["iContractDeadLineFrom"] + "§" + paraDic["iContractDeadLineTo"]);
                paraDic.Add("iResignDate[d]", paraDic["iResignDateFrom"] + "§" + paraDic["iResignDateTo"]);
                paraDic.Add("iUpdatedOn[d]", paraDic["iModifyOnFrom"] + "§" + paraDic["iModifyOnTo"]);
                paraDic.Remove("iEmployeeDateFrom");
                paraDic.Remove("iEmployeeDateTo");
                paraDic.Remove("iContractDeadLineFrom");
                paraDic.Remove("iContractDeadLineTo");
                paraDic.Remove("iResignDateFrom");
                paraDic.Remove("iResignDateTo");
                paraDic.Remove("iModifyOnFrom");
                paraDic.Remove("iModifyOnTo");

                HRInfoManager service = new HRInfoManager();
                List<HRInfoEntity> list = service.GetSearchAll(SessionHelper.CurrentUser.UserType, paraDic);
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
            catch(Exception e)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(e);
                return null;
            }

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
