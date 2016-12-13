using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kevin.Common
{
    public static class LogFileHelper
    {
        public static void WriteLog(string msg)
        {
            string logFolderPath = System.AppDomain.CurrentDomain.BaseDirectory + "/log";
            string FilePath = logFolderPath + "/ErrorLog_" + DateTime.Now.ToString("yyyyMMdd")+ ".txt";

            try
            {
                if (Directory.Exists(logFolderPath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(logFolderPath);
                }

                if (File.Exists(FilePath))
                {
                    using (StreamWriter tw = File.AppendText(FilePath))
                    {
                        tw.WriteLine(DateTime.Now.ToString() + "> " + msg);
                    }  //END using

                }  //END if
                else
                {
                    FileStream fs = new FileStream(FilePath ,FileMode.Create, FileAccess.Write);
                    using (StreamWriter tw = new StreamWriter(fs))
                    {
                        tw.WriteLine(DateTime.Now.ToString() + "> " + msg);
                    }  //END using
                    fs.Close();
                }  
            }  
            catch (Exception ex)
            { } //END Catch   

        }  // END ErrorLog

    }
}
