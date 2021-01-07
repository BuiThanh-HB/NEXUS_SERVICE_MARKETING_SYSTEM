using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class RegisterCustomerInputModel
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public int province_id { get; set; }
        public int district_id { get; set; }
        public int village_id { get; set; }
    }
}