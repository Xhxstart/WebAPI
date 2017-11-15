using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    public class message
    {
        private bool success;
        private string msg;

        public message(bool success, string msg)
        {
            this.success = success;
            this.msg = msg;
        }

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }
    }
}