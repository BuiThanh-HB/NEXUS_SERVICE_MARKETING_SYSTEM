using System;
using Data.DB;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class CustomerServicePlanDetailModel : ListServicePlanOfCus
    {
        public List<HistoryCustomerServicePlan> histories { get; set; }
    }
}