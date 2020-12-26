using Data.DB;
using Data.Utils;
using Data.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIProject.Models;

namespace Data.Business
{
    public class CategoryBusiness : GenericBusiness
    {
        ResponseBusiness rp = new ResponseBusiness();
        public CategoryBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }

        public IPagedList<ListCategoryOutputModel> Search(int page, string searchKey, string fromDate, string toDate, string type)
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);

                var data = cnn.Categories.Where(c => c.IsActive.Equals(SystemParam.ACTIVE)
                && (!String.IsNullOrEmpty(searchKey) ? c.Name.Contains(searchKey) : true) && (fd.HasValue ? c.CreatedDate >= fd.Value : true)
                && (td.HasValue ? c.CreatedDate <= td.Value : true) && (!String.IsNullOrEmpty(type) ? c.Type == type : true))
                    .Select(c => new ListCategoryOutputModel
                    {
                        ID = c.ID,
                        Name = c.Name,
                        Type = c.Type,
                        CreatedDate = c.CreatedDate
                    }).OrderByDescending(c => c.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch (Exception e)
            {
                e.ToString();
                return new List<ListCategoryOutputModel>().ToPagedList(1, 1);
            }
        }

        //Thêm danh mục gói cước
        public JsonResultModel AddCategory(string name, string type)
        {
            try
            {
                Category check = cnn.Categories.Where(ct => ct.IsActive.Equals(SystemParam.ACTIVE) && ct.Name.Equals(name) && ct.Type.Equals(type)).FirstOrDefault();
                if (check != null)
                    return rp.response(SystemParam.ERROR, SystemParam.CODE_EXISTING, SystemParam.ERROR_MESSAGE_CATE_EXISTING, "");
                Category c = new Category();
                c.Name = name;
                c.Type = type;
                c.IsActive = SystemParam.ACTIVE;
                c.CreatedDate = DateTime.Now;
                cnn.Categories.Add(c);
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch
            {
                return rp.serverError();
            }
        }

        //Sửa danh mục
        public JsonResultModel UpdateCategory(int id, string name, string type)
        {
            try
            {
                Category check = cnn.Categories.Where(ct => ct.IsActive.Equals(SystemParam.ACTIVE) && ct.Name.Equals(name) && ct.Type.Equals(type)).FirstOrDefault();
                if (check != null)
                    return rp.response(SystemParam.ERROR, SystemParam.CODE_EXISTING, SystemParam.ERROR_MESSAGE_CATE_EXISTING, "");
                Category c = cnn.Categories.Find(id);
                c.Name = name;
                c.Type = type;
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch(Exception e)
            {
                e.ToString();
                return rp.serverError();
            }
        }

        //Xóa danh mục

        public JsonResultModel DelCategory(int id)
        {
            try
            {
                Category c = cnn.Categories.Find(id);
                c.IsActive = SystemParam.NO_ACTIVE;
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch
            {
                return rp.serverError();
            }

        }
    }
}