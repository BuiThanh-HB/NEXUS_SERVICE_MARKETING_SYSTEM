using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class OrderDetailOuputModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string CusPhone { get; set; }
        public string CusName { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public string ServiceName { get; set; }
        public string AdminNote { get; set; }
        public string LocationRequest { get; set; }
        public int DiscountValue { get; set; }
    }
}