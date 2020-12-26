using Data.DB;
using Data.Utils;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Business
{
    public class ServicePlanBusiness : GenericBusiness
    {
        ResponseBusiness rp = new ResponseBusiness();
        public ServicePlanBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }

        //Lấy list danh mục
        public List<ListCategoryOutputModel> GetListCategory()
        {
            try
            {
                List<ListCategoryOutputModel> data = new List<ListCategoryOutputModel>();
                data = cnn.Categories.Where(c => c.IsActive.Equals(SystemParam.ACTIVE))
                   .Select(c => new ListCategoryOutputModel
                   {
                       ID = c.ID,
                       Name = c.Name
                   }).ToList();
                return data;
            }
            catch
            {
                return new List<ListCategoryOutputModel>();
            }
        }
    }
}