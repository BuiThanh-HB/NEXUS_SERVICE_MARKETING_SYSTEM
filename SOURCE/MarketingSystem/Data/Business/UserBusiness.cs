using APIProject.Models;
using Data.DB;
using Data.Model;
using Data.Model.APIWeb;
using Data.Utils;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Business
{
    public class UserBusiness : GenericBusiness
    {
        ResponseBusiness rp = new ResponseBusiness();
        public UserBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }

        public JsonResultModel ChangePassUser(string OldPass, string NewPass)
        {
            try
            {
                LoginOutputModel session = (LoginOutputModel)HttpContext.Current.Session[SystemParam.ADMIN];
                User u = cnn.Users.Find(session.Id);
                if (Util.CheckPass(OldPass, u.Password))
                {
                    u.Password = Util.GenPass(NewPass);
                    cnn.SaveChanges();
                    return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
                }
                return rp.response(SystemParam.ERROR, SystemParam.FAIL, SystemParam.ERROR_MESSAGE_CHECK_PASS_FAIL, "");
            }
            catch
            {
                return rp.serverError();
            }
        }

        public IPagedList<UserDetailOutputModel> Search(int page, string searchKey, string fromDate, string toDate)
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);

                var data = cnn.Users.Where(u => u.IsActive.Equals(SystemParam.ACTIVE) && (fd.HasValue ? u.CreatedDate >= fd.Value : true)
                && (td.HasValue ? u.CreatedDate <= td.Value : true) && (!String.IsNullOrEmpty(searchKey) ? u.Name.Contains(searchKey) || u.Phone.Contains(searchKey) : true))
                    .Select(u => new UserDetailOutputModel
                    {
                        ID = u.ID,
                        UserName = u.Name,
                        Phone = u.Phone,
                        CreateDate = u.CreatedDate,
                        role = u.Role

                    }).OrderByDescending(u => u.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);

                return data;
            }
            catch
            {
                return new List<UserDetailOutputModel>().ToPagedList(1, 1);
            }

        }

        public JsonResultModel AddUser(string userName, string userPhone, string password, int role)
        {
            try
            {
                User checkExistUser = cnn.Users.Where(user => user.IsActive.Equals(SystemParam.ACTIVE) && (user.Name.Equals(userName) || user.Phone.Equals(userPhone))).FirstOrDefault();
                if (checkExistUser != null)
                    return rp.response(SystemParam.ERROR, SystemParam.FAIL, "Tên người dùng hoặc tài khoản đã tồn tại", "");
                User u = new User();
                u.Name = userName;
                u.Phone = userPhone;
                u.Role = role;
                u.Token = "";
                u.CreatedDate = DateTime.Now;
                u.Password = Util.GenPass(password);
                u.IsActive = SystemParam.ACTIVE;
                cnn.Users.Add(u);
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch
            {
                return rp.serverError();
            }
        }

        public JsonResultModel UpdateUserInfo(int id, string userName, string userPhone, string password, int role)
        {
            try
            {
                User checkExistUser = cnn.Users.Where(user => user.IsActive.Equals(SystemParam.ACTIVE) && !user.ID.Equals(id) && (user.Name.Equals(userName) || user.Phone.Equals(userPhone))).FirstOrDefault();
                if (checkExistUser != null)
                    return rp.response(SystemParam.ERROR, SystemParam.FAIL, "Tên người dùng hoặc tài khoản đã tồn tại", "");
                User u = cnn.Users.Find(id);
                u.Name = userName;
                u.Phone = userPhone;
                u.Role = role;
                u.Password = !String.IsNullOrEmpty(password) ? Util.GenPass(password) : u.Password;
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch
            {
                return rp.serverError();
            }
        }

        public JsonResultModel DeleteUser(int id)
        {
            try
            {
                User u = cnn.Users.Find(id);
                u.IsActive = SystemParam.NO_ACTIVE;
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