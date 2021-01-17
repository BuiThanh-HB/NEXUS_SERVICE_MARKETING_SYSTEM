using Data.DB;
using Data.Model;
using Data.Utils;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Business
{
    public class ServicePlanOfCusBusiness : GenericBusiness
    {
        public ServicePlanOfCusBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }

        EmailBusiness email = new EmailBusiness();

        IPagedList<ListServicePlanOfCus> Search(int page, string searchKey, string code, int? status, string fromDate, string toDate)
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);
                var data = cnn.CustomerServicePlans.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && (!String.IsNullOrEmpty(searchKey) ? c.Customer.Name.Contains(searchKey) || c.Customer.Phone.Contains(searchKey) : true)
                && (!String.IsNullOrEmpty(code) ? c.code.Equals(code) : true) && (fd.HasValue ? c.CreatedDate >= fd.Value : true) && (td.HasValue ? c.CreatedDate <= td.Value : true) && (status.HasValue ? c.Status.Equals(status) : true))
                    .Select(c => new ListServicePlanOfCus
                    {

                    }).OrderByDescending(c => c.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch
            {
                return new List<ListServicePlanOfCus>().ToPagedList(1, 1);
            }

        }
    }
}