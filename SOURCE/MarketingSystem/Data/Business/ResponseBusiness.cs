using APIProject.Models;
using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Business
{
    public class ResponseBusiness
    {
        public JsonResultModel response(int status, int code, string message, object data)
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = status;
            result.Code = code;
            result.Message = message;
            result.Data = data;
            return result;
        }

        public JsonResultModel serverError()
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = SystemParam.ERROR;
            result.Code = SystemParam.CODE_ERROR;
            result.Message = SystemParam.SERVER_ERROR;
            result.Data = "";
            return result;
        }
    }
}