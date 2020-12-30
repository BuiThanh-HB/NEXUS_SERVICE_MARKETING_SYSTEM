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
        public DateTime CreatedDate { get; set; }
        public string CateName { get; set; }

    }
}