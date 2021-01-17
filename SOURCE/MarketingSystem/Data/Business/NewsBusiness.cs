using Data.Utils;
using Data.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.DB;
using APIProject.Models;

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
                    }).OrderByDescending(n => n.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch
            {
                return new List<ListNewsOutputModel>().ToPagedList(1, 1);
            }
        }
        
        //Thêm bài đăng

        public JsonResultModel CreateNews(int type, bool status, string content, string title, string summary, string img)
        {
            try
            {
                News n = new News();
                n.Title = title;
                n.Content = content;
                n.Status = status;
                n.Summary = summary;
                n.ImageUrl = img;
                n.IsActive = SystemParam.ACTIVE;
                n.CreatedDate = DateTime.Now;
                n.Type = type;
                n.CategoryNewID = type;
                cnn.News.Add(n);
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch (Exception e)
            {
                e.ToString();
                return rp.serverError();
            }
        }

        //Xem chi tiết bài viết
        public NewsDetailOutputModel GetNewsDetail(int id)
        {
            try
            {
                NewsDetailOutputModel data = new NewsDetailOutputModel();
                News n = cnn.News.Find(id);
                data.ID = n.ID;
                data.Title = n.Title;
                data.Summary = n.Summary;
                data.Type = n.Type;
                data.ImgUrl = n.ImageUrl;
                data.Status = n.Status.Equals(SystemParam.ACTIVE) ? 1 : 0;
                data.Content = n.Content;
                return data;
            }
            catch
            {
                return new NewsDetailOutputModel();
            }
        }

        //Cập nhật bài viết
        public JsonResultModel UpdateNews(int id, int type, bool status, string content, string title, string summary, string img)
        {
            try
            {
                News n = cnn.News.Find(id);
                n.Type = type;
                n.Status = status;
                n.Content = content;
                n.Title = title;
                n.Summary = summary;
                n.ImageUrl = img;
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch
            {
                return rp.serverError();
            }
        }

        //Xóa bài viết
        public JsonResultModel DelNews(int id)
        {
            try
            {
                News n = cnn.News.Find(id);
                n.IsActive = SystemParam.NO_ACTIVE;
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch
            {
                return rp.serverError();
            }
        }


        // web
        public IPagedList<ListNewsOutputModel> SearchNewsWeb(int page, string searchKey ,int type = SystemParam.NEWS_TYPE)
        {
            try
            {
                var data = cnn.News.Where(n => n.IsActive.Equals(SystemParam.ACTIVE) 
                    && (!String.IsNullOrEmpty(searchKey) ? n.Title.Contains(searchKey) : true)
                    &&  n.Status.Equals(SystemParam.ACTIVE)
                   )
                    .Select(n => new ListNewsOutputModel()
                    {
                        ID = n.ID,
                        Title = n.Title,
                        Status = n.Status,
                        Type = n.Type,
                        ImgUrl = n.ImageUrl,
                        CateName = n.CategoryNew.Name,
                        CreatedDate = n.CreatedDate
                    }).OrderByDescending(n => n.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch
            {
                return new List<ListNewsOutputModel>().ToPagedList(1, 1);
            }
        }
        // danh sách màn home
        public List<ListNewsOutputModel> ListNewsWeb(int limit = 3)
        {
            try
            {
                var data = cnn.News.Where(n => n.IsActive.Equals(SystemParam.ACTIVE)
                    && n.Status.Equals(SystemParam.ACTIVE)
                    && n.Type.Equals(SystemParam.PROMOTION_TYPE))
                    .Select(n => new ListNewsOutputModel()
                    {
                        ID = n.ID,
                        Title = n.Title,
                        Status = n.Status,
                        ImgUrl = n.ImageUrl,
                        CateName = n.CategoryNew.Name,
                        CreatedDate = n.CreatedDate
                    }).OrderByDescending(n => n.ID).Take(limit).ToList();
                return data;
            }
            catch
            {
                return new List<ListNewsOutputModel>();
            }
        }

    }
}