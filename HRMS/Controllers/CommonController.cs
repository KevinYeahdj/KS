using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.Converter;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HRMS.WEB.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/

        public ActionResult ExcelPreview(string fileName, string filePath)
        {
            FileStream fs = new FileStream(Server.MapPath("/UploadSalary/" + filePath), FileMode.Open);
            IWorkbook workbook = null;
            if (fileName.IndexOf(".xlsx") > -1) // 2007版本
                workbook = new XSSFWorkbook(fs);
            else // 2003版本
                workbook = new HSSFWorkbook(fs);
            ExcelToHtmlConverter excelToHtmlConverter = new ExcelToHtmlConverter();
            // 设置输出参数
            excelToHtmlConverter.OutputColumnHeaders = true;
            excelToHtmlConverter.OutputHiddenColumns = false;
            excelToHtmlConverter.OutputHiddenRows = false;
            excelToHtmlConverter.OutputLeadingSpacesAsNonBreaking = true;
            excelToHtmlConverter.OutputRowNumbers = true;
            excelToHtmlConverter.UseDivsToSpan = true;
            // 处理的Excel文件
            excelToHtmlConverter.ProcessWorkbook(workbook);
            ViewBag.html = excelToHtmlConverter.Document.InnerXml;
            return View();
        }
    }
}
