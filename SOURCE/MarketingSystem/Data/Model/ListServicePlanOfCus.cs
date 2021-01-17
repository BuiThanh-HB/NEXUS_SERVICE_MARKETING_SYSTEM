using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class ListServicePlanOfCus
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ServiceName { get; set; }
        public string CusName { get; set; }
        public int Status { get; set; }
        public DateTime ActiveDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LocaRequest { get; set; }
    }
}