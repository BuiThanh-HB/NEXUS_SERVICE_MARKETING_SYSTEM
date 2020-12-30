using Data.Utils;
using Data.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.DB;

namespace Data.Business
{
    public class NewsBusiness : GenericBusiness
    {
        ResponseBusiness rp = new ResponseBusiness();
        public NewsBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }

        public IPagedList<ListNewsOutputModel> Search(int page, string searchKey, Boolean? status, int? type, string fromDate, string toDate)
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);
                var data = cnn.News.Where(n => n.IsActive.Equals(SystemParam.ACTIVE) && (!String.IsNullOrEmpty(searchKey) ? n.Title.Contains(searchKey) : true)
                && (status.HasValue ? n.Status.Equals(status.Value) : true) && (type.HasValue ? n.Type.Equals(type.Value) : true)
                && (fd.HasValue ? n.CreatedDate >= fd.Value : true) && (td.HasValue ? n.CreatedDate <= td.Value : true))
                    .Select(n => new ListNewsOutputModel()
                    {
                        ID = n.ID,
                        Title = n.Title,
                        Status = n.Status,
                        ImgUrl = n.ImageUrl,
                        CateName = n.CategoryNew.Name,
                        CreatedDate = n.CreatedDate
                    }).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch
            {
                return new List<ListNewsOutputModel>().ToPagedList(1, 1);
            }
        }
    }
}