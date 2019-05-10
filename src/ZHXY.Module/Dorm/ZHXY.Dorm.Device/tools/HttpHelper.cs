using System;
using System.IO;
using System.Net;
using System.Text;

namespace ZHXY.Dorm.Device.tools
{
    public class HttpHelper
    {
        public static string ExecutePost(string URI, string Param, string token)
        {
            try
            {
                var myRequest = HttpWebRequest.Create(URI) as HttpWebRequest;
                myRequest.Method = "POST";
                myRequest.ContentType = "application/json;charset=UTF-8";
                myRequest.ReadWriteTimeout = 30000;
                if(token != null){ myRequest.Headers.Add("X-Subject-Token", token);}
                byte[] data = Encoding.UTF8.GetBytes(Param);
                myRequest.ContentLength = data.Length;
                var myStream = myRequest.GetRequestStream();
                myStream.Write(data, 0, data.Length);
                myStream.Close();
                var myResponse = myRequest.GetResponse() as HttpWebResponse;
                var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string res = sr.ReadToEnd();
                return res;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                int statucCode = (int) rsp.StatusCode;
                if(statucCode == 401)
                {
                    var sr = new StreamReader(rsp.GetResponseStream(), Encoding.UTF8);
                    string res = sr.ReadToEnd();
                    return res;
                }
                return null;
            }
        }

        public static string ExecuteGetPersons(string URI, PersonMoudle personMoudle, string token)
        {
            string url = returnParam(URI, personMoudle);
            try
            {
                var myRequest = HttpWebRequest.Create(url) as HttpWebRequest;
                myRequest.Method = "GET";
                myRequest.ContentType = "application/json;charset=UTF-8";
                myRequest.ReadWriteTimeout = 30000;
                if (token != null) { myRequest.Headers.Add("Cookie", "JSESSIONID=636972042564F947376441F073B8160D;token=" + token); }
                var myResponse = myRequest.GetResponse() as HttpWebResponse;
                var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string res = sr.ReadToEnd();
                return res;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                int statucCode = (int)rsp.StatusCode;
                if (statucCode == 401)
                {
                    var sr = new StreamReader(rsp.GetResponseStream(), Encoding.UTF8);
                    string res = sr.ReadToEnd();
                    return res;
                }
                return null;
            }
        }

        public static string ExecutePostMachineInfo(string URI, string Param, string token)
        {
            try
            {
                var myRequest = HttpWebRequest.Create(URI) as HttpWebRequest;
                myRequest.Method = "POST";
                myRequest.ContentType = "application/json;charset=UTF-8";
                myRequest.ReadWriteTimeout = 30000;
                if (token != null) { myRequest.Headers.Add("Cookie", "JSESSIONID=636972042564F947376441F073B8160D;token=" + token); }
                byte[] data = Encoding.UTF8.GetBytes(Param);
                myRequest.ContentLength = data.Length;
                var myStream = myRequest.GetRequestStream();
                myStream.Write(data, 0, data.Length);
                myStream.Close();

                var myResponse = myRequest.GetResponse() as HttpWebResponse;
                var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string res = sr.ReadToEnd();
                return res;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                int statucCode = (int)rsp.StatusCode;
                if (statucCode == 401)
                {
                    var sr = new StreamReader(rsp.GetResponseStream(), Encoding.UTF8);
                    string res = sr.ReadToEnd();
                    return res;
                }
                return null;
            }
        }

        public static string GetDormitorInfo(string url,string name, string pid, string token)
        {
            try
            {
                var strBuild = new StringBuilder(url + "?");
                if(name != null && name.Length != 0)
                {
                    strBuild.Append("name=" + name + "&");
                }
                if(pid != null && pid.Length != 0)
                {
                    strBuild.Append("pid=" + pid + "&");
                }
                url = strBuild.ToString().Substring(0, strBuild.ToString().Length - 1);
                var myRequest = HttpWebRequest.Create(url) as HttpWebRequest;
                myRequest.Method = "GET";
                myRequest.ContentType = "application/json;charset=UTF-8";
                myRequest.ReadWriteTimeout = 30000;
                if (token != null) { myRequest.Headers.Add("Cookie", "JSESSIONID=636972042564F947376441F073B8160D;token=" + token); }
                var myResponse = myRequest.GetResponse() as HttpWebResponse;
                var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string res = sr.ReadToEnd();
                return res;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                int statucCode = (int)rsp.StatusCode;
                if (statucCode == 401)
                {
                    var sr = new StreamReader(rsp.GetResponseStream(), Encoding.UTF8);
                    string res = sr.ReadToEnd();
                    return res;
                }
                return null;
            }
        }

