//====================================================================
// 文件名称：DateHelper.cs
// 项目名称：常用方法实用工具集
// ===================================================================

using System;
using System.Collections.Generic;

namespace ZHXY.Common
{
    public class DateHelper
    {
        #region 返回本月有多少天

        /// <summary>
        ///     本月有多少天
        /// </summary>
        /// <param name="year"> 年 </param>
        /// <param name="month"> 月 </param>
        /// <returns> 天数 </returns>
        public static int GetDaysOfMonth(int year, int month)
        {
            var days = 0;
            switch (month)
            {
                case 2:
                    days = IsRuYear(year) ? 29 : 28;
                    break;

                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    days = 31;
                    break;

                case 4:
                case 6:
                case 9:
                case 11:
                    days = 30;
                    break;
            }

            return days;
        }

        #endregion 返回本月有多少天

        #region 返回本月有多少天

        /// <summary>
        ///     本月有多少天
        /// </summary>
        /// <param name="dt"> 日期 </param>
        /// <returns> 天数 </returns>
        public static int GetDaysOfMonth(DateTime dt) => GetDaysOfMonth(dt.Year, dt.Month);

        #endregion 返回本月有多少天

        #region 返回当前日期的星期编号

        /// <summary>
        ///     返回当前日期的星期编号
        /// </summary>
        public static string GetWeekNumberOfDay(DateTime idt)
        {
            var week = "";

            var dt = idt.DayOfWeek.ToString();
            switch (dt)
            {
                case "Mondy":
                    week = "1";
                    break;

                case "Tuesday":
                    week = "2";
                    break;

                case "Wednesday":
                    week = "3";
                    break;

                case "Thursday":
                    week = "4";
                    break;

                case "Friday":
                    week = "5";
                    break;

                case "Saturday":
                    week = "6";
                    break;

                case "Sunday":
                    week = "7";
                    break;
            }

            return week;
        }

        #endregion 返回当前日期的星期编号

        #region 判断当前年份是否是闰年，私有函数

        /// <summary>
        ///     判断当前年份是否是闰年，私有函数
        /// </summary>
        private static bool IsRuYear(int year) => year % 400 == 0 || (year % 4 == 0 && year % 100 != 0);

        #endregion 判断当前年份是否是闰年，私有函数

        #region 获取两个日期之间的差值 可返回年 月 日 小时 分钟 秒

        /// <summary>
        ///     获取两个日期之间的差值
        /// </summary>
        /// <param name="howtocompare"> 比较的方式可为：year month day hour minute second </param>
        /// <param name="startDate">    开始日期 </param>
        /// <param name="endDate">      结束日期 </param>
        /// <returns> 时间差 </returns>
        public static double DateDiff(string howtocompare, DateTime startDate, DateTime endDate)
        {
            double diff = 0;
            try
            {
                var TS = new TimeSpan(endDate.Ticks - startDate.Ticks);

                switch (howtocompare.ToLower())
                {
                    case "year":
                        diff = Convert.ToDouble(TS.TotalDays / 365);
                        break;

                    case "month":
                        diff = Convert.ToDouble(TS.TotalDays / 365 * 12);
                        break;

                    case "day":
                        diff = Convert.ToDouble(TS.TotalDays);
                        break;

                    case "hour":
                        diff = Convert.ToDouble(TS.TotalHours);
                        break;

                    case "minute":
                        diff = Convert.ToDouble(TS.TotalMinutes);
                        break;

                    case "second":
                        diff = Convert.ToDouble(TS.TotalSeconds);
                        break;
                }
            }
            catch (Exception)
            {
                diff = 0;
            }

            return diff;
        }

        #endregion 获取两个日期之间的差值 可返回年 月 日 小时 分钟 秒

        #region 获取本周的开始日期 星期的的日期

        /// <summary>
        ///     获取本周的开始日期(星期一)
        /// </summary>
        /// <returns>  </returns>
        public static DateTime GetStartDateOfWeek()
        {
            var now = DateTime.Now.Date;
            return now.AddDays(-(int)now.DayOfWeek + 1);
        }

        #endregion 获取本周的开始日期 星期的的日期

        #region 将时间转换为CronExpression格式 此方法仅限于"hh:mm:ss"格式的字符串。返回每天定时格式。该方法还存在扩展空间
        /// <summary>
        /// 将时间转换为CronExpression格式 此方法仅限于"hh:mm:ss"格式的字符串。返回每天定时格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string TimeToQuartzCron(string time)
        {
            if (string.IsNullOrEmpty(time)) return "";
            var error = "传入的时间:" + time + "有误";
            var span = time.Trim().Split(':');
            var hh = Convert.ToInt32(span[0]);
            var mm = Convert.ToInt32(span[1]);
            var ss = Convert.ToInt32(span[2]);
            if (hh > 24) throw new Exception(error);
            if (mm > 59) throw new Exception(error);
            if (ss > 59) throw new Exception(error);
            string cronExpression = ss + " " + mm + " " + hh + " ? * *";
            return cronExpression;
        }
        #endregion

