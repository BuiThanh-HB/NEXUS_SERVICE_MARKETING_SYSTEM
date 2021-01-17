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
        public static string ADMIN = "Login";//Session admin
        public static string CLIENT = "Client";//Session người dùng

        //config email
        public const string HOST_DEFAULT = "smtp.gmail.com";
        public const string EMAIL_CONFIG = "nexussystem123@gmail.com";
        public const string PASS_CONFIG = "johbktjcpplkpvmo";
        public const string PASS_EMAIL = "123456aA@";
        public const string HEAD_EMAIL = "NEXUS SYSTEM";


        public const string CONVERT_DATETIME = "dd/MM/yyyy";
        public const string CONVERT_DATETIME_HAVE_HOUR = "dd/MM/yyyy HH:mm";
        public const int MAX_ROW_IN_LIST = 20;
        public const bool ACTIVE = true;
        public const bool NO_ACTIVE = false;


        public const string TOKEN_INVALID = "Phiên đăng nhập hết hạn";
        public const string TOKEN_NOT_FOUND = "Mời bạn đăng nhập";
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

        //Các type tin tức
        public const int NEWS_TYPE = 1;//Tin tức
        public const int EVENT_TYPE = 2;//Sự kiện
        public const int PROMOTION_TYPE = 3;//Khuyến mại

        //Các role user admin
        public const int ROLE_ADMIN = 1;//Role admin
        public const int ROLE_CLERK = 2;//Nhân viên kế toán
        public const int ROLE_TECHNICAL_STAFF = 3;//Nhân viên kỹ thuật

        //Status order
        public const int CANCEL = 0;//Trạng thái bị từ chối
        public const int PENDING = 1;//Trạng thái chờ xử lý
        public const int ACCEPT = 2;//Trạng thái được xác nhận
        public const int COMPLETE = 3;//Trạng thái hoàn thành

        //Status service of customer
        public const int NO_ACTIVE_STATUS = 0;//Trạng thái ngừng hoạt động
        public const int ACTIVE_STATUS = 1;//Trạng thái hoạt động
        public const int REQUEST_DEACTIVE = 2;

        public const int WRONG_PASSWORD = 2;
        public const int COUNT_LIST_WEB = 8;
        
    }
}
