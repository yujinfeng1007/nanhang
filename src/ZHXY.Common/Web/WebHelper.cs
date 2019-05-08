using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ZHXY.Common
{
    public class WebHelper
    {
        public static string GetResponse(string url, object parameters, bool isPost = false,
            string contentType = "application/x-www-form-urlencoded")
        {
            return Send_V2(url, parameters, Encoding.GetEncoding("utf-8"), isPost, contentType);
        }

        public static string SendRequest(string url, bool isPost = false)
        {
            return Send(url, string.Empty, Encoding.GetEncoding("utf-8"), isPost);
        }

        public static string SendRequest(string url, string parameters, bool isPost = false,
            string contentType = "application/x-www-form-urlencoded")
        {
            return Send(url, parameters, Encoding.GetEncoding("utf-8"), isPost, contentType);
        }

        public static string GetString(string requestUri, Dictionary<string, string> parameters)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ContentType", "text/html;charset=UTF-8");
            if (parameters != null)
            {
                var strParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                requestUri = string.Concat(requestUri, '?', strParam);
            }
            return client.GetStringAsync(requestUri).Result;
        }


        #region private



        private static string Send(string url, string parameters, Encoding encoding, bool isPost = false,  string contentType = "application/x-www-form-urlencoded", CookieContainer cookie = null, int timeout = 120000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.CookieContainer = cookie;
            if (isPost)
            {
                if ("application/json".Equals(contentType))
                {
                    var dic = new Dictionary<string, string>();
                    var paramArry = parameters.Replace("&amp;", "&").Split('&');
                    for (var i = 0; i < paramArry.Length; i++)
                    {
                        var Arry = paramArry[i].Split('=');
                        dic.Add(Arry[0], Arry[1]);
                    }

                    parameters = dic.ToJson();
                }

                var postData = encoding.GetBytes(parameters);
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = postData.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
            }

            var response = (HttpWebResponse)request.GetResponse();
            string result;
            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                    return string.Empty;
                using (var reader = new StreamReader(stream, encoding))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        private static string Send_V2(string url, object parameters, Encoding encoding, bool isPost = false,string contentType= "application/x-www-form-urlencoded")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 120000;
            request.CookieContainer = null;
            if (isPost)
            {
                var postData = encoding.GetBytes(parameters.ToJson());
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = postData.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
            }

            var response = (HttpWebResponse)request.GetResponse();
            string result;
            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                    return string.Empty;
                using (var reader = new StreamReader(stream, encoding))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        #endregion
    }
}