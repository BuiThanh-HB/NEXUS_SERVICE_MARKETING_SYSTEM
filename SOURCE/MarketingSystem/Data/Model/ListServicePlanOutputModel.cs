using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class ListServicePlanOutputModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CateName { get; set; }
        public int Price { get; set; }
        public int Value { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public string Descreiption { get; set; }
    }
}