using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RenkeiCommon.Mapping
{
    public class BaseColumns
    {
        public string ColumnNm { get; set; }
        public string RefReportDataKey { get; set; }

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