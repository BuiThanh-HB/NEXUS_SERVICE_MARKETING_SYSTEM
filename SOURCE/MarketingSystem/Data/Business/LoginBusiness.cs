using Data.DB;
using Data.Utils;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Data.Model.APIWeb;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static Data.Utils.SystemParam;
using APIProject.Models;

namespace Data.Business
{
    public class LoginBusiness : GenericBusiness
    {
        ResponseBusiness rp = new ResponseBusiness();
        public LoginBusiness(ThisDataEntities context = null) : base()
        {

        }

        public JsonResultModel LoginWeb(string account, string password)
        {
            try
            {
                var user = cnn.Users.Where(u => u.IsActive.Equals(ACTIVE) && u.Phone.Equals(account) || u.Name.Equals(account));
                if (user.Count() > 0)
                {
                    User us = user.FirstOrDefault();
                    if (Util.CheckPass(password, us.Password))
                    {
                        string token = Util.CreateMD5(DateTime.Now.ToString());
                        LoginOutputModel data = new LoginOutputModel();
                        data.Account = us.Name;
                        data.Name = us.Name;
                        data.Role = us.Role;
                        data.Id = us.ID;
                        data.Token = token;
                        us.Token = token;
                        cnn.SaveChanges();
                        HttpContext.Current.Session[Sessions.LOGIN] = data;
                        return rp.response(SUCCESS, SUCCESS_CODE, SUCCESS_MESSAGE, "");
                    }
                    else
                    {
                        return rp.response(FAIL_LOGIN, FAIL, ERROR_MESSAGE_LOGIN_FAIL, "");
                    }

                }
                return rp.response(FAIL_LOGIN, FAIL, ERROR_MESSAGE_LOGIN_FAIL, "");
            }
            catch (Exception e)
            {
                e.ToString();
                return rp.serverError();
            }
        }


        public int? checkTokenUser(string token)
        {
            try
            {
                int? id = cnn.Users.Where(u => u.IsActive.Equals(ACTIVE) && u.Token.Equals(token)).Select(u => u.ID).FirstOrDefault();
                return id;
            }
            catch (Exception e)
            {
                e.ToString();
                return ERROR;
            }
        }

    }
}
