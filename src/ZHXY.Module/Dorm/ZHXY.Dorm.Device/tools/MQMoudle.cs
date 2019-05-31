namespace ZHXY.Dorm.Device.tools
{
    public class MQMoudle
    {
        public string clientType { get; set; } = "WINPC"; // 电脑名称
        public string clientMac { get; set; } = "30:9c:23:79:40:08"; //电脑mac地址
        public string clientPushId { get; set; } = ""; //
        public string project { get; set; } = "PSDK"; //
        public string method { get; set; } = "BRM.Config.GetMqConfig"; //方法名
        public data data { get; set; } = new data(); //Json串

    }

    public class data
    {
        public string optional { get; set; } //url信息
    }

    public class SurveyMoudle
    {
        public string[] channelId { get; set; } //通道列表
        public string code { get; set; } //学工号 （非必填）
        public string name { get; set; }  //姓名
        public int sex { get; set; } //性别 1男 2女
        public string idCode { get; set; }  //身份证号
        public string photoBase64 { get; set; } //Base64图片
        public string initialTime { get; set; }  //有效日期
        public string expireTime { get; set; } //失效日期
        public int personId { get; set; } //人员ID
    }
}
