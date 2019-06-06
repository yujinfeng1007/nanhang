namespace ZHXY.Common
{
    /// <summary>
    /// 正则验证规则
    /// </summary>
    public class RegularRule
    {
        /// <summary>
        /// 手机号正则字符串
        /// <author>yujinfeng</author>
        /// </summary>
        public static string PHONE_NUMBER { get; } = @"^(0|86|17951)?(13[0-9]|14[57]|15[012356789]|166|17[3678]|18[0-9])[0-9]{8}$";

        /// <summary>
        /// Email正则字符串
        /// </summary>
        public static string EMAIL { get; } = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// URL正则字符串
        /// </summary>
        public static string URL { get; } = @"^https?:\/\/(([a-zA-Z0-9_-])+(\.)?)*(:\d+)?(\/((\.)?(\?)?=?&?[a-zA-Z0-9_-](\?)?)*)*$";

        /// <summary>
        /// 日期正则字符串(yyyy-MM-dd,yyyy/MM/dd,yyyy.MM.dd,yyyy_MM_dd)
        /// </summary>
        public static string DATE { get; } = @"^[1-2][0-9][0-9][0-9][-/._][0-1]{0,1}[0-9][-/._][0-3]{0,1}[0-9]$";

        /// <summary>
        /// 中国身份证正则字符串
        /// </summary>
        public static string CHINESE_ID_CARD { get; } = @"^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$";

        /// <summary>
        /// UserAgent移动设备
        /// </summary>
        public static string UA_MOBILE { get; } = @"(nokia|iphone|android|ipad|motorola|^mot\-|softbank|foma|docomo|kddi|up\.browser|up\.link|htc|dopod|blazer|netfront|helio|hosin|huawei|novarra|CoolPad|webos|techfaith|palmsource|blackberry|alcatel|amoi|ktouch|nexian|samsung|^sam\-|s[cg]h|^lge|ericsson|philips|sagem|wellcom|bunjalloo|maui|symbian|smartphone|midp|wap|phone|windows ce|iemobile|^spice|^bird|^zte\-|longcos|pantech|gionee|^sie\-|portalmmm|jig\s browser|hiptop|^ucweb|^benq|haier|^lct|opera\s*mobi|opera\*mini|320x320|240x320|176x220)";

     
    }
}