        #region 获取日期的一部分，返回部分字符串。支持年，月，日，时，分，秒
        /// <summary>
        /// 获取日期的一部分，返回部分字符串。支持年，月，日，时，分，秒
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetDateTimePart(string where,DateTime datetime)
        {
            switch (where.Trim().ToLower())
            {
                case "year": { return datetime.Year.ToString(); }
                case "month": { return datetime.Month.ToString(); }
                case "day": { return datetime.Day.ToString(); }
                case "hour": { return datetime.Hour.ToString(); }
                case "minute": { return datetime.Minute.ToString(); }
                case "second": { return datetime.Second.ToString(); }
                default: { return ""; }
            }
        }
        #endregion

        #region 判断一个时间段是否跨年，跨月，跨日
        /// <summary>
        /// 判断一个时间段是否跨年，跨月，跨日
        /// </summary>
        public static bool IsExtendIntoNext(string range, DateTime startTime, DateTime endTime) => !string.Equals(GetDateTimePart(range.Trim().ToLower(), startTime), GetDateTimePart(range.Trim().ToLower(), endTime));
        #endregion

        #region 南航系统 - 访客功能，通过时间类型，返回两个时间点

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Time_Type">时间类型：
        ///                             0 = 未选择时间（默认今日）,
        ///                             1 = 今日，
        ///                             2 = 昨天，
        ///                             3 = 本周，
        ///                             4 = 本月，
        ///                             5 = 上周，
        ///                             6 = 上月，
        ///                             7 = 其他时间，需要手动筛选时间段
        /// <returns></returns>
        public static Dictionary<string, string> GetDateTimes(int TimeType)
        {
            string startTime = null, endTime = null;
            switch (TimeType)
            {
                case 0: startTime = DateTime.Today.ToString(); endTime = DateTime.Now.AddDays(1).Date.ToString(); break;
                case 1: startTime = DateTime.Today.ToString(); endTime = DateTime.Now.AddDays(1).Date.ToString(); break;
                case 2: startTime = DateTime.Today.AddDays(-1).ToString(); endTime = DateTime.Today.AddSeconds(-1).ToString(); break;
                case 3: startTime = DateTime.Today.AddDays(-(int)DateTime.Now.Date.DayOfWeek + 1).ToString(); endTime = DateTime.Today.AddDays(-(int)DateTime.Now.Date.DayOfWeek + 8).AddSeconds(-1).ToString(); break;
                case 4: startTime = DateTime.Today.AddDays(-(int)DateTime.Now.Date.Day + 1).ToString(); endTime = DateTime.Today.AddDays(-(int)DateTime.Now.Day + 1).AddMonths(1).AddSeconds(-1).ToString(); break;
                case 5: startTime = DateTime.Today.AddDays(-(int)DateTime.Now.Date.DayOfWeek + 1 - 7).ToString(); endTime = DateTime.Today.AddDays(-(int)DateTime.Now.Date.DayOfWeek + 8 - 7).AddSeconds(-1).ToString(); break;
                case 6: startTime = DateTime.Today.AddMonths(-1).AddDays(-(int)DateTime.Now.Date.Day + 1).ToString(); endTime = DateTime.Today.AddDays(-(int)DateTime.Now.Day + 1).AddSeconds(-1).ToString(); break;
                default: break;
            }
            var hashtable = new Dictionary<string, string>();
            hashtable.Add("startTime", startTime);
            hashtable.Add("endTime", endTime);
            return hashtable;
        }
        #endregion

        #region 获取本周/本月的开始时间和截止时间 yujinfeng

        /// <summary>
        ///     获取本周的开始时间
        /// </summary>
        /// <returns>  </returns>
        public static DateTime GetStartTimeOfWeek()
        {
            var now = DateTime.Today;
            return now.AddDays(-(int)now.DayOfWeek + 1);
        }

        /// <summary>
        ///     获取本周的结束时间
        /// </summary>
        public static DateTime GetEndTimeOfWeek()
        {
            var now = DateTime.Today;
            return now.AddDays(7 - (int)now.DayOfWeek + 1).AddSeconds(-1);
        }

        /// <summary>
        ///     获取本月的开始时间
        /// </summary>
        public static DateTime GetStartTimeOfMonth()
        {
            var now = DateTime.Today;
            return now.AddDays(-now.Day + 1);
        }

        /// <summary>
        ///     获取本月的结束时间
        /// </summary>
        public static DateTime GetEndTimeOfMonth()
        {
            var now = DateTime.Today;
            return now.AddMonths(1).AddDays(-now.AddMonths(1).Day + 1).AddSeconds(-1);
        }

        #endregion 获取本周的开始日期 星期的的日期

        #region 将时间戳转换为C# DateTime格式 时间戳格式为十三位，不够后面补0
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timespan">13位整数型timestamp</param>
        /// <returns></returns>
        public static DateTime GetTime(string timestamp)
        {
            var StartDateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var timespan = timestamp.Trim();
            var length = timespan.Length;
            if (string.IsNullOrEmpty(timespan)) { return DateTime.Now; }
            if (timespan.Length < 13)
            {
                for (int i = 0; i < 13 - length; i++)
                {
                    timespan = timespan + "0";
                }
            }
            try
            {
                long lTime = long.Parse(timespan) * 10000L;
                var toNow = new TimeSpan(lTime);
                return StartDateTime.Add(toNow);
            }
            catch
            {
                return DateTime.Now;
            }
        }
        #endregion

        #region 将C# DateTime 转换为时间戳
        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name=”time”></param>
        /// <returns></returns>
        public static long ConvertDateTimeInt(DateTime time)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(time - startTime).Ticks / 10000;
        }
        #endregion
    }
}