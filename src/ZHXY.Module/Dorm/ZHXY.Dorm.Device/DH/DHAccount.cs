using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using ZHXY.Common;
using ZHXY.Dorm.Device.tools;
using HttpHelper = ZHXY.Dorm.Device.tools.HttpHelper;

namespace ZHXY.Dorm.Device.DH
{
    public class DHAccount
    {
        public static string REDIS_TOKEN_SET_KEY = "huawang_token_redis_key"; //Token存储Redis的key
        public static int REDIS_LINE_RECORD_DB_LEVEL = 15;
        public static string X_SUBJECT_TOKEN = null;

        /// <summary>
        /// 取消授权 （登出操作）
        /// </summary>
        public static object DHLogOut()
        {
            if(X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            string cookieStr = "X-Subject-Token="+ X_SUBJECT_TOKEN;
            var DataDic = new Dictionary<string, string>();
            DataDic.Add("userName", Constants.DAHUA_LOGIN_USERNAME);
            DataDic.Add("token", X_SUBJECT_TOKEN);
            return HttpHelper.ExecutePost(Constants.UPDATE_TOKEN_URI, JsonConvert.SerializeObject(DataDic), cookieStr);
        }

          /*****************************        定时任务调用方法：推送人员信息至大华闸机         ********************************/
        /// <summary>
        /// 人员信息  => 推送人员信息至大华
        /// </summary>
        public static object PUSH_DH_ADD_PERSON(PersonMoudle personMoudle)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }

            if (personMoudle.photoUrl != null && personMoudle.photoUrl.Length != 0)
            {
                personMoudle.photoBase64 = GetImageBase64Str.ImageBase64Str(personMoudle.photoUrl); //通过头像地址，获取头像Base64位字符串
            }
            return HttpHelper.ExecutePostMachineInfo(Constants.CREATE_STUDENTS_INFO + "?sessionId=" + X_SUBJECT_TOKEN, JsonConvert.SerializeObject(personMoudle), X_SUBJECT_TOKEN);
        }

