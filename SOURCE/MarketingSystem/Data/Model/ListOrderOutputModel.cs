using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class ListOrderOutputModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string CusPhone { get; set; }
        public string CusName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }

    }
}