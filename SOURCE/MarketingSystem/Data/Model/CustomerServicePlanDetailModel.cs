using System;
using Data.DB;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Utils;

namespace Data.Model
{
    public class CustomerServicePlanDetailModel : ListServicePlanOfCus
    {
        public string Note { get; set; }
        public int Type { get; set; }
        public int UserID { get; set; }
        public List<HistoryCustomerServicePlan> histories { get; set; }
    }

    public class CustomerService
    {
        public int ID { get; set; }
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ImageUrl { get; set; }
        public int Value { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public double Price { get; set; }
        public int OrderID { get; set; }
        public DateTime ActiveDate { get; set; }
        public string ActiveDateStr
        {
            get
            {
                return  ActiveDate.ToString(SystemParam.CONVERT_DATETIME);
            }
            set { }
        }
        public DateTime? ExtendDate { get; set; }
        public string ExtendDateStr
        {
            get
            {
                return ExtendDate.HasValue ? ExtendDate.Value.ToString(SystemParam.CONVERT_DATETIME) : "";
            }
            set { }
        }
        public DateTime? ExpiryDate { get; set; }
        public string ExpiryDateStr
        {
            get
            {
                return ExpiryDate.HasValue ? ExpiryDate.Value.ToString(SystemParam.CONVERT_DATETIME) : "";
            }
            set { }
        }
    }
}