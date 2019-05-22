namespace ZHXY.Dorm.Device.tools
{
    /// <summary>
    /// 常量类
    /// </summary>
    public class Constants
    {
        public static string DH_IP = "210.35.32.30";
        public static string REQUEST_URL_PORT = "http://"+ DH_IP + ":9080";
        public static string REQUEST_AUTHENTICATION_URI = "http://"+ DH_IP + ":80";
        public static string DAHUA_LOGIN_USERNAME = "system";//登录大华设备的用户名（大华提供）
        public static string DAHUA_LOGIN_PASSWORD = "admin123";//登录大华设备的用户密码（大华提供）
        public static string AUTHENTICATION_URI = REQUEST_AUTHENTICATION_URI + "/admin/API/accounts/authorize"; //鉴权的请求地址
        public static string UPDATE_TOKEN_URI = REQUEST_AUTHENTICATION_URI + "/admin/API/accounts/updateToken"; //更新令牌和取消授权请求地址
        public static string CREATE_STUDENTS_INFO = REQUEST_URL_PORT + "/pims/person/third"; //创建人员信息(可自动创建宿舍)
        public static string UPDATE_STUDENTS_INFO = REQUEST_URL_PORT + "/pims/person/id"; //修改人员信息(不可自动创建宿舍)
        public static string SELECT_STUDENTS_INFO = REQUEST_URL_PORT + "/pims/persons"; //查询人员信息
        public static string UPLOAD_PHOTO_ZIP_URI = REQUEST_URL_PORT + "/pims/batchPhoto"; // 导入人员照片 （.ZIP文件）
        public static string UPLOAD_TEACHER_EXCEL_URI = REQUEST_URL_PORT + "/pims/defaultUploadPerson?cid=1"; // 导入教师人员信息excel
        public static string UPLOAD_STUDENT_EXCEL_URI = REQUEST_URL_PORT + "/pims/uploadPerson?cid=1"; // 导入学生人员信息excel
        public static string GET_MACHINE_INFO_URI = REQUEST_URL_PORT + "/fas/dormitory/getDeviceTree"; //获取设备信息
        public static string GET_MQ_CONFIG_URI = REQUEST_URL_PORT + "/admin/API/BRM/Config/GetMqConfig"; //获取MQ配置
        public static string CREATE_DORMITORY_INFO_URI = REQUEST_URL_PORT + "/pims/dormitory/info"; //创建宿舍信息
        public static string SELECT_DORMITORY_INFOS_URI = REQUEST_URL_PORT + "/pims/dromitory/infos"; //查看宿舍信息
        public static string ADD_VISIT_SURVEY_URL = REQUEST_URL_PORT + "/pims/door/tempSurvey"; //布控（访客相关）
        public static string CALCLE_VISIT_SURVEY_URL = REQUEST_URL_PORT + "/pims/door/cancerSurvey"; //撤控 （访客相关）
    }
}