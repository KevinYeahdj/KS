﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinBrain.WorkFlowEngine.Parser
{
    public class process
    {
        public string name { get; set; }
        public string id { get; set; }
        public string Description { get; set; }
        public List<snode> snodes { get; set; }
        public List<sline> slines { get; set; }
    }
}
