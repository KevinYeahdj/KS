using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.WorkFlowEngine.Business.Entity
{
    public abstract class QueryBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRowsCount { get; set; }
    }

    public class Query<T> : QueryBase
        where T:class
    {
        public T Entity { get; set; }
    }
}
