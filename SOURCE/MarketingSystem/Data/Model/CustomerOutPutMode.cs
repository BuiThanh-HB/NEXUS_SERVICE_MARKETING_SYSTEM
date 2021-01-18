using Data.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class CustomerOutPutMode
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int ProvinceID { get; set; }
        public string Province { get; set; }
        public int DistrictID { get; set; }
        public string District { get; set; }
        public int VillageID { get; set; }
        public string Village { get; set; }

    }
}