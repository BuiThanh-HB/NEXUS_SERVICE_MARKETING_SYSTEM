﻿using Data.DB;
using Data.Utils;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using APIProject.Models;

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

        //Tìm kiếm thông tin gói cước
        public IPagedList<ListServicePlanOutputModel> Search(int page, string searchKey, Boolean? status, int? cateID, string fromDate, string toDate)
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);
                var data = cnn.ServicePlans.Where(s => s.IsActive.Equals(SystemParam.ACTIVE) && (!String.IsNullOrEmpty(searchKey) ? s.Name.Contains(searchKey) : true)
                && (fd.HasValue ? s.CreatedDate >= fd.Value : true) && (td.HasValue ? s.CreatedDate <= td.Value : true)
                && (status.HasValue ? s.Status.Equals(status.Value) : true) && (cateID.HasValue && cateID.Value > 0 ? s.CategoryID.Equals(cateID.Value) : true))
                    .Select(s => new ListServicePlanOutputModel
                    {
                        ID = s.ID,
                        Name = s.Name,
                        ImageUrl = s.ImageUrl,
                        CateName = s.Category.Name,
                        CreatedDate = s.CreatedDate,
                        Status = s.Status,
                        Value = s.Value,
                        Price = s.Price,
                        Descreiption = s.Description,
                        CateID = s.CategoryID
                    }).OrderByDescending(s => s.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch (Exception e)
            {
                e.ToString();
                return new List<ListServicePlanOutputModel>().ToPagedList(1, 1);
            }
        }

        //Thêm gói cước

        public JsonResultModel AddServicePlan(ListServicePlanOutputModel dt)
        {
            try
            {
                ServicePlan check = cnn.ServicePlans.Where(sv => sv.IsActive.Equals(SystemParam.ACTIVE) && sv.Name.Equals(dt.Name) && sv.CategoryID.Equals(dt.CateID)).FirstOrDefault();

                if (check != null)
                    return rp.response(SystemParam.ERROR, SystemParam.CODE_EXISTING, SystemParam.ERROR_MESSAGE_SERVICE_PLAN_EXISTING, "");
                ServicePlan s = new ServicePlan();
                s.Name = dt.Name;
                s.Status = dt.Status;
                s.Price = dt.Price;
                s.IsActive = SystemParam.ACTIVE;
                s.CreatedDate = DateTime.Now;
                s.Description = dt.Descreiption;
                s.ImageUrl = dt.ImageUrl;
                s.CategoryID = dt.CateID;
                s.Value = dt.Value;
                cnn.ServicePlans.Add(s);
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");

            }
            catch
            {
                return rp.serverError();
            }
        }


        //Cập nhật thông tin gói cước
        public JsonResultModel UpdateServicePlan(ListServicePlanOutputModel dt)
        {
            try
            {
                ServicePlan check = cnn.ServicePlans.Where(sv => sv.IsActive.Equals(SystemParam.ACTIVE) && sv.Name.Equals(dt.Name) && sv.CategoryID.Equals(dt.CateID) && !sv.ID.Equals(dt.ID)).FirstOrDefault();

                if (check != null)
                    return rp.response(SystemParam.ERROR, SystemParam.CODE_EXISTING, SystemParam.ERROR_MESSAGE_SERVICE_PLAN_EXISTING, "");
                ServicePlan s = cnn.ServicePlans.Find(dt.ID);
                s.Name = dt.Name;
                s.CategoryID = dt.CateID;
                s.Price = dt.Price;
                s.Status = dt.Status;
                s.Value = dt.Value;
                s.Description = dt.Descreiption;
                s.ImageUrl = dt.ImageUrl;
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch
            {
                return rp.serverError();
            }
        }

        //Xóa gói cước
        public JsonResultModel DelSercicePlan(int id)
        {
            try
            {
                ServicePlan s = cnn.ServicePlans.Find(id);
                s.IsActive = SystemParam.NO_ACTIVE;
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch
            {
                return rp.serverError();
            }
        }

        // tìm kiếm web
        public IPagedList<ListServicePlanOutputModel> SearchFontEnd(int page = 1, string searchKey = "",int status = 1, int cateID = 0)
        {
            try
            {
                var data = cnn.ServicePlans.Where(s => s.IsActive.Equals(SystemParam.ACTIVE))
                    .Select(s => new ListServicePlanOutputModel
                    {
                        ID = s.ID,
                        Name = s.Name,
                        ImageUrl = s.ImageUrl,
                        CateName = s.Category.Name,
                        CreatedDate = s.CreatedDate,
                        Status = s.Status,
                        Value = s.Value,
                        Price = s.Price,
                        Descreiption = s.Description,
                        CateID = s.CategoryID,
                        CateType = s.Category.Type
                    }).ToList();

                if(cateID > 0)
                {
                    data = data.Where(si => si.CateID.Equals(cateID)).ToList();
                }

                if (!String.IsNullOrEmpty(searchKey))
                {
                    data = data.Where(se => se.Name.Contains(searchKey)).ToList();
                }

                return data.OrderByDescending(sr => sr.ID).ToPagedList(page, SystemParam.COUNT_LIST_WEB);
            }
            catch (Exception e)
            {
                e.ToString();
                return new List<ListServicePlanOutputModel>().ToPagedList(1, 1);
            }
        }
        
        public ListServicePlanOutputModel ServiceDetail(int ID)
        {
            try
            {
                var data = cnn.ServicePlans.Where(s => s.ID.Equals(ID))
                    .Select(s => new ListServicePlanOutputModel
                    {
                        ID = s.ID,
                        Name = s.Name,
                        ImageUrl = s.ImageUrl,
                        CateName = s.Category.Name,
                        CreatedDate = s.CreatedDate,
                        Status = s.Status,
                        Value = s.Value,
                        Price = s.Price,
                        Descreiption = s.Description,
                        CateID = s.CategoryID,
                        CateType = s.Category.Type
                    }).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                e.ToString();
                return new ListServicePlanOutputModel();
            }
        }

        // danh sách gói dịch vụ của người dungf
        public IPagedList<ListServicePlanOutputModel> SearchMyService(int CusID, int page = 1, string searchKey = "", int cateID = 0)
        {
            try
            {
                var data = cnn.ServicePlans.Where(s => s.IsActive.Equals(SystemParam.ACTIVE)
                    && (!String.IsNullOrEmpty(searchKey) ? s.Name.Contains(searchKey) : true)
                    && s.Status.Equals(SystemParam.ACTIVE)
                    && (cateID > 0 ? s.CategoryID.Equals(cateID) : true))
                    .Select(s => new ListServicePlanOutputModel
                    //.Select(s => new Custom
                    {
                        ID = s.ID,
                        Name = s.Name,
                        ImageUrl = s.ImageUrl,
                        CateName = s.Category.Name,
                        CreatedDate = s.CreatedDate,
                        Status = s.Status,
                        Value = s.Value,
                        Price = s.Price,
                        Descreiption = s.Description,
                        CateID = s.CategoryID
                    })
                    .OrderByDescending(s => s.ID).ToList()
                    .ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch (Exception e)
            {
                e.ToString();
                return new List<ListServicePlanOutputModel>().ToPagedList(1, 1);
            }
        }

        // 
        public List<ListServicePlanOutputModel> ListServiceHome(int limit = 4)
        {
            try
            {
                var data = cnn.ServicePlans.Where(s => s.IsActive.Equals(SystemParam.ACTIVE) && s.Status.Equals(SystemParam.ACTIVE))
                    .Select(s => new ListServicePlanOutputModel
                    {
                        ID = s.ID,
                        Name = s.Name,
                        ImageUrl = s.ImageUrl,
                        CateName = s.Category.Name,
                        CreatedDate = s.CreatedDate,
                        Status = s.Status,
                        Value = s.Value,
                        Price = s.Price,
                        Descreiption = s.Description,
                        CateID = s.CategoryID
                    }).OrderByDescending(s => s.ID).Take(limit).ToList();
                return data;
            }
            catch (Exception e)
            {
                e.ToString();
                return new List<ListServicePlanOutputModel>();
            }
        }

        public int CountServicePlan()
        {
            return cnn.ServicePlans.Where(c => c.IsActive.Equals(SystemParam.ACTIVE)).Count();
        }
    }
}