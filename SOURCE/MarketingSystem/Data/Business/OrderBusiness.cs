using APIProject.Models;
using Data.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Business
{
    public class OrderBusiness : GenericBusiness
    {
        ResponseBusiness rp = new ResponseBusiness();
        public OrderBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }

        //Đăng ký dịch vụ
        //public JsonResultModel CreateOrder(int id)
        //{

        //}
    }
}