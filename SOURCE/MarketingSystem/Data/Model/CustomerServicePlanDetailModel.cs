using System;
using Data.DB;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class CustomerServicePlanDetailModel : ListServicePlanOfCus
    {
        public string Note { get; set; }
        public int Type { get; set; }
        public int UserID { get; set; }
        public List<HistoryCustomerServicePlan> histories { get; set; }
    }
}