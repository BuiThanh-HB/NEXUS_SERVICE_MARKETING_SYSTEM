using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class UserDetailOutputModel
    {
        public int ID { get; set; }
        public int Role { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public int role { get; set; }
        public DateTime? CreateDate { set; get; }
        public string Token { get; set; }
        public string CraeteDateStr
        {
            set { }
            get
            {
                return CreateDate.HasValue ? CreateDate.Value.ToString(SystemParam.CONVERT_DATETIME) : "";
            }
        }
    }
}