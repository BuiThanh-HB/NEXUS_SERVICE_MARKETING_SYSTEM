using Data.Business;
using Data.DB;
using Data.Model.APIWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Utils
{


    public class SystemParam : GenericBusiness
    {


        public const int ERROR = 0;
        public const int SUCCESS = 1;


        public const string CONVERT_DATETIME = "dd/MM/yyyy";
        public const string CONVERT_DATETIME_HAVE_HOUR = "dd/MM/yyyy HH:mm";
        public const int MAX_ROW_IN_LIST = 20;
        public const bool ACTIVE = true;
        public const bool NO_ACTIVE = false;


        public const string TOKEN_INVALID = "Phiên đăng nhập hết hạn";
        public const string TOKEN_NOT_FOUND = "Token not found";
        public const string SERVER_ERROR = "Hệ thống đang bảo trì!";
        public const string SUCCESS_MESSAGE = "Thành công";
        public const string ERROR_MESSAGE = "Thất bại";
        public const string ERROR_MESSAGE_LOGIN_FAIL = "Tài khoản hoặc mật khẩu không đúng";
        public const string ERROR_MESSAGE_CHECK_PASS_FAIL = "Mật khẩu cũ không chính xác";
        public const string ERROR_MESSAGE_CATE_EXISTING = "Tên danh mục đã tồn tại";
        public const string ERROR_MESSAGE_SERVICE_PLAN_EXISTING = "Gói cước đã tồn tại trên hệ thống";


        public const int ERROR_PASSWORD = 2;
        public const int ERROR_EXIST_EMAIL = 10;
        public const int EXIST = 2;

        public const int NOT_EXIST = -1;
        public const int USER_EXIST = 2;

        //login
        public const int FAIL_LOGIN = 2;
        public const int FAIL = 501;



        //Code define
        public const int CODE_ERROR = 500;
        public const int SUCCESS_CODE = 200;
        public const int CODE_EXISTING = 502;

        //Các type của gói cước
        public const string FOR_DIAL = "D";//Để quay số 
        public const string CONNECT_PHONE_ONLY = "T";//Chỉ kết nối điện thoại
        public const string BROADBAND = "B";//Cho băng thông rộng
    }
}
