using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class HomeFrontEndOutputModel
    {
        public List<ListNewsOutputModel> ListNews { get; set; }
        public List<ListServicePlanOutputModel> ListService { get; set; }
    }
}