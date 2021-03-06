﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HRMS.Data.Entity;
using HRMS.Data;
using HRMS.Data.Manager;
using HRMS.WEB.Models;
using HRMS.WEB.Utils;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HRMS.Controllers
{
    public class ManageController : BaseController
    {
        public ActionResult UserIndex()
        {
            ManageAjaxController ma = new ManageAjaxController();
            var contents = ma.GetStandardMenuTree(false);
            ViewBag.treeNodes = JsonConvert.SerializeObject(contents);

            var companyProjectNodes = ma.GetCompanyProjectTree();
            ViewBag.companyProjectNodes = JsonConvert.SerializeObject(companyProjectNodes);
            return View();
        }

        public ActionResult UsersIndex()
        {
            return View();
        }
        public ActionResult DicIndex()
        {
            DicManager dm = new DicManager();
            var companies = dm.GetDicByType("公司");
            ViewBag.Companies = companies;
            return View();
        }

        public ActionResult PeopleIndex()
        {
            return View();
        }

        public ActionResult MenuIndex()
        {
            ManageAjaxController ma = new ManageAjaxController();

            var contents = ma.GetStandardMenuTree(true);
            ViewBag.treeNodes = JsonConvert.SerializeObject(contents);
            return View();
        }

        public ActionResult CompanyIndex()
        {
            return View();
        }

        public ActionResult ProjectIndex()
        {
            DicManager service = new DicManager();
            List<CompanyEntity> companies = service.GetAllValidCompanies();
            ViewBag.Companies = companies;
            return View();
        }

        public ActionResult CompanyFlowUsersSetting()
        {
            DicManager service = new DicManager();
            List<CompanyEntity> companies = service.GetAllValidCompanies();
            ViewBag.Companies = companies;
            return View();
        }


        public ActionResult CompanyRoleIndex()
        {
            DicManager service = new DicManager();
            List<CompanyEntity> companies = service.GetAllValidCompanies();
            ViewBag.Companies = companies;
            return View();
        }
    }
    public class ManageAjaxController : Controller
    {
        /*
        #region 用户方法
        public void GetAllUsers_old()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            string searchKey = HttpContext.Request.Params["search"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

            int total = 0;
            UserManager um = new UserManager();
            List<UserEntity> list = um.GetSearch(searchKey, sort, order, offset, pageSize, out total);

            DicManager dm = new DicManager();
            List<DicEntity> companyDicE = dm.GetDicByType("公司");
            Dictionary<string, string> companyDic = new Dictionary<string, string>();
            foreach (var item in companyDicE)
            {
                companyDic.Add(item.iKey, item.iValue);
            }
            List<UserViewModel> listView = new List<UserViewModel>();
            foreach (var item in list)
            {
                listView.Add(new UserViewModel { iCompanyCode = item.iCompanyCode, iCompanyName = item.iCompanyCode, iEmployeeCodeId = item.iEmployeeCodeId, iPassWord = item.iPassWord, iUserName = item.iUserName, iUserType = item.iUserType, iUpdatedOn = item.iUpdatedOn.ToString("yyyyMMdd HH:mm") });
            }

            //给分页实体赋值  
            PageModels<UserViewModel> model = new PageModels<UserViewModel>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = listView;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }

        public string UsersSaveChanges_old(string jsonString, string action)
        {

            try
            {
                UserEntity ue = JsonConvert.DeserializeObject<UserEntity>(jsonString);
                UserManager pm = new UserManager();
                if (action == "add")
                {
                    ue.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    ue.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    pm.Insert(ue);
                }
                else
                {
                    UserEntity ueOld = pm.GetUser(ue.iEmployeeCodeId);
                    ue.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    ue.iCreatedBy = ueOld.iCreatedBy;
                    ue.iCreatedOn = ueOld.iCreatedOn;
                    if (ue.iIsDeleted != 1)
                    {
                        ue.iStatus = 1;
                    }
                    pm.Update(ue);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        #endregion
        */
        #region 新用户方法
        public void GetAllUsers()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            string searchKey = HttpContext.Request.Params["search"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

            int total = 0;
            UserManager um = new UserManager();
            List<UserEntity> list = um.GetSearch(searchKey, sort, order, offset, pageSize, out total);

            //给分页实体赋值  
            PageModels<UserEntity> model = new PageModels<UserEntity>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = list;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }

        public string UserSaveChanges(string jsonString)
        {

            try
            {
                UserEntity ue = JsonConvert.DeserializeObject<UserEntity>(jsonString);
                UserManager service = new UserManager();
                UserEntity ueOld = service.GetUser(ue.iEmployeeCodeId);
                if (ueOld == null)
                {
                    ue.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    ue.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    ue.iEmployeeCodeId = ue.iEmployeeCodeId.Trim();  //工号不要出现空格
                    service.Insert(ue);
                }
                else
                {
                    ue.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    ue.iEmployeeCodeId = ue.iEmployeeCodeId.Trim();
                    service.Update(ue);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string CheckUserValid(string iEmployeeCodeId)
        {
            UserManager service = new UserManager();
            if (service.CheckUserValid(iEmployeeCodeId))
            {
                return "valid";
            }
            return "invalid";
        }

        #endregion

        #region 字典方法

        public void GetAllDics()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            string searchKey = HttpContext.Request.Params["search"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);
            string iCompanyCode = HttpContext.Request.Params["iCompanyCode"];
            string iType = HttpContext.Request.Params["iType"];
            Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
            bizParaDic.Add("search", searchKey);
            bizParaDic.Add("iCompanyCode", iCompanyCode);
            bizParaDic.Add("iType", iType);

            int total = 0;
            DicManager dm = new DicManager();
            List<DicEntity> list = dm.GetSearch(bizParaDic, sort, order, offset, pageSize, out total);
            List<DicViewModel> listView = new List<DicViewModel>();
            var companies = dm.GetDicByType("公司");
            foreach (var item in list)
            {
                string companiesNameString = "";
                if (item.iCompanyCode != null)
                {
                    string[] companiesArray = item.iCompanyCode.Trim('[', ']').Split(',');
                    foreach (string com in companiesArray)
                    {
                        var currentCompany = companies.FirstOrDefault(i => i.iKey == com.Trim('\''));
                        if (currentCompany != null)
                        {
                            companiesNameString += currentCompany.iValue + ",";
                        }
                    }
                    companiesNameString = companiesNameString.Trim(',');
                }

                listView.Add(new DicViewModel { iId = item.iId, iKey = item.iKey, iValue = item.iValue, iType = item.iType, iCompanyCode = item.iCompanyCode, iCompanyName = companiesNameString, iUpdatedOn = item.iUpdatedOn.ToString("yyyyMMdd HH:mm") });
            }

            //给分页实体赋值  
            PageModels<DicViewModel> model = new PageModels<DicViewModel>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = listView;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }

        public string DicsSaveChanges(string jsonString, string action)
        {

            try
            {
                DicEntity ue = JsonConvert.DeserializeObject<DicEntity>(jsonString);
                DicManager dm = new DicManager();
                if (action == "add")
                {
                    ue.iId = Guid.NewGuid().ToString();
                    ue.iKey = ue.iValue;
                    ue.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    ue.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    dm.Insert(ue);
                }
                else
                {
                    DicEntity ueOld = dm.GetDic(ue.iId);
                    ue.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    ue.iKey = ueOld.iKey;
                    ue.iCreatedBy = ueOld.iCreatedBy;
                    ue.iCreatedOn = ueOld.iCreatedOn;
                    if (ue.iIsDeleted != 1)
                    {
                        ue.iStatus = 1;
                    }
                    dm.Update(ue);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        #endregion

        #region 公用方法
        public ActionResult ChangeProject(string newCompany, string newProject)
        {
            if (SessionHelper.CurrentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                LoginUserInfo userinfo = SessionHelper.CurrentUser;
                userinfo.CurrentCompany = newCompany;
                userinfo.CurrentProject = newProject;
                Session[SessionHelper.CurrentUserKey] = userinfo;
                return Content("success");
            }
        }

        public JsonResult GetCompanyProjects(string companyCode)
        {
            try
            {
                string sql = "select distinct iKey, iValue from [SysDic] where iType='项目' and iStatus = 1 and iIsDeleted=0 and icompanycode like '%''" + companyCode + "''%' ";
                DataSet ds = DbHelperSQL.Query(sql);
                Dictionary<string, string> projectsDic = new Dictionary<string, string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    projectsDic.Add(dr[0].ToString(), dr[1].ToString());
                }
                return new JsonResult { Data = new { success = true, msg = "msg", data = projectsDic }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public JsonResult GetAuthorisedProjectDic(string companyCode)
        {
            try
            {
                Dictionary<string, string> projectsDic = new Dictionary<string, string>();
                DicManager service = new DicManager();
                projectsDic = service.GetAuthorisedProjectDic(SessionHelper.CurrentUser.UserId, SessionHelper.CurrentUser.UserType, companyCode);

                return new JsonResult { Data = new { success = true, msg = "msg", data = projectsDic }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        #endregion

        #region 人力方法
        public void GetAllPeople()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            string searchKey = HttpContext.Request.Params["search"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);

            int total = 0;
            LaborInfoManager lim = new LaborInfoManager();
            List<LaborInfoEntity> list = lim.GetSearch(SessionHelper.CurrentUser.CurrentCompany, searchKey, sort, order, offset, pageSize, out total);

            //给分页实体赋值  
            PageModels<LaborInfoEntity> model = new PageModels<LaborInfoEntity>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = list;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }


        public JsonResult ImportPeople()
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
                DataTable dt = ExcelSheetToDataTable(workbook.GetSheetAt(0));

                WriteDataTableToServer(dt, "LaborInfo", ConfigurationManager.ConnectionStrings["HRMSDBConnectionString"].ConnectionString);

                return new JsonResult { Data = new { success = true, msg = "msg" } };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, msg = ex.ToString() } };

            }
        }

        public void ExportPeople()
        {
            try
            {
                Dictionary<string, string> keycolumnMap = new Dictionary<string, string>();
                keycolumnMap.Add("rkey", "中文");
                string idata = Request.Params["idata"];
                if (idata == null)
                    return;
                else
                {
                    //反序列化成字典
                    List<Dictionary<string, string>> datadic = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(idata);

                    //生成数据表
                    DataTable datatable = new DataTable();
                    foreach (Dictionary<string, string> cols in datadic)
                    {
                        //为datatable添加列  
                        if (datatable.Columns.Count == 0)
                        {
                            foreach (string key in cols.Keys)
                            {
                                datatable.Columns.Add(key);
                            }
                        }
                        DataRow dr = datatable.NewRow();
                        //为行中的每一列列赋值  
                        foreach (string keyname in cols.Keys)
                        {
                            dr[keyname] = cols[keyname];
                        }
                        datatable.Rows.Add(dr);
                    }

                    HSSFWorkbook workbook = new HSSFWorkbook();  //excel 2003版本
                    ISheet sheet = workbook.CreateSheet("数据");
                    IRow row = sheet.CreateRow(0);
                    int index = 0;
                    foreach (DataColumn item in datatable.Columns)
                    {
                        ICell cell = row.CreateCell(index, CellType.String);
                        if (keycolumnMap.ContainsKey(item.ColumnName))
                            cell.SetCellValue(keycolumnMap[item.ColumnName]);
                        else
                            cell.SetCellValue(item.ColumnName);
                        index++;
                    }
                    for (int i = 0; i < datatable.Rows.Count; i++)
                    {
                        IRow rowInner = sheet.CreateRow(i + 1);
                        int indexInner = 0;
                        foreach (DataColumn item in datatable.Columns)
                        {
                            ICell cell = rowInner.CreateCell(indexInner, CellType.String);
                            cell.SetCellValue(datatable.Rows[i][item.ColumnName].ToString());
                            indexInner++;
                        }
                    }
                    string path = "导出数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                    Response.Clear();
                    MemoryStream ms = new MemoryStream();
                    workbook.Write(ms);
                    byte[] data = ms.ToArray();
                    Response.BinaryWrite(data);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string encodefileName = System.Web.HttpUtility.UrlEncode(path, System.Text.Encoding.UTF8);
                    Response.AddHeader("content-disposition", "attachment;  filename=" + encodefileName);
                    Response.Flush();
                    Response.End();

                }
            }
            catch
            {
                Response.Write("<script>alert('导出错误！');window.opener=null;window.open('','_self');window.close();</script>");
            }
        }

        /// <summary>
        /// 把Sheet中的数据转换为DataTable
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private DataTable ExcelSheetToDataTable(ISheet sheet)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("序号", "iNo");
            dic.Add("项目名称", "iItemName");
            dic.Add("所在公司", "iCompany");
            dic.Add("姓名", "iName");
            dic.Add("工号", "iEmpNo");
            dic.Add("电话", "iPhone");
            dic.Add("紧急联系人", "iEmeContact");
            dic.Add("紧急联系人电话", "iEmeContactPhone");
            dic.Add("性别", "iSex");
            dic.Add("出生日期", "iBirthday");
            dic.Add("户籍", "iRegistry");
            dic.Add("民族", "iNation");
            dic.Add("身份证号", "iIdCard");
            dic.Add("户口性质", "iResidenceProperty");
            dic.Add("户籍地址", "iRegistryAddress");
            dic.Add("签发机关", "iIssuedBy");
            dic.Add("身份证有效期", "iIdCardValidate");
            dic.Add("现住地", "iLivedIn");
            dic.Add("员工状态", "iEmployeeStatus");
            dic.Add("入职时间", "iEmployeeDate");
            dic.Add("转正时间", "iPositiveDate");
            dic.Add("到期日期", "iDeadLine");
            dic.Add("离职类型", "iResignType");
            dic.Add("离职日期", "iResignDate");
            dic.Add("工资截止日期", "iPayDeadLine");
            dic.Add("离职原因（公司）", "iResignReason");
            dic.Add("工资开户行", "iWageBank");
            dic.Add("工资帐号", "iWageAccount");  //工资账号
            dic.Add("所属一级部门", "iFirstDep");
            dic.Add("所属二级部门", "iSecondDep");
            dic.Add("所属三级部门", "iThirdDep");
            dic.Add("所属四级部门", "iFourthDep");
            dic.Add("所属五级部门", "iFifthDep");
            dic.Add("所在工作地", "iWorkPlace");
            dic.Add("人员类别", "iEmpType");
            dic.Add("岗位", "iPosition");
            dic.Add("管理层级", "iManageLevel");
            dic.Add("所在项目组", "iProjectGroup");
            dic.Add("司龄", "iCompanyWorkYear");
            dic.Add("婚姻状况", "iMariage");
            dic.Add("年龄", "iAge");
            dic.Add("健康", "iHealth");
            dic.Add("政治面貌", "iPolitical");
            dic.Add("通讯地址", "iPostalAddress");
            dic.Add("工作时间", "iWorkDate");
            dic.Add("工作年限", "iWorkYearSpan");
            dic.Add("文化水平", "iEducationLevel");
            dic.Add("毕业学校", "iGraduatedSchool");
            dic.Add("专业", "iMajor");
            dic.Add("毕业时间", "iGraduatedDate");
            dic.Add("所学外语", "iForeignLanguage");
            dic.Add("外语等级", "iForeignLanguageLevel");
            dic.Add("计算机掌握程度", "iComputerMastery");
            dic.Add("计算机等级", "iComputerLevel");
            dic.Add("所获证书", "iCertificate");
            dic.Add("教育简历", "iEducationResume");
            dic.Add("特长", "iSpecialty");
            dic.Add("爱好", "iHobby");
            dic.Add("邮箱", "iEmail");
            dic.Add("联系地址", "iContactAddress");
            dic.Add("联系电话", "iContactTel");
            dic.Add("邮编", "iPostCode");
            dic.Add("身高", "iHeight");
            dic.Add("体重", "iWeight");
            dic.Add("血型", "iBloodType");
            dic.Add("工作要求", "iJobRequire");
            dic.Add("社会关系", "iSocietyRelation");
            dic.Add("工作经历", "iWorkExperience");
            dic.Add("学习经历", "iStudyExperience");
            dic.Add("合同类型", "iContractType");
            dic.Add("参加基金状态", "iFundStatus");
            dic.Add("合同/协议期限", "iContractTerm");
            dic.Add("档案位置", "iFileLocation");
            dic.Add("社保账号", "iSocialSecurityAccount");
            dic.Add("社保公司缴纳金额", "iSocialSecurityCompanyPay");
            dic.Add("社保个人缴纳金额", "iSocialSecurityPersonalPay");
            dic.Add("社保缴纳总金额", "iSocialSecurityAmount");
            dic.Add("公积金账号", "iProvidentAccount");
            dic.Add("公积金公司缴费金额", "iProvidentCompanyPay");
            dic.Add("公积金个人缴费金额", "iProvidentPersonalPay");
            dic.Add("劳务公司", "iLaborCompany");
            dic.Add("一级返费金额", "iFirstReturn");
            dic.Add("一级返费日期", "iFirstReturnDate");
            dic.Add("一级返费付款情况", "iFirstReturnPayStatus");
            dic.Add("二级返费", "iSecondReturn");
            dic.Add("二级返费日期", "iSecondReturnDate");
            dic.Add("二级返费付款情况", "iSecondReturnPayStatus");
            dic.Add("三级返费", "iThirdReturn");
            dic.Add("三级返费到期日期", "iThirdReturnDate");
            dic.Add("三级返费付款情况", "iThirdReturnPayStatus");
            dic.Add("劳务所银行支行", "iLaborBank");
            dic.Add("劳务所账号", "iLaborAccount");
            dic.Add("账户所有人", "iAccountOwnner");
            dic.Add("备注1", "iNote1");
            dic.Add("备注2", "iNote2");

            DataTable dt = new DataTable();
            dt.Columns.Add("iGuid");
            dt.Columns.Add("iCompanyCode");

            //默认，第一行是字段
            IRow headRow = sheet.GetRow(1);

            //设置datatable字段
            for (int i = headRow.FirstCellNum, len = headRow.LastCellNum; i < len; i++)
            {
                if (dic.ContainsKey(headRow.Cells[i].StringCellValue))
                {
                    dt.Columns.Add(dic[headRow.Cells[i].StringCellValue]);
                }
                else
                {
                    log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                    log.Error("excel里列名有改动：列" + headRow.Cells[i].StringCellValue);
                }
            }
            //遍历数据行
            for (int i = (sheet.FirstRowNum + 2), len = sheet.LastRowNum + 1; i < len; i++)
            {
                IRow tempRow = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                dataRow[0] = Guid.NewGuid();
                dataRow[1] = SessionHelper.CurrentUser.CurrentCompany;

                //遍历一行的每一个单元格
                for (int r = 2, j = tempRow.FirstCellNum, len2 = tempRow.LastCellNum; j < len2; j++, r++)
                {

                    ICell cell = tempRow.GetCell(j);

                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.String:
                                dataRow[r] = cell.StringCellValue;
                                break;
                            case CellType.Numeric:
                                dataRow[r] = cell.NumericCellValue;
                                break;
                            case CellType.Boolean:
                                dataRow[r] = cell.BooleanCellValue;
                                break;
                            default: dataRow[r] = "ERROR";
                                break;
                        }
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        private void WriteDataTableToServer(DataTable dt, string targettable, string connectstr)
        {

            using (SqlConnection conn = new SqlConnection(connectstr))
            {
                conn.Open();
                using (SqlBulkCopy sqlbulk = new SqlBulkCopy(conn))
                {

                    sqlbulk.DestinationTableName = targettable;
                    sqlbulk.NotifyAfter = dt.Rows.Count;
                    for (int i = 0; i < dt.Columns.Count; ++i)
                    {
                        SqlBulkCopyColumnMapping map = new SqlBulkCopyColumnMapping(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        sqlbulk.ColumnMappings.Add(map);

                    }
                    sqlbulk.WriteToServer(dt);

                }

            }

        }

        #endregion

        #region 菜单方法
        public string SaveItemTree(string treeJson)
        {
            try
            {
                List<ItemContent2> contents = JsonConvert.DeserializeObject<List<ItemContent2>>(treeJson);
                List<ItemContent> contents2 = RecursiveFun(contents[0]);
                if (contents2.Count() > 0)
                {
                    string[] nameUrl = contents2[0].name.Split('$');
                    StringBuilder sourceTableSb = new StringBuilder("MERGE INTO [SysMenu] AS T USING( SELECT ");
                    sourceTableSb.Append("'" + contents2[0].id + "' as IGUID, ");
                    sourceTableSb.Append("'" + contents2[0].pId + "' as IPARENTID, ");
                    sourceTableSb.Append("'" + nameUrl[0].Replace("'", "''") + "' as INAME, ");
                    sourceTableSb.Append("'" + nameUrl[1] + "' as IURL ");

                    for (int i = 1; i < contents2.Count; i++)
                    {
                        nameUrl = contents2[i].name.Split('$');
                        sourceTableSb.Append(" union all select ");
                        sourceTableSb.Append("'" + contents2[i].id + "', ");
                        sourceTableSb.Append("'" + contents2[i].pId + "', ");
                        sourceTableSb.Append("'" + nameUrl[0].Replace("'", "''") + "', ");
                        sourceTableSb.Append("'" + nameUrl[1] + "' ");
                    }
                    sourceTableSb.Append(") AS S ON T.IGUID = S.IGUID WHEN MATCHED THEN UPDATE SET T.IPARENTID=S.IPARENTID, T.INAME=S.INAME, T.IUrl = S.IURL WHEN NOT MATCHED THEN INSERT(IGUID,IPARENTID,INAME,IURL) VALUES(S.IGUID,S.IPARENTID,S.INAME,S.IURL);");

                    DbHelperSQL.ExecuteSql(sourceTableSb.ToString());

                }
                /* 样例
       MERGE INTO [TestDb].[dbo].[GUIDTable] AS T
       USING 
     ( select '1' as iguid, 'test1'as idata
     union all select '1375' , 'test1375'
     union all select '2' , 'test2'
     ) 
     AS S
     ON T.iguid=S.iguid
     WHEN MATCHED 
     THEN UPDATE SET T.idata=S.idata , t.iguid=s.iguid
     WHEN NOT MATCHED 
     THEN INSERT(iguid)  VALUES(S.iguid);
     */
                return "success";

            }
            catch (Exception e)
            {
                return "fail" + e.ToString();
            }

        }

        private List<ItemContent> RecursiveFun(ItemContent2 items)
        {
            List<ItemContent> resultTemp = new List<ItemContent>();
            resultTemp.Add(new ItemContent { id = items.id, pId = items.pId, name = items.name });

            if (items.children == null || items.children.Count() == 0)
                return resultTemp;
            else
            {
                foreach (var item in items.children)
                {
                    resultTemp.AddRange(RecursiveFun(item));
                }
                return resultTemp;
            }
        }

        public JsonResult GetUserMenuTree_old(string userId, string projectid)
        {
            try
            {
                string querySql = "select iMenuId+'|'+iMenuRights from sysUserMenu where iemployeecode = '{0}' and iProjectCode= '{1}'";
                DataSet ds = DbHelperSQL.Query(string.Format(querySql, userId, projectid));
                List<string> contents = new List<string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    contents.Add(dr[0].ToString());
                }
                return new JsonResult { Data = new { success = true, data = contents }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, data = "", msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public JsonResult GetUserMenuTree(string userId, string companyProjectTreeNodeId)
        {
            try
            {
                string[] array = companyProjectTreeNodeId.Split('|');
                string companyId = array[0];
                string projectId = array.Length > 1 ? array[1] : "-";
                string querySql = "select iMenuId+'|'+iMenuRights from sysUserMenuTree where iemployeecodeid = '{0}' and iCompanyId= '{1}' and iProjectId = '{2}'";
                DataSet ds = DbHelperSQL.Query(string.Format(querySql, userId, companyId, projectId));
                List<string> contents = new List<string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    contents.Add(dr[0].ToString());
                }
                return new JsonResult { Data = new { success = true, data = contents }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, data = "", msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public string UserMenuSaveChanges(string jsonString, string userid, string projectid)
        {
            //在超级用户与普通用户变更时会有冗余数据出现，不过并不影响，可清可不清
            try
            {
                List<string> menuIds = JsonConvert.DeserializeObject<List<string>>(jsonString);

                DataTable dt = new DataTable("sysUserMenu");
                dt.Columns.Add("iEmployeeCode");
                dt.Columns.Add("iProjectCode");
                dt.Columns.Add("iMenuId");
                dt.Columns.Add("iMenuRights");
                foreach (string menuid in menuIds)
                {
                    DataRow dataRow = dt.NewRow();
                    dataRow[0] = userid;
                    dataRow[1] = projectid;
                    dataRow[2] = menuid.Split('|')[0];
                    dataRow[3] = menuid.Split('|')[1];
                    dt.Rows.Add(dataRow);
                }
                string deleteSql = "delete from [sysUserMenu] where iemployeecode = '{0}' and iProjectCode = '{1}' ";
                DbHelperSQL.ExecuteSql(string.Format(deleteSql, userid, projectid));
                WriteDataTableToServer(dt, "sysUserMenu", ConfigurationManager.ConnectionStrings["HRMSDBConnectionString"].ConnectionString);
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        public string UserMenuTreeSaveChanges(string jsonString, string userid, string companyProjectTreeNodeId)
        {
            //在超级用户与普通用户变更时会有冗余数据出现，不过并不影响，可清可不清
            try
            {
                List<string> menuIds = JsonConvert.DeserializeObject<List<string>>(jsonString);
                string[] array = companyProjectTreeNodeId.Split('|');
                string companyId = array[0];
                string projectId = array.Length > 1 ? array[1] : "-";

                DataTable dt = new DataTable("sysUserMenuTree");
                dt.Columns.Add("iEmployeeCodeId");
                dt.Columns.Add("iCompanyId");
                dt.Columns.Add("iProjectId");
                dt.Columns.Add("iMenuId");
                dt.Columns.Add("iMenuRights");
                foreach (string menuid in menuIds)
                {
                    DataRow dataRow = dt.NewRow();
                    dataRow[0] = userid;
                    dataRow[1] = companyId;
                    dataRow[2] = projectId;
                    dataRow[3] = menuid.Split('|')[0];
                    dataRow[4] = menuid.Split('|')[1];
                    dt.Rows.Add(dataRow);
                }
                string deleteSql = "delete from [sysUserMenuTree] where iemployeecodeId = '{0}' and iCompanyId='{1}' and iProjectId = '{2}' ";
                DbHelperSQL.ExecuteSql(string.Format(deleteSql, userid, companyId, projectId));
                WriteDataTableToServer(dt, "sysUserMenuTree", ConfigurationManager.ConnectionStrings["HRMSDBConnectionString"].ConnectionString);
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public List<ItemContent> GetStandardMenuTree(bool widthUrl = true)
        {
            string querySql = "select iguid, iparentid, iname, iUrl from  [sysMenu] ";
            DataSet ds = DbHelperSQL.Query(querySql);
            List<ItemContent> contents = new List<ItemContent>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (widthUrl)
                {
                    contents.Add(new ItemContent { id = dr[0].ToString(), pId = dr[1].ToString(), name = dr[2].ToString() + "$" + ((dr[3] != null && !string.IsNullOrEmpty(dr[3].ToString())) ? dr[3].ToString() : ""), open = true });

                }
                else
                {
                    contents.Add(new ItemContent { id = dr[0].ToString(), pId = dr[1].ToString(), name = dr[2].ToString(), open = true });

                }
            }
            return contents;
        }

        public string GetMenuHtml()
        {
            string querySql = "select iguid, iparentid, iname, iUrl from  [sysMenu] a inner join [sysUserMenu] b on a.iguid = b.imenuid and b.iemployeecode='" + SessionHelper.CurrentUser.UserId + "' and b.iProjectCode='" + ((SessionHelper.CurrentUser.UserType == "超级用户") ? "-" : SessionHelper.CurrentUser.CurrentProject) + "' ";

            if (SessionHelper.CurrentUser.UserType == "超级管理员")
            {
                querySql = "select iguid, iparentid, iname, iUrl from  [sysMenu]";
            }
            DataSet ds = DbHelperSQL.Query(querySql);
            List<ItemContent> contents = new List<ItemContent>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                contents.Add(new ItemContent { id = dr[0].ToString(), pId = dr[1].ToString(), name = dr[2].ToString() + "$" + ((dr[3] != null && !string.IsNullOrEmpty(dr[3].ToString())) ? dr[3].ToString() : "") });
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in contents.Where(i => i.pId == "0"))
            {
                sb.AppendLine(string.Format("<li class=\"sub-menu\"> \n <a href=\"javascript:;\"> \n <i class=\"icon-laptop\"></i> \n <span>{0}</span> \n </a>", item.name.Split('$')[0]));

                foreach (var menu in contents.Where(i => i.pId == item.id))
                {
                    sb.AppendLine(string.Format("<ul class=\"sub\"> \n <li><a href=\"{1}\">{0}</a></li> \n </ul>", menu.name.Split('$')[0], menu.name.Split('$')[1]));
                }
                sb.AppendLine("</li>");
            }
            return sb.ToString();
        }

        public string GetAuthorisedMenuHtml()
        {
            string querySql = "";
            //普通用户
            if (SessionHelper.CurrentUser.UserType == "普通用户")
            {
                if (SessionHelper.CurrentUser.CurrentCompany == "-")
                {
                    return "";
                }
                else
                {
                    querySql = "select iguid, iparentid, iname, iUrl from  [sysMenu] a inner join [sysUserMenuTree] b on a.iguid = b.imenuid and b.iEmployeeCodeId='" + SessionHelper.CurrentUser.UserId + "' and b.iCompanyId='" + SessionHelper.CurrentUser.CurrentCompany + "' and b.iProjectId='" + SessionHelper.CurrentUser.CurrentProject + "' ";
                }
            }
            else if (SessionHelper.CurrentUser.UserType == "超级用户")
            {
                querySql = "select iguid, iparentid, iname, iUrl from  [sysMenu] a inner join [sysUserMenuTree] b on a.iguid = b.imenuid and b.iEmployeeCodeId='" + SessionHelper.CurrentUser.UserId + "' and b.iCompanyId='-' and b.iProjectId='-' ";
            }
            else if (SessionHelper.CurrentUser.UserType == "超级管理员")
            {
                querySql = "select iguid, iparentid, iname, iUrl from  [sysMenu]";
            }
            DataSet ds = DbHelperSQL.Query(querySql);
            if (SessionHelper.CurrentUser.UserType == "普通用户" && ds.Tables[0].Rows.Count == 0)  //普通用户没查到项目上的，查绑定到公司的
            {
                querySql = "select iguid, iparentid, iname, iUrl from  [sysMenu] a inner join [sysUserMenuTree] b on a.iguid = b.imenuid and b.iEmployeeCodeId='" + SessionHelper.CurrentUser.UserId + "' and b.iCompanyId='" + SessionHelper.CurrentUser.CurrentCompany + "' and b.iProjectId='-' ";
                ds = DbHelperSQL.Query(querySql);
            }

            List<ItemContent> contents = new List<ItemContent>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                contents.Add(new ItemContent { id = dr[0].ToString(), pId = dr[1].ToString(), name = dr[2].ToString() + "$" + ((dr[3] != null && !string.IsNullOrEmpty(dr[3].ToString())) ? dr[3].ToString() : "") });
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in contents.Where(i => i.pId == "0"))
            {
                sb.AppendLine(string.Format("<li class=\"sub-menu\"> \n <a href=\"javascript:;\"> \n <i class=\"icon-laptop\"></i> \n <span>{0}</span> \n </a>", item.name.Split('$')[0]));

                foreach (var menu in contents.Where(i => i.pId == item.id))
                {
                    sb.AppendLine(string.Format("<ul class=\"sub\"> \n <li><a href=\"{1}\">{0}</a></li> \n </ul>", menu.name.Split('$')[0], menu.name.Split('$')[1]));
                }
                sb.AppendLine("</li>");
            }
            return sb.ToString();
        }

        #endregion

        #region 公司方法
        public void GetAllCompanies()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);
            string searchKey = HttpContext.Request.Params["search"];
            Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
            bizParaDic.Add("search", searchKey);

            int total = 0;
            DicManager dm = new DicManager();
            List<CompanyEntity> list = dm.CompanyGetSearch(bizParaDic, sort, order, offset, pageSize, out total);
            //给分页实体赋值  
            PageModels<CompanyEntity> model = new PageModels<CompanyEntity>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = list;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }
        public string CheckCompanyValid(string iName)
        {
            DicManager dm = new DicManager();
            if (dm.CheckCompanyValid(iName))
            {
                return "valid";
            }
            return "invalid";

        }
        public string CompanySaveChanges(string jsonString)
        {
            try
            {
                CompanyEntity en = JsonConvert.DeserializeObject<CompanyEntity>(jsonString);
                DicManager dm = new DicManager();
                if (string.IsNullOrEmpty(en.iGuid))
                {
                    en.iGuid = Guid.NewGuid().ToString();
                    en.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    en.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    dm.Insert(en);
                }
                else
                {
                    CompanyEntity enOld = dm.CompanyFirstOrDefault(en.iGuid);
                    enOld.iName = en.iName;
                    enOld.iIsDeleted = en.iIsDeleted;
                    enOld.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    dm.Update(enOld);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        #endregion

        #region 项目方法
        public void GetAllProjects()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);
            string searchKey = HttpContext.Request.Params["search"];
            Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
            bizParaDic.Add("search", searchKey);

            int total = 0;
            DicManager dm = new DicManager();
            List<ProjectEntity> list = dm.ProjectGetSearch(bizParaDic, sort, order, offset, pageSize, out total);
            //给分页实体赋值  
            PageModels<ProjectEntity> model = new PageModels<ProjectEntity>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = list;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }
        public string CheckProjectValid(string iName)
        {
            DicManager dm = new DicManager();
            if (dm.CheckProjectValid(iName))
            {
                return "valid";
            }
            return "invalid";

        }
        public string ProjectSaveChanges(string jsonString)
        {
            try
            {
                ProjectEntity en = JsonConvert.DeserializeObject<ProjectEntity>(jsonString);
                DicManager dm = new DicManager();
                if (string.IsNullOrEmpty(en.iGuid))
                {
                    en.iGuid = Guid.NewGuid().ToString();
                    en.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    en.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    dm.Insert(en);
                }
                else
                {
                    ProjectEntity enOld = dm.ProjectFirstOrDefault(en.iGuid);
                    enOld.iName = en.iName;
                    enOld.iIsDeleted = en.iIsDeleted;
                    enOld.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    dm.Update(enOld);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        #endregion

        #region 项目公司对应关系
        public JsonResult GetCompaniesOfProject(string projectid)
        {
            try
            {
                DicManager service = new DicManager();
                List<string> result = service.GetCompaniesOfProject(projectid);
                return new JsonResult { Data = new { success = true, msg = "msg", data = result }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
                log.Error(ex);
                return new JsonResult { Data = new { success = false, msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public string CompanyProjectRelationSaveChanges(string checkedStr)
        {
            try
            {
                string projectId = checkedStr.Split('$')[0];
                List<string> checkedCompanyIdList = checkedStr.Split('$')[1].TrimEnd('|').Split('|').ToList();
                DicManager dm = new DicManager();
                dm.SaveCompanyProjectRelation(projectId, checkedCompanyIdList, SessionHelper.CurrentUser.UserName);
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public List<ItemContent> GetCompanyProjectTree()
        {
            string querySql = "SELECT [iGuid], '' pid, [iName] FROM [SysCompany] where iStatus=1 and iIsDeleted=0 union all select icompanyid+'|'+iprojectid, icompanyid, iname from [SysCompanyProjectRelation] a inner join [SysProject] b on a.iProjectId = b.iGuid  and a.iStatus = 1 and a.iIsDeleted=0 and b.iStatus =1 and b.iIsDeleted=0";
            DataSet ds = DbHelperSQL.Query(querySql);
            List<ItemContent> contents = new List<ItemContent>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                contents.Add(new ItemContent { id = dr[0].ToString(), pId = dr[1].ToString(), name = dr[2].ToString(), open = true });

            }
            return contents;
        }

        public string UserCompanyProjectSaveChanges(string jsonString, string userid)
        {
            try
            {
                List<string> treeIds = JsonConvert.DeserializeObject<List<string>>(jsonString);
                DataTable dt = new DataTable("SysUserCompanyProjectTree");
                dt.Columns.Add("iEmployeeCodeId");
                dt.Columns.Add("iId");
                dt.Columns.Add("iPid");
                foreach (string node in treeIds)
                {
                    DataRow dataRow = dt.NewRow();
                    dataRow[0] = userid;
                    dataRow[1] = node;
                    dataRow[2] = node.Split('|')[0];   //保存了公司
                    dt.Rows.Add(dataRow);
                }
                string deleteSql = "delete from [SysUserCompanyProjectTree] where iEmployeeCodeId = '" + userid + "'";
                DbHelperSQL.ExecuteSql(deleteSql);
                WriteDataTableToServer(dt, "SysUserCompanyProjectTree", ConfigurationManager.ConnectionStrings["HRMSDBConnectionString"].ConnectionString);
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public JsonResult GetUserCompanyProjectTree(string userid)
        {
            try
            {
                string querySql = "select iId from SysUserCompanyProjectTree where iEmployeeCodeId = '{0}' ";
                DataSet ds = DbHelperSQL.Query(string.Format(querySql, userid));
                List<string> contents = new List<string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    contents.Add(dr[0].ToString());
                }
                return new JsonResult { Data = new { success = true, data = contents }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, data = "", msg = ex.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        #endregion

        #region 流程对应人员
        public void GetJournalBpmUsers()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);
            string searchKey = HttpContext.Request.Params["search"];
            Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
            bizParaDic.Add("search", searchKey);

            int total = 0;
            DicManager dm = new DicManager();
            List<BpmUserView> list = dm.JournalBpmUserGetSearch(bizParaDic, sort, order, offset, pageSize, out total);
            //给分页实体赋值  
            PageModels<BpmUserView> model = new PageModels<BpmUserView>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = list;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }
        public string JournalBpmUserSaveChanges(string jsonString)
        {
            try
            {
                BpmUserEntity en = JsonConvert.DeserializeObject<BpmUserEntity>(jsonString);
                DicManager dm = new DicManager();
                if (string.IsNullOrEmpty(en.iGuid))
                {
                    en.iGuid = Guid.NewGuid().ToString();
                    en.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    en.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    dm.InsertBpmUser(en);
                }
                else
                {
                    BpmUserEntity enOld = dm.BpmUserFirstOrDefault(en.iGuid);
                    enOld.iUsers = en.iUsers;
                    enOld.iNote = en.iNote;
                    enOld.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    dm.UpdateBpmUser(enOld);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        #endregion

        #region 流程角色人员
        public void GetAllRoles()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            int offset = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageSize = Convert.ToInt32(HttpContext.Request.Params["limit"]);
            string searchKey = HttpContext.Request.Params["search"];
            Dictionary<string, string> bizParaDic = new Dictionary<string, string>();
            bizParaDic.Add("search", searchKey);

            int total = 0;
            DicManager dm = new DicManager();
            List<RoleEntity> list = dm.RoleGetSearch(bizParaDic, sort, order, offset, pageSize, out total);
            //给分页实体赋值  
            PageModels<RoleEntity> model = new PageModels<RoleEntity>();
            model.total = total;
            if (total % pageSize == 0)
                model.page = total / pageSize;
            else
                model.page = (total / pageSize) + 1;

            model.rows = list;

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }
        public string CheckRoleValid(string iName)
        {
            DicManager dm = new DicManager();
            if (dm.CheckRoleValid(iName))
            {
                return "valid";
            }
            return "invalid";

        }
        public string RoleSaveChanges(string jsonString)
        {
            try
            {
                RoleEntity en = JsonConvert.DeserializeObject<RoleEntity>(jsonString);
                DicManager dm = new DicManager();
                if (string.IsNullOrEmpty(en.iGuid))
                {
                    en.iGuid = Guid.NewGuid().ToString();
                    en.iCreatedBy = SessionHelper.CurrentUser.UserName;
                    en.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    dm.InsertRole(en);
                }
                else
                {
                    RoleEntity enOld = dm.RoleFirstOrDefault(en.iGuid);
                    enOld.iName = en.iName;
                    enOld.iNote = en.iNote;
                    enOld.iIsDeleted = en.iIsDeleted;
                    enOld.iUpdatedBy = SessionHelper.CurrentUser.UserName;
                    dm.UpdateRole(enOld);
                }
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        public string RoleUsersSaveChanges(string usersStr)
        {
            try
            {
                string roleid = usersStr.Split('$')[0];
                List<string> sqls = new List<string>();
                sqls.Add("delete from [SysRoleUsers] where iRoleGuid = '" + roleid + "'");
                string tmpl = "insert into SysRoleUsers values(newid(),'{0}','{1}','{2}',getdate(),'sys',getdate(),'sys',1,0 )";
                foreach (string node in usersStr.Split('$')[1].Split('|'))
                {
                    sqls.Add(string.Format(tmpl, roleid, node.Split('§')[0], node.Split('§')[1]));
                }
                DbHelperSQL.ExecuteSqlTran(sqls);
                return "success";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        public JsonResult GetRoleUsers(string roleid)
        {
            try
            {
                DicManager dm = new DicManager();
                List<RoleUsersEntity> result = dm.GetRoleUsers(roleid);
                return new JsonResult { Data = new { success = true, data = result }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception e)
            {
                return new JsonResult { Data = new { success = false, msg = e.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        #endregion
    }
}
