using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class ListNewsOutputModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public bool Status { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CateID { get; set; }
        public string CateName { get; set; }
        public string CreatedDateStr 
        {
            set { }
            get
            {
                return CreatedDate.ToString(SystemParam.CONVERT_DATETIME) ;
            }
        }

    }
}