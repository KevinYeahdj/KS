﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.WorkFlowEngine.Xpdl
{
    /// <summary>
    /// 流程定义的XML文件中，用到的常量定义
    /// </summary>
    internal class XPDLDefinition
    {
        internal static readonly string StrXmlTransitionPath = "WorkflowProcess/Process/Transitions/Transition";
        internal static readonly string StrXmlActivityPath = "WorkflowProcess/Process/Activities/Activity";
        internal static readonly string StrXmlParticipantsPath = "Participants";
        internal static readonly string StrXmlSingleParticipantPath = "Participants/Participant";
        internal static readonly string StrXmlDataItemPermissions = "DataCollection/DataItemPermissions";
        internal static readonly string StrXmlParticipantDataItemPermissions = "DataCollection/DataItemPermissions/Participant";
        internal static readonly string StrXmlDataItems = "DataCollection/DataItems";
        internal static readonly string StrXmlSingleDataItems = "DataCollection/DataItems/DataItem";
    }
}
