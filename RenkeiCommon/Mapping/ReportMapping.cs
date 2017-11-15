using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RenkeiCommon.Mapping
{
    public class ReportMapping
    {
        public string FormatId { get; set; }

        public string Condition { get; set; }

        public string TableNm { get; set; }

        public string Registerkey { get; set; }

        public string Report_data { get; set; }

        public string RootPath { get; set; }

        public ReportInfoList ReportInfo { get; set; }
    }
}