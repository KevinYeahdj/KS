using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Business.Manager;

namespace ClinBrain.WorkFlowEngine.Utility
{
    public class ApplicationNoHelper
    {
        public string GetFlowApplicationNo(string employeeCode, string processPrefix)
        {
            int SEQ_LENGTH = 4;
            string DATEPART_FORMAT = "yyyyMMdd";
            string FLOW_PREFIX = processPrefix;
            StringBuilder appNo = new StringBuilder();

            if (!string.IsNullOrEmpty(FLOW_PREFIX))
            {
                appNo.Append(FLOW_PREFIX);
            }
            appNo.AppendFormat("-" + System.DateTime.Now.ToString(string.IsNullOrEmpty(DATEPART_FORMAT) ? "yyyyMMdd" : DATEPART_FORMAT));

            return appNo.ToString() + "-" + GetID(appNo.ToString()).PadLeft(SEQ_LENGTH, '0');

        }
        private string GetID(string name)
        {
            ProcessInstanceManager pim = new ProcessInstanceManager();
            var processNoCheck = pim.GetProcessNOCheck(name);
            if(processNoCheck ==  null)
            {
                ProcessNoCheckEntity pnce = new ProcessNoCheckEntity();
                pnce.KeyName = name;
                pnce.NumberValue = 1;
                pim.InsertProcessNOCheck(pnce);
                return "1";
            }
            else
            {
                processNoCheck.NumberValue ++;
                pim.UpdateProcessNOCheck(processNoCheck);
                return processNoCheck.NumberValue.ToString();
            }
        }
    }
}
