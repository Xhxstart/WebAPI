using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RenkeiCommon.Mapping
{
    public class ReportColumns
    {
        public string ColumnNm { get; set; }
        public object DefaultValue { get; set; }
        public string RefReportDataKey { get; set; }
        public string RefSubReportIdKey { get; set; }
        public bool Condition { get; set; }
        //public string CD_SECTION { get; set; }

        private object value;

        public object Value
        {
            get
            {
                if (this.value != null)
                {
                    return (object)this.value.ToString().Trim();
                }
                else
                {
                    return this.value;
                }
            }
            set
            {
                this.value = value;
            }
        }
    }
}