        /// <summary>
        /// 人员信息 => 修改人员信息（大华）
        /// </summary>
        /// <param name="personMoudle"></param>
        public static object PUSH_DH_UPDATE_PERSON(PersonMoudle personMoudle)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            if (personMoudle.photoUrl != null && personMoudle.photoUrl.Length != 0 && personMoudle.photoBase64 != null && personMoudle.photoBase64.Length != 0)
            {
                personMoudle.photoBase64 = GetImageBase64Str.ImageBase64Str(personMoudle.photoUrl);//通过头像地址，获取头像Base64位字符串
            }
            return HttpHelper.ExecutePut(Constants.CREATE_STUDENTS_INFO + "/"+personMoudle.code + "?sessionId=" + X_SUBJECT_TOKEN, JsonConvert.SerializeObject(personMoudle), X_SUBJECT_TOKEN);
        }

        /// <summary>
        /// 人员信息 => 删除人员信息（大华）
        /// </summary>
        /// <param name="personMoudle"></param>
        public static object PUSH_DH_DELETE_PERSON(string[] Ids)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            return HttpHelper.ExecuteDelete(Constants.SELECT_STUDENTS_INFO+ "?sessionId=" + X_SUBJECT_TOKEN, JsonConvert.SerializeObject(Ids), X_SUBJECT_TOKEN);
        }


        /// <summary>
        /// 查询大华设备的用户信息
        /// </summary>
        /// <param name="personMoudle"></param>
        /// <returns></returns>
        public static object SELECT_DH_PERSON(PersonMoudle personMoudle)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            return HttpHelper.ExecuteGetPersons(Constants.SELECT_STUDENTS_INFO, personMoudle, X_SUBJECT_TOKEN);
        }

        /*****************************        接口调用方法：宿舍相关         ********************************/
        /// <summary>
        /// 创建宿舍
        /// </summary>
        /// <param name="name">宿舍名称</param>
        /// <param name="pid">上级ID</param>
        /// <returns></returns>
        public static object CREATE_DORMITORY(String name, string pid, int level)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            var dicObj = new Dictionary<string, object>();
            var dic = new Dictionary<string, string>();
            dic.Add("name", name);
            dic.Add("pname", pid);
            dic.Add("level", level.ToString());
            dicObj.Add("clientPushId", "20171113");
            dicObj.Add("clientType", "pc-web");
            dicObj.Add("project", "mms");
            dicObj.Add("data", dic);
            return HttpHelper.ExecutePostMachineInfo(Constants.CREATE_DORMITORY_INFO_URI+ "?sessionId="+X_SUBJECT_TOKEN, JsonConvert.SerializeObject(dicObj), X_SUBJECT_TOKEN);
        }

        /// <summary>
        /// 获取宿舍信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static object SELECT_DORMITOR(string name, string pid)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            return HttpHelper.GetDormitorInfo(Constants.SELECT_DORMITORY_INFOS_URI, name, pid, X_SUBJECT_TOKEN);
        }

        /*****************************        接口调用方法：推送人员信息至大华闸机         ********************************/
        /// <summary>
        /// 导入人员照片（ZIP文件）
        /// </summary>
        /// <param name="zipFilePath"> 压缩多张图片之后的压缩包路径 </param>
        public static object PUSH_DH_BATCHPHOTO_ZIP(string zipFilePath)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }

            //var dire = new DirectoryInfo(zipFilePath);
            //foreach(var file in dire.GetFiles())
            //{
            //    HttpHelper.UploadFileToDH(Constants.UPLOAD_PHOTO_ZIP_URI, X_SUBJECT_TOKEN, file.FullName);
            //}
            HttpHelper.UploadFileToDH(Constants.UPLOAD_PHOTO_ZIP_URI, X_SUBJECT_TOKEN, zipFilePath);
            return null;
        }

        /// <summary>
        /// 导入人员信息 （学生Excel表格）
        /// </summary>
        /// <param name="excelFilePath">填入数据的Excel表格所在路径</param>
        public static void PUSH_DH_STUDENT_EXCEL(string excelFilePath)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            HttpHelper.UploadFileToDH(Constants.UPLOAD_STUDENT_EXCEL_URI, X_SUBJECT_TOKEN, excelFilePath);
        }

        /// <summary>
        /// 导入人员信息（教师Excel）
        /// </summary>
        /// <param name="excelFilePath"></param>
        public static void PUSH_DH_TEACHER_EXCEL(string excelFilePath)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            HttpHelper.UploadFileToDH(Constants.UPLOAD_TEACHER_EXCEL_URI, X_SUBJECT_TOKEN, excelFilePath);
        }

        /*****************************        接口调用方法：查询大华闸机的设备信息         ********************************/
        /// <summary>
        ///     获取大华的设备信息
        /// </summary>
        /// <param name="id">树节点ID</param>
        /// <param name="type">01;01,02,05,07_4,08_3,08_5,34,38;01@9,07,12</param>
        /// <param name="isDomain">0</param>
        /// <param name="searchKey">设备名称（模糊查询）</param>
        public static JObject GetMachineInfo(string id, string type, string isDomain, string searchKey)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            var DataDic = new Dictionary<string, string>();
            DataDic.Add("id", id);
            DataDic.Add("type", type);
            DataDic.Add("isDomain", isDomain);
            DataDic.Add("searchKey", searchKey);
            string response = HttpHelper.ExecutePostMachineInfo(Constants.GET_MACHINE_INFO_URI, JsonConvert.SerializeObject(DataDic), X_SUBJECT_TOKEN);
            return (JObject)JsonConvert.DeserializeObject(response);
        }

        /// <summary>
        /// MQ 请求参数对象
        /// </summary>
        /// <param name="mqMoudle"></param>
        public static JObject GetMQConfig(MQMoudle mqMoudle)
        {
            if (X_SUBJECT_TOKEN == null)
            {
                var db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
                X_SUBJECT_TOKEN = db.StringGet(REDIS_TOKEN_SET_KEY);
            }
            mqMoudle.data.optional = Constants.GET_MQ_CONFIG_URI + "?token=" + X_SUBJECT_TOKEN;
            string response = HttpHelper.ExecutePostMachineInfo(mqMoudle.data.optional, JsonConvert.SerializeObject(mqMoudle), X_SUBJECT_TOKEN);
            var jo = (JObject)JsonConvert.DeserializeObject(response);
            int code = jo.Value<int>("code"); //返回码
            string desc = jo.Value<string>("desc"); //结果描述  Success
            string userName = jo["data"]["userName"].ToString(); //用户名
            string addr = jo["data"]["addr"].ToString(); //MQ地址
            string password = jo["data"]["password"].ToString(); //密码
            return jo;
        }
    }
}
