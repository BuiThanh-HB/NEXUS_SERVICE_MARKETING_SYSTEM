using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class NewsDetailOutputModel
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string ImgUrl { get; set; }
    }

    public class CategoryNewsOutputModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}