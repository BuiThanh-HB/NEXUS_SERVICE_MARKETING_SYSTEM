using Data.Utils;
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
        public int CateID { get; set; }
        public string CateType { get; set; }
        public string CateTypeStr {
            set { }
            get
            {
                string output = "";
                switch (CateType)
                {
                    case SystemParam.FOR_DIAL:
                        output = "Để quay số";
                        break;
                    case SystemParam.CONNECT_PHONE_ONLY:
                        output = "Chỉ kết nối điện thoại";
                        break;
                    case SystemParam.BROADBAND:
                        output = "Cho băng thông rộng";
                        break;
                }
                return output;
            }
        }
    }
}