﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.WorkFlowEngine.Common
{
    public class XmlDefinitionException : System.ApplicationException
    {
         public XmlDefinitionException(string message)
            : base(message)
        {

        }

         public XmlDefinitionException(string message, System.Exception ex)
            : base(message, ex)
        {

        }
    }
}
