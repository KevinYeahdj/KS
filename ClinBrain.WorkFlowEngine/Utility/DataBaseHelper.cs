﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.WorkFlowEngine.Utility
{
    internal class DataBaseHelper
    {
        internal static DbTransaction BeginTransaction(DataContext dataContext)
        {
            if (dataContext.Connection.State != ConnectionState.Open)
                dataContext.Connection.Open();

            return dataContext.Connection.BeginTransaction();
        }

        internal static DbTransaction BeginTransaction(DataContext dataContext,
            IsolationLevel isolationLevel)
        {
            if (dataContext.Connection.State != ConnectionState.Open)
                dataContext.Connection.Open();

            return dataContext.Connection.BeginTransaction(isolationLevel);
        }
    }
}
