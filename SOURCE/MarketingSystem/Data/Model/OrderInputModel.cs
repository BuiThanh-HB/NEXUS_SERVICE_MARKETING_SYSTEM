using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class OrderInputModel
    {
        public int ServiceID { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int VillageID { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
    }
}