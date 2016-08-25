using System;
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
using ClinBrain.Data.Entity;
using HRMS.Data.Manager;
using HRMS.WEB.Models;
using HRMS.WEB.Utils;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HRMS.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("序号","iNo");
            dic.Add("项目名称","iItemName");
            dic.Add("所在公司","iCompany");
            dic.Add("姓名","iName");
            dic.Add("工号","iEmpNo");
            dic.Add("电话","iPhone");
            dic.Add("紧急联系人","iEmeContact");
            dic.Add("紧急联系人电话","iEmeContactPhone");
            dic.Add("性别","iSex");
            dic.Add("出生日期","iBirthday");
            dic.Add("户籍","iRegistry");
            dic.Add("民族","iNation");
            dic.Add("身份证号","iIdCard");
            dic.Add("户口性质","iResidenceProperty");
            dic.Add("户籍地址","iRegistryAddress");
            dic.Add("签发机关","iIssuedBy");
            dic.Add("身份证有效期","iIdCardValidate");
            dic.Add("现住地","iLivedIn");
            dic.Add("员工状态","iEmployeeStatus");
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

            //StringBuilder sb = new StringBuilder("CREATE TABLE [dbo].[LaborInfo]( \n");
            //foreach (KeyValuePair<string, string> kvp in dic)
            //{
            //    sb.Append(kvp.Value);
            //    sb.Append(" [nvarchar](4000) NULL, \n");
            //}
            //string result = sb.ToString();

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.Append(string.Format("public string {0} {2} get; set; {3} //{1} \n",kvp.Value,kvp.Key,"{","}"));
            }
            string result = sb.ToString();
            int i = 0;







            //UserEntity user = new UserEntity();
            //user.EmployeeCode = Guid.NewGuid().ToString();
            //user.UserName = "Kevin";
            //user.PassWord = "123";
            //UserManager um = new UserManager();
            //um.Insert(user);

            return View();
        }

        public void GetAllPeople()
        {
            //用于序列化实体类的对象  
            JavaScriptSerializer jss = new JavaScriptSerializer();

            //请求中携带的条件  
            string order = HttpContext.Request.Params["order"];
            string sort = HttpContext.Request.Params["sort"];
            string searchKey = HttpContext.Request.Params["search"];
            int dataIndex = Convert.ToInt32(HttpContext.Request.Params["offset"]);  //0
            int pageCount = Convert.ToInt32(HttpContext.Request.Params["limit"]);

            LaborInfoManager lim = new LaborInfoManager();
            List<LaborInfoEntity> list = lim.GetSearch(SessionHelper.CurrentUser.iCompanyCode, searchKey, sort, order, dataIndex, pageCount)


            #region 模拟数据获取
            List<SimpleModel> list = new List<SimpleModel>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(new SimpleModel() { age = 18, name = "小李" + i, rkey = i + 1, sex = "男" });
            }


            //查询满足条件的数据  
            List<SimpleModel> getList;
            if (bysex != null && searchKey != null)
            {
                getList = (from p in list
                           where p.sex == (bysex == "0" ? "女" : "男") && p.name.Contains(searchKey.Trim())
                           select p).ToList();
            }
            else
            {
                getList = list;
            }
            #endregion

            //将结果增加一列序号列  
            Dictionary<int, SimpleModel> testModel = new Dictionary<int, SimpleModel>();
            for (int i = 0; i < getList.Count; i++)
            {
                testModel.Add(i + 1, getList[i]);
            }

            //给分页实体赋值  
            PageModels<SimpleModel> model = new PageModels<SimpleModel>();
            model.total = getList.Count;
            if (getList.Count % pageCount == 0)
                model.page = getList.Count / pageCount;
            else
                model.page = (getList.Count / pageCount) + 1;

            //獲取對應頁的數據  
            model.rows = testModel.Where(t => t.Key > dataIndex && t.Key <= dataIndex + pageCount).Select(t => t.Value).ToList();

            //将查询结果返回  
            HttpContext.Response.Write(jss.Serialize(model));
        }


        public JsonResult ImportPeople()
        {
            try
            {
                var oFile = HttpContext.Request.Files["txt_file"];
                //保存文件
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" +oFile.FileName;
                string path =  Server.MapPath("~") + "UploadFiles\\" + newFile;
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
                dataRow[1] = SessionHelper.CurrentUser.iCompanyCode;

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

    }
    public class SimpleModel
    {
        public int age { get; set; }
        public string name { get; set; }
        public int rkey { get; set; }
        public string sex { get; set; }
    }

    public class PageModels<T>
    {
        public int total { get; set; }
        public int page { get; set; }
        public List<T> rows { get; set; }
    }
}
