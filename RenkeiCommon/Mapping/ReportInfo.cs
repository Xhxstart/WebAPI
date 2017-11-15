using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RenkeiCommon.Mapping
{
    public class ReportInfoList
    {
        public List<BaseColumns> BaseData { get; set; }

        public List<ReportColumns> ReportData { get; set; }

        public List<List<ImageColumns>> ImageData { get; set; }
    }
}