        /// <summary>
        ///     修改信息  PUT请求
        /// </summary>
        /// <param name="URI"></param>
        /// <param name="Param"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static string ExecutePut(string URI, string Param, string token)
        {
            try
            {
                var myRequest = HttpWebRequest.Create(URI) as HttpWebRequest;
                myRequest.Method = "PUT";
                myRequest.ContentType = "application/json;charset=UTF-8";
                myRequest.ReadWriteTimeout = 30000;
                if (token != null) { myRequest.Headers.Add("Cookie", "JSESSIONID=636972042564F947376441F073B8160D;token=" + token); }
                byte[] data = Encoding.UTF8.GetBytes(Param);
                myRequest.ContentLength = data.Length;
                var myStream = myRequest.GetRequestStream();
                myStream.Write(data, 0, data.Length);
                myStream.Close();
                var myResponse = myRequest.GetResponse() as HttpWebResponse;
                var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string res = sr.ReadToEnd();
                return res;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 删除信息 DELETE请求
        /// </summary>
        /// <param name="URI"></param>
        /// <param name="Param"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static string ExecuteDelete(string URI, string Param, string token)
        {
            try
            {
                var myRequest = HttpWebRequest.Create(URI) as HttpWebRequest;
                myRequest.Method = "DELETE";
                myRequest.ContentType = "application/json;charset=UTF-8";
                myRequest.ReadWriteTimeout = 30000;
                if (token != null){myRequest.Headers.Add("Cookie", "JSESSIONID=636972042564F947376441F073B8160D;token=" + token); }
                byte[] data = Encoding.UTF8.GetBytes(Param);
                myRequest.ContentLength = data.Length;
                var myStream = myRequest.GetRequestStream();
                myStream.Write(data, 0, data.Length);
                myStream.Close();
                var myResponse = myRequest.GetResponse() as HttpWebResponse;
                var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string res = sr.ReadToEnd();
                return res;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 导入人员照片信息（ZIP压缩文件）和人员基本信息（Excel表格）
        /// </summary>
        /// <param name="URI">上传的接口地址</param>
        /// <param name="cookieContainer">JESSIONID和TOKEN信息</param>
        /// <param name="FilePath">待上传的文件路径 .zip/.excel </param>
        /// <returns></returns>
        public static string UploadFileToDH(string URI, string token, string filePath)
        {
            try
            {
                var myRequest = HttpWebRequest.Create(URI) as HttpWebRequest;
                myRequest.Method = "POST";
                myRequest.ReadWriteTimeout = 1000 * 60 * 60;
                if (token != null) {myRequest.Headers.Add("Cookie", "JSESSIONID=636972042564F947376441F073B8160D;token=" + token); }
                myRequest.AllowAutoRedirect = true;
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                myRequest.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");

                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                int pos = filePath.LastIndexOf("\\");
                string fileName = filePath.Substring(pos + 1);
                var sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));

                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();
                var postStream = myRequest.GetRequestStream();
                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(bArr, 0, bArr.Length);
                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();
                var myResponse = myRequest.GetResponse() as HttpWebResponse;
                var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string res = sr.ReadToEnd();
                return res;
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                Console.WriteLine(message);
                return null;
            }
        }

        /// <summary>
        /// 组合Get请求的参数
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="personMoudle"></param>
        /// <returns></returns>
        public static string returnParam(string Url, PersonMoudle personMoudle)
        {
            var Param = new StringBuilder("?");
            if (personMoudle.code != null && personMoudle.code.Length != 0)
            {
                Param.Append("code=" + personMoudle.code + "&");
            }
            if (personMoudle.name != null && personMoudle.name.Length != 0)
            {
                Param.Append("name=" + personMoudle.name + "&");
            }
            if (personMoudle.contactNum != null && personMoudle.contactNum.Length != 0)
            {
                Param.Append("contactNum=" + personMoudle.contactNum + "&");
            }
            if (personMoudle.dormitoryCode != null && personMoudle.dormitoryCode.Length != 0)
            {
                Param.Append("dormitoryCode=" + personMoudle.dormitoryCode + "&");
            }
            if (personMoudle.dormitoryFloor != null && personMoudle.dormitoryFloor.Length != 0)
            {
                Param.Append("dormitoryFloor=" + personMoudle.dormitoryFloor + "&");
            }
            if (personMoudle.dormitoryRoom != null && personMoudle.dormitoryRoom.Length != 0)
            {
                Param.Append("dormitoryRoom=" + personMoudle.dormitoryRoom + "&");
            }
            if (personMoudle.dormitoryBed != null && personMoudle.dormitoryBed.Length != 0)
            {
                Param.Append("dormitoryBed=" + personMoudle.dormitoryBed + "&");
            }
            if (personMoudle.colleageCode != null && personMoudle.colleageCode.Length != 0)
            {
                Param.Append("colleageCode=" + personMoudle.colleageCode + "&");
            }
            if (personMoudle.colleageMajor != null && personMoudle.colleageMajor.Length != 0)
            {
                Param.Append("colleageMajor=" + personMoudle.colleageMajor + "&");
            }
            if (personMoudle.colleageGrade != null && personMoudle.colleageGrade.Length != 0)
            {
                Param.Append("colleageGrade=" + personMoudle.colleageGrade + "&");
            }
            if (personMoudle.colleageClass != null && personMoudle.colleageClass.Length != 0)
            {
                Param.Append("colleageClass=" + personMoudle.colleageClass + "&");
            }
            if (personMoudle.orgId != null && personMoudle.orgId.Length != 0)
            {
                Param.Append("orgId=" + personMoudle.orgId + "&");
            }
            return Url + Param.ToString().Substring(0, Param.ToString().Length - 1);
        }
    }
}