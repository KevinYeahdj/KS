﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.WorkFlowEngine.Common
{
    /// <summary>
    /// 任务的执行者对象
    /// </summary>
    public class Performer
    {
        public Performer(string userID, string userName)
        {
            UserID = userID;
            UserName = userName;
        }

        public string UserID
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
    }

    public class PerformerList : List<Performer>
    {
        public PerformerList()
        {
        }
    }
}
