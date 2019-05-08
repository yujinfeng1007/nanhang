using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class Result
    {
        public string F_Student { get; set; }
        public string F_Name { get; set; }
        public string F_HeadPic { get; set; }
        public string F_CheckType { get; set; }
        public string F_CheckStatus { get; set; }
        public int F_Rank { get; set; }
        public string F_Time { get; set; }

    }

    public class AttendanceRuleRepository : Data.Repository<AttendanceRule>, IAttendanceRuleRepository
    {
        //计算
        /// <summary>
        ///     根据时间算在哪个时间规则里
        ///     算出结果,迟到，早退，请假
        /// </summary>
        /// <param name="e"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        private AttendanceStuCount stat(AttendanceStuCount e, AttendanceRule rule, TimeSpan timeSpan)
        {
            var dayOfWeek = DateHelper.GetWeekNumberOfDay(DateTime.Now);
            //var ifReturn = true; //是否结束算法
            var time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                       timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            //是否符合规则

            #region 规则1校验

            //规则1校验
            if (rule.F_Start1 < rule.F_End1 && rule.F_AttendanceTime1.IndexOf(dayOfWeek) != -1)
            {
                ifUpdate = false;

                var F_Start1_DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                        rule.F_Start1.Hours, rule.F_Start1.Minutes, rule.F_Start1.Seconds);

                var F_End1_DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                       rule.F_End1.Hours, rule.F_End1.Minutes, rule.F_End1.Seconds);

                //提前签到有效时间
                var F_StartHead1 = F_Start1_DateTime.Add(new TimeSpan(0, -(int)rule.F_Ahead1, 0));

                //滞后签退有效时间
                var F_EndLater1 = F_End1_DateTime.Add(new TimeSpan(0, (int)rule.F_Later1, 0));

                //中间时间
                var F_Mid1 = F_Start1_DateTime.Add(new TimeSpan((F_End1_DateTime - F_Start1_DateTime).Ticks / 2));

                //早上打卡时间
                if (rule.F_Start1 != TimeSpan.Zero && !rule.F_Ahead1.IsEmpty() && F_Mid1 >= time &&
                        time >= F_StartHead1 && e.F_Qj == 0)
                {
                    e.F_Start1_Check = DateTime.Now;
                    e.F_Start1 = F_Start1_DateTime;

                    //签到成功后，所有打卡不计算
                    if ("1".Equals(e.F_Start1_Result))
                    {
                        result = "打卡成功";
                    }
                    else
                    {
                        //正常时间段
                        if (F_Start1_DateTime >= time &&
                            time >= F_StartHead1)
                        {
                            result = "签到成功";
                            e.F_Start1_Result = "1";
                            e.F_Qk = 0;
                            ifUpdate = true;
                        }
                        //迟到时间段
                        else
                        {
                            result = "迟到";
                            if (!"3".Equals(e.F_Start1_Result))
                            {
                                e.F_Start1_Result = "3";
                                e.F_Cd++;
                                ifUpdate = true;
                                e.F_Qk = 0;
                            }
                        }
                    }
                }
                //下午打卡时间
                else if (F_Mid1 < time && time < F_EndLater1 && e.F_Qj == 0)
                {
                    //签退成功，所有打卡不计算
                    if ("1".Equals(e.F_End1_Result))
                    {
                        result = "打卡成功";
                    }
                    else
                    {
                        e.F_End1_Check = DateTime.Now;
                        e.F_End1 = F_End1_DateTime;

                        //比较签退
                        if (F_Mid1 < time && time < F_End1_DateTime)
                        {
                            result = "早退";
                            ifUpdate = true;
                            if (!"4".Equals(e.F_End1_Result))
                            {
                                e.F_End1_Result = "4";
                                e.F_Zt++;
                                e.F_Qk2 = 0;
                            }
                        }
                        //正常签退
                        else
                        {
                            e.F_End1_Result = "1";
                            result = "签退成功";
                            ifUpdate = true;
                            e.F_Qk2 = 0;
                            //重置早退
                            if (e.F_Zt > 0)
                                e.F_Zt = 0;
                        }
                    }
                }
                else
                {
                    result = "打卡成功";
                }
            }

            #endregion 规则1校验

            #region 规则2校验

            //if (ifReturn) return e;
            //ifUpdate = true;
            ////规则2校验
            //if (rule.F_Start2 < rule.F_End2 && rule.F_AttendanceTime2.IndexOf(dayOfWeek) != -1)
            //{
            //    //提前签到有效时间
            //    TimeSpan F_StartHead2 = rule.F_Start2.Add(new TimeSpan(0, -(int)rule.F_Ahead2, 0));
            //    //滞后签退有效时间
            //    TimeSpan F_EndLater2 = rule.F_End2.Add(new TimeSpan(0, (int)rule.F_Later2, 0));

            //    DateTime F_Start2_DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            //            rule.F_Start2.Hours, rule.F_Start2.Minutes, rule.F_Start2.Seconds);

            //    DateTime F_End2_DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            //           rule.F_End2.Hours, rule.F_End2.Minutes, rule.F_End2.Seconds);

            //    if (e.F_Start2_Result.IsEmpty() && e.F_End2_Result.IsEmpty())
            //    {
            //        e.F_Start2_Check = DateTime.Now;
            //        e.F_Start2 = F_Start2_DateTime;
            //        if (rule.F_Start2 != TimeSpan.Zero && !rule.F_Ahead2.IsEmpty() && rule.F_Start2 >= time &&
            //            time >= F_StartHead2)
            //        {
            //            result = "签到成功";
            //            e.F_Start2_Result = "1";
            //        }
            //        else
            //        {
            //            e.F_Start2_Result = "5"; //无效签到打卡
            //            result = "打卡成功";
            //            ifUpdate = true;
            //        }
            //    }
            //    //早上签过,只检查截止
            //    else if (!e.F_Start2_Result.IsEmpty() && e.F_End2_Result.IsEmpty())
            //    {
            //        e.F_End2_Check = DateTime.Now;
            //        e.F_End2 = F_End2_DateTime;

            //        if (rule.F_Start2 > time && "1".Equals(e.F_Start2_Result))
            //        {
            //            result = "打卡成功";
            //        }
            //        else if (rule.F_Start2!= TimeSpan.Zero && !rule.F_Ahead2.IsEmpty() &&
            //                 rule.F_Start2 >= time && !"1".Equals(e.F_Start2_Result))
            //        {
            //            e.F_Start2_Result = "1";
            //            result = "签到成功";
            //            ifUpdate = true;
            //        }
            //        else if (rule.F_End2 != TimeSpan.Zero && !rule.F_Later2.IsEmpty() && rule.F_End2 <= time &&
            //                 time <= F_EndLater2)
            //        {
            //            e.F_End2_Result = "2";
            //            result = "签退成功";
            //            ifUpdate = true;
            //        }
            //        //else if (time < rule.F_End1 && time > rule.F_Start1)
            //        //{
            //        //    e.F_End1_Result = "4";
            //        //    result = "早退";
            //        //    ifUpdate = true;
            //        //    e.F_Zt = "1";
            //        //}
            //        else
            //        {
            //            ////如果有规则2，则规则一设置缺卡 下放至规则2 否则 无效签到打卡
            //            //if (e.F_End1_Result.IsEmpty() && rule.F_Start2 != TimeSpan.Zero && !rule.F_Ahead2.IsEmpty() &&
            //            //    rule.F_Start2.Add(new TimeSpan(-(int)rule.F_Ahead2, 0, 0)) > time)
            //            //{
            //            //    e.F_End1_Result = "6"; //缺卡
            //            //    result = "无效签到";
            //            //    ifUpdate = true;
            //            //    ifReturn = false;
            //            //    e.F_Qk = "1";
            //            //}
            //            //else
            //            //{
            //            //    e.F_End1_Result = "5"; //无效签到打卡
            //            //    result = "无效签到";
            //            //    ifUpdate = true;
            //            //}
            //            e.F_End2_Result = "5"; //无效签到打卡
            //            result = "打卡成功";
            //            ifUpdate = true;

            //        }
            //    }
            //    //都刷过 重复截止
            //    else if (!e.F_Start2_Result.IsEmpty() && !e.F_End2_Result.IsEmpty())
            //    {
            //        e.F_End2_Result = "5"; //无效签到打卡
            //        result = "打卡成功";
            //        ifUpdate = true;
            //    }
            //}

            #endregion 规则2校验

            #region 规则3校验

            //if (ifReturn) return e;
            ////规则3校验
            //if (e.F_Start3_Result.IsEmpty() && e.F_End3_Result.IsEmpty())
            //{
            //    e.F_Start3_Check = DateTime.Now;
            //    e.F_Start3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, rule.F_Start3.Hours,
            //        rule.F_Start3.Minutes, rule.F_Start2.Seconds);
            //    if (rule.F_Start3 != TimeSpan.Zero && !rule.F_Ahead3.IsEmpty() && rule.F_Start3 >= time &&
            //        time >= rule.F_Start3.Add(new TimeSpan(-(int)rule.F_Ahead3, 0, 0)))
            //    {
            //        result = "签到成功";
            //        e.F_Start3_Result = "1";
            //    }
            //    else if (rule.F_Start3 < time && rule.F_End3 > time)
            //    {
            //        e.F_Start3_Result = "3";
            //        result = "迟到";
            //        e.F_Cd++;
            //    }
            //    else
            //    {
            //        //如果有规则2，则规则一设置缺卡 下放至规则2 否则 无效签到打卡
            //        if (e.F_End3_Result.IsEmpty())
            //        {
            //            e.F_Start3_Result = "6"; //缺卡
            //            result = "无效签到";
            //            ifUpdate = true;
            //            ifReturn = false;
            //            e.F_Qk++;
            //        }
            //        else
            //        {
            //            e.F_Start3_Result = "5"; //无效签到打卡
            //            result = "无效签到";
            //            ifUpdate = true;
            //        }
            //    }
            //}
            ////早上签过,只检查截止
            //else if (!e.F_Start3_Result.IsEmpty() && e.F_End3_Result.IsEmpty())
            //{
            //    e.F_End3_Check = DateTime.Now;
            //    e.F_End3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, rule.F_End3.Hours,
            //        rule.F_End3.Minutes, rule.F_End3.Seconds);

            //    if (rule.F_Start3 > time && "1".Equals(e.F_Start3_Result))
            //    {
            //        result = "已签到，重复打卡";
            //    }
            //    else if (rule.F_Start3 != TimeSpan.Zero && !rule.F_Ahead3.IsEmpty() &&
            //             rule.F_Start3.Add(new TimeSpan(-(int)rule.F_Ahead3, 0, 0)) <= time && rule.F_Start3 >= time &&
            //             !"1".Equals(e.F_Start3_Result))
            //    {
            //        e.F_Start3_Result = "1";
            //        result = "签到成功";
            //        ifUpdate = true;
            //    }
            //    else if (rule.F_End3 != TimeSpan.Zero && !rule.F_Later3.IsEmpty() && rule.F_End3 <= time &&
            //             time <= rule.F_End3.Add(new TimeSpan((int)rule.F_Later3, 0, 0)))
            //    {
            //        e.F_End3_Result = "1";
            //        result = "签到成功";
            //        ifUpdate = true;
            //    }
            //    else if (time < rule.F_End3 && time > rule.F_Start3)
            //    {
            //        e.F_End3_Result = "4";
            //        result = "早退";
            //        ifUpdate = true;
            //        e.F_Zt++;
            //    }
            //    else
            //    {
            //        //如果有规则2，则规则一设置缺卡 下放至规则2 否则 无效签到打卡
            //        if (e.F_End3_Result.IsEmpty())
            //        {
            //            e.F_End3_Result = "6"; //缺卡
            //            result = "无效签到";
            //            ifUpdate = true;
            //            ifReturn = false;
            //        }
            //        else
            //        {
            //            e.F_End3_Result = "5"; //无效签到打卡
            //            result = "无效签到";
            //            ifUpdate = true;
            //        }
            //    }
            //}
            ////都刷过 重复截止
            //else if (!e.F_Start3_Result.IsEmpty() && !e.F_End3_Result.IsEmpty())
            //{
            //    e.F_End3_Check = DateTime.Now;
            //    e.F_End3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, rule.F_End3.Hours,
            //        rule.F_End3.Minutes, rule.F_End3.Seconds);

            //    if (rule.F_End3 != TimeSpan.Zero && !rule.F_Later3.IsEmpty() && rule.F_End3 <= time &&
            //        time <= rule.F_End3.Add(new TimeSpan((int)rule.F_Later3, 0, 0)))
            //    {
            //        if ("1".Equals(e.F_End3_Result))
            //        {
            //            result = "已签到，重复打卡";
            //        }
            //        //原来是早退，更新为签到成功
            //        else if ("4".Equals(e.F_End3_Result))
            //        {
            //            e.F_End3_Result = "1";
            //            result = "签到成功";
            //            ifUpdate = true;
            //        }
            //    }
            //    else if (time < rule.F_End3 && time > rule.F_Start3)
            //    {
            //        result = "早退";
            //        ifUpdate = true;
            //        e.F_Zt ++;
            //    }
            //    else
            //    {
            //        //如果有规则2，则规则一设置缺卡 下放至规则2 否则 无效签到打卡 但不跟新结果
            //        if (rule.F_Start3 != TimeSpan.Zero && !rule.F_Ahead3.IsEmpty() &&
            //            rule.F_Start3.Add(new TimeSpan(-(int)rule.F_Ahead3, 0, 0)) > time)
            //        {
            //            //e.F_End1_Result = "6"; //缺卡
            //            result = "无效签到";
            //            //ifUpdate = true;
            //            ifReturn = false;
            //        }
            //        else
            //        {
            //            e.F_End2_Result = "5"; //无效签到打卡
            //            result = "无效签到";
            //            ifUpdate = true;
            //            ifReturn = false;
            //        }
            //    }
            //}

            #endregion 规则3校验

            return e;
        }

        //  走班签到计算
        /// <summary>
        ///     根据时间算在哪个时间规则里
        ///     算出结果.
        /// </summary>
        /// <returns></returns>
        private Dictionary<AttendanceStuCount, AttendanceStuFlow> statMoveClass(AttendanceStuCount e,
            AttendanceStuFlow flow, string F_TimeSpan, TimeSpan time)
        {
            //解析时间段
            if (!string.IsNullOrEmpty(F_TimeSpan))
            {
                var dic = new Dictionary<AttendanceStuCount, AttendanceStuFlow>();
                var start = TimeSpan.Parse(F_TimeSpan.Split('-')[0]);
                var end = TimeSpan.Parse(F_TimeSpan.Split('-')[1]);

                #region 规则校验

                //规则校验
                if (time <= start)
                {
                    result = "签到成功";
                }
                else if (time > start && time < end)
                {
                    result = "迟到";
                    e.F_Cd++;
                    flow.F_Cd = "1";
                }
                else
                {
                    result = "无效签到";
                    e.F_Qk++;
                    flow.F_Qk = "1";
                }

                dic.Add(e, flow);
                return dic;

                #endregion 规则校验
            }

            throw new Exception("参数错误");
        }

        private string getCheckType(AttendanceStuCount e)
        {
            var sb = new StringBuilder();
            //迟到
            if (e.F_Cd > 0) sb.Append("3");
            //早退
            if (e.F_Zt > 0) sb.Append("4");
            //缺卡1 6 缺卡2 8
            if (e.F_Qk > 0) sb.Append("6");

            if (e.F_Qk2 > 0) sb.Append("8");

            //请假 7
            if (e.F_Qj > 0) sb.Append("7");

            if ("".Equals(sb.ToString())) sb.Append("1");
            return sb.ToString();
        }

        private string getMemo(AttendanceStuFlow e, SchScheduleMoveCourse c, Classroom room)
        {
            var sb = new StringBuilder();
            var ext = c.Course.F_Name + " " + room.F_Building_No + "号楼" + room.F_Name + " " +
                      e.F_Time.ToString(@"hh\:mm\:ss");
            //迟到
            if ("1".Equals(e.F_Cd))
                sb.Append("走班迟到：" + ext);
            //缺卡
            else if ("1".Equals(e.F_Qk))
                sb.Append("走班缺卡：" + ext);
            else
                sb.Append("走班签到：" + ext);

            return sb.ToString();
        }

        private bool ifUpdate;
        private string result = "";
        //private string f_School_Id;



        public string MakeAttendanceLogs4EG(Student student, string F_Class, string F_Sn,
            string F_Course_PrepareID, string F_TimeSpan, string F_Pos)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var date = DateTime.Now;
                try
                {
                    if (!F_TimeSpan.IsEmpty())
                        date = DateTime.Parse(F_TimeSpan);
                }
                catch
                {
                    date = DateTime.Now;
                }

                //保存流水
                var flow = new AttendanceStuFlow();
                flow.Create(false);
                flow.F_Student = student.F_Id;
                flow.F_Date = date.Date;
                flow.F_Time = date.TimeOfDay;
                flow.F_Device = F_Sn;
                flow.F_Type = "2"; //刷卡
                flow.F_Source = "2"; //班牌
                //flow.F_Photo = F_Photo; //监控拍照
                flow.F_Course_PrepareID = F_Course_PrepareID; //备课ID
                flow.F_Cd = "0";
                flow.F_Zt = "0";
                flow.F_Qk = "0";
                flow.F_Pos = F_Pos;

                AttendanceStuCount e = null;

                //计算是否符合走班签到
                if (!string.IsNullOrEmpty(F_Course_PrepareID) && !string.IsNullOrEmpty(F_TimeSpan))
                {
                }
                else
                {
                    if (!F_Class.Equals(student.F_Class_ID)) throw new Exception("本班级中没有该学生");
                    //计算
                    var rule = db.FindEntity<AttendanceRule>(t =>
                        t.F_DepartmentId == student.F_Divis_ID && t.F_EnabledMark == true);
                    if (rule != null)
                    {
                        if (e == null)
                            e = db.FindEntity<AttendanceStuCount>(t =>
                                t.F_Student == student.F_Id && t.F_Date == flow.F_Date);
                        if (e == null)
                        {
                            e = new AttendanceStuCount();
                            e.Create(false);
                            e.F_Date = flow.F_Date;
                            e.F_Student = student.F_Id;
                            e.F_Cd = 0;
                            e.F_Zt = 0;
                            e.F_Qk = 0;
                            e.F_DeviceName = F_Sn;
                            e.F_Class = F_Class;
                            e = stat(e, rule, flow.F_Time);
                            e.F_CheckType = getCheckType(e);
                            db.Insert(e);
                        }
                        else
                        {
                            e = stat(e, rule, flow.F_Time);
                            e.F_CheckType = getCheckType(e);
                            if (ifUpdate)
                            {
                                e.Modify(e.F_Id, false);
                                db.Update(e);
                            }
                        }
                    }

                    flow.F_Memo = result;
                }
                //throw new Exception("没有设置考勤规则");

                //记录打卡结果
                //flow.F_Memo = result;
                db.Insert(flow);
                db.Commit();
            }

            return result;
        }

        public string MakeAttendanceLogs(string schoolCode, Student student, string F_Sn, string F_Class, string F_Photo,
            string F_Course_PrepareID, string F_TimeSpan)
        {
            using (var db = new Data.UnitWork(schoolCode).BeginTrans(schoolCode))
            {
                var device = db.FindEntity<ElectronicBoard>(t => t.F_Sn == F_Sn);
                if (device == null)
                    throw new Exception("没有这台设备");
                var date = DateTime.Now;
                try
                {
                    if (!F_TimeSpan.IsEmpty())
                        date = DateTime.Parse(F_TimeSpan);
                }
                catch
                {
                    date = DateTime.Now;
                }
                //保存流水
                var flow = new AttendanceStuFlow();
                flow.Create(false);
                flow.F_Student = student.F_Id;
                flow.F_Date = date.Date;
                flow.F_Time = date.TimeOfDay;
                flow.F_Device = device.F_Sn;
                flow.F_Type = "2"; //刷卡
                flow.F_Source = "2"; //班牌
                flow.F_Photo = F_Photo; //监控拍照
                flow.F_Course_PrepareID = F_Course_PrepareID; //备课ID
                flow.F_Cd = "0";
                flow.F_Zt = "0";
                flow.F_Qk = "0";

                AttendanceStuCount e = null;

                //计算是否符合走班签到
                if (!string.IsNullOrEmpty(F_Course_PrepareID) && !string.IsNullOrEmpty(F_TimeSpan))
                {
                    //课程ID校验
                    var c = db.FindEntity<SchScheduleMoveCourse>(t =>
                        t.F_Course_PrepareID == F_Course_PrepareID);
                    if (c == null || !c.F_IsMoveCourse.ToBool())
                    {
                        result = "无效课程，签到失败";
                        flow.F_Qk = "1";
                        flow.F_Memo = "走班签到无效，非走班课程";
                    }
                    else if (c.F_Classroom != device.F_Classroom)
                    {
                        result = "无效课程，请注意教室";
                        flow.F_Qk = "1";
                        flow.F_Memo = "走班签到无效，教室错误";
                    }
                    else
                    {
                        var room = db.FindEntity<Classroom>(t => t.F_Id == c.F_Classroom);
                        var s = db.FindEntity<Schedule_MoveClassStudent_Entity>(t =>
                            t.F_StudentId == student.F_Id && t.F_MoveClassId == c.F_Class);
                        if (s == null)
                        {
                            result = "无效课程，签到失败";
                            flow.F_Qk = "1";
                            flow.F_Memo = "走班签到无效,错误课程";
                        }
                        else
                        {
                            e = db.FindEntity<AttendanceStuCount>(t =>
                                t.F_Student == student.F_Id && t.F_Date == flow.F_Date);
                            if (e == null)
                            {
                                e = new AttendanceStuCount();
                                e.Create(false);
                                e.F_Date = flow.F_Date;
                                e.F_Student = student.F_Id;
                                e.F_Cd = 0;
                                e.F_Zt = 0;
                                e.F_Qk = 0;
                                e.F_DeviceName = device.F_Device_Name + "-" + device.F_Device_Code;
                                e.F_Class = student.F_Class_ID; //走班班级
                                var dic = statMoveClass(e, flow, F_TimeSpan, flow.F_Time);
                                e = dic.First().Key;
                                flow = dic.First().Value;
                                e.F_CheckType = getCheckType(e);
                                db.Insert(e);
                            }
                            else
                            {
                                var dic = statMoveClass(e, flow, F_TimeSpan, flow.F_Time);
                                e = dic.First().Key;
                                flow = dic.First().Value;
                                e.F_CheckType = getCheckType(e);
                                if (ifUpdate)
                                {
                                    e.Modify(e.F_Id, false);
                                    db.Update(e);
                                }
                            }

                            flow.F_Memo = getMemo(flow, c, room);
                        }
                    }
                }
                else
                {
                    if (!F_Class.Equals(student.F_Class_ID)) throw new Exception("本班级中没有该学生");
                    //计算
                    var rule = db.FindEntity<AttendanceRule>(t =>
                        t.F_DepartmentId == student.F_Divis_ID && t.F_EnabledMark == true);
                    if (rule != null)
                    {
                        if (e == null)
                            e = db.FindEntity<AttendanceStuCount>(t =>
                                t.F_Student == student.F_Id && t.F_Date == flow.F_Date);
                        if (e == null)
                        {
                            e = new AttendanceStuCount();
                            e.Create(false);
                            e.F_Date = flow.F_Date;
                            e.F_Student = student.F_Id;
                            e.F_Cd = 0;
                            e.F_Zt = 0;
                            e.F_Qk = 1; //早上打卡
                            e.F_Qk2 = 1; //下午打卡
                            e.F_Qj = 0; //早上请假
                            //e.F_Qj2 = 0; //下午请假
                            e.F_DeviceName = device.F_Device_Name + "-" + device.F_Device_Code;
                            e.F_Class = F_Class;
                            e = stat(e, rule, flow.F_Time);
                            e.F_CheckType = getCheckType(e);
                            db.Insert(e);
                        }
                        else
                        {
                            e = stat(e, rule, flow.F_Time);
                            e.F_CheckType = getCheckType(e);
                            if (ifUpdate)
                            {
                                e.Modify(e.F_Id, false);
                                db.Update(e);
                            }
                        }
                    }

                    flow.F_Memo = result;
                }
                //throw new Exception("没有设置考勤规则");

                //记录打卡结果
                //flow.F_Memo = result;
                db.Insert(flow);
                db.Commit();
            }

            return result;
        }

        public string MakeTeaAttendanceLogs(string schoolCode, Teacher teacher, string F_Sn, string F_Class, string F_Photo,
            string F_Course_PrepareID, string F_TimeSpan)
        {
            using (var db = new Data.UnitWork(schoolCode).BeginTrans(schoolCode))
            {
                var device = db.FindEntity<ElectronicBoard>(t => t.F_Sn == F_Sn);
                if (device == null)
                    throw new Exception("没有这台设备");
                var date = DateTime.Now;
                try
                {
                    if (!F_TimeSpan.IsEmpty())
                        date = DateTime.Parse(F_TimeSpan);
                }
                catch
                {
                    date = DateTime.Now;
                }
                //保存流水
                var flow = new AttendanceStuFlow();
                flow.Create(false);
                flow.F_Student = teacher.F_Id;
                flow.F_Date = date.Date;
                flow.F_Time = date.TimeOfDay;
                flow.F_Device = device.F_Sn;
                flow.F_Type = "2"; //刷卡
                flow.F_Source = "3"; //老师刷卡班牌
                flow.F_Photo = F_Photo; //监控拍照
                //记录打卡结果
                flow.F_Memo = result;
                db.Insert(flow);
                db.Commit();
            }

            return result;
        }

        public string GetMySignInfo(Student student, string F_Sn, string F_Date)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取总考勤统计
                var date = DateTime.Parse(F_Date);
                var d1 = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
                var tmp = d1.AddMonths(1).AddDays(-1);
                var d2 = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);
                var days = DateHelper.GetDaysOfMonth(date);
                var expression = ExtLinq.True<AttendanceStuCount>();
                expression = expression.And(t => t.F_Student == student.F_Id);
                expression = expression.And(t => t.F_CreatorTime >= d1);
                expression = expression.And(t => t.F_CreatorTime <= d2);
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();
                //获取考勤规则
                var rule = db.FindEntity<AttendanceRule>(t =>
                    t.F_DepartmentId == student.F_Divis_ID && t.F_EnabledMark == true);
                if (rule == null)
                    throw new Exception("没有设置考勤规则");
                //计算

                var re = new List<object>();
                //string[] blank = new string[1];
                for (var dayNo = 1; dayNo < days + 1; dayNo++)
                {
                    var today = new DateTime(date.Year, date.Month, dayNo);
                    if (today > DateTime.Now) break;
                    var e = list.Where(t => t.F_Date == today);
                    if (e.Count() == 1)
                    {
                        var sa = e.First();
                        var start = DateTime.Parse(((DateTime)sa.F_CreatorTime).ToString("yyyy-MM-dd") + " 00:00:00");
                        var end = DateTime.Parse(((DateTime)sa.F_CreatorTime).ToString("yyyy-MM-dd") + " 23:59:59");
                        //var times = db.IQueryable<AttendanceStuFlow>().
                        //    Where(t => t.F_CreatorTime >= start && t.F_CreatorTime <= end && t.F_Student == student.F_Id).ToList().Select(t => t.F_Time.ToString(@"hh\:mm\:ss"));
                        var flows = db.QueryAsNoTracking<AttendanceStuFlow>(t =>
                            t.F_CreatorTime >= start && t.F_CreatorTime <= end && t.F_Student == student.F_Id).ToList();
                        var times = new List<object>();
                        foreach (var flow in flows)
                        {
                            var o = new
                            {
                                F_CheckTime = flow.F_Time.ToString(@"hh\:mm\:ss"),
                                F_CheckInfo = flow.F_Memo
                            };
                            times.Add(o);
                        }

                        var res = new
                        {
                            F_CheckType = sa.F_CheckType,
                            F_DeviceName = sa.F_DeviceName,
                            F_CheckDate = ((DateTime)sa.F_CreatorTime).ToString("yyyy-MM-dd"),
                            F_CheckTime = times
                        };
                        re.Add(res);
                    }
                    else
                    {
                        var period = DateHelper.GetWeekNumberOfDay(new DateTime(date.Year, date.Month, dayNo));
                        //考勤规则 查看是否需要考勤
                        if (!rule.F_AttendanceTime1.IsEmpty() && rule.F_AttendanceTime1.IndexOf(period) == -1)
                        {
                        }
                        else if (!rule.F_AttendanceTime2.IsEmpty() && rule.F_AttendanceTime2.IndexOf(period) == -1)
                        {
                        }
                        else if (!rule.F_AttendanceTime3.IsEmpty() && rule.F_AttendanceTime3.IndexOf(period) == -1)
                        {
                        }
                        else
                        {
                            var res = new
                            {
                                F_CheckType = "2",
                                F_DeviceName = "",
                                F_CheckDate = new DateTime(date.Year, date.Month, dayNo).ToString("yyyy-MM-dd"),
                                F_CheckTime = "[]"
                            };
                            re.Add(res);
                        }
                    }
                }

                db.Commit();
                return re.ToJson();
            }
        }

        public string GetMySignInfoV2(Student student, string F_Sn, string F_Date)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取总考勤统计
                var date = DateTime.Parse(F_Date);
                var d1 = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
                var tmp = d1.AddMonths(1).AddDays(-1);
                var d2 = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);
                var days = DateHelper.GetDaysOfMonth(date);
                var expression = ExtLinq.True<AttendanceStuCount>();
                expression = expression.And(t => t.F_Student == student.F_Id);
                expression = expression.And(t => t.F_CreatorTime >= d1);
                expression = expression.And(t => t.F_CreatorTime <= d2);
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();
                //获取考勤规则
                var rule = db.FindEntity<AttendanceRule>(t =>
                    t.F_DepartmentId == student.F_Divis_ID && t.F_EnabledMark == true);
                if (rule == null)
                    throw new Exception("没有设置考勤规则");
                //计算

                var re = new List<object>();
                //string[] blank = new string[1];
                for (var dayNo = 1; dayNo < days + 1; dayNo++)
                {
                    var today = new DateTime(date.Year, date.Month, dayNo);
                    if (today > DateTime.Now) break;
                    var e = list.Where(t => t.F_Date == today);
                    if (e.Count() == 1)
                    {
                        var sa = e.First();
                        var start = DateTime.Parse(((DateTime)sa.F_CreatorTime).ToString("yyyy-MM-dd") + " 00:00:00");
                        var end = DateTime.Parse(((DateTime)sa.F_CreatorTime).ToString("yyyy-MM-dd") + " 23:59:59");
                        var flows = db.QueryAsNoTracking<AttendanceStuFlow>(t =>
                            t.F_CreatorTime >= start && t.F_CreatorTime <= end && t.F_Student == student.F_Id).ToList();
                        var times = new List<object>();
                        foreach (var flow in flows)
                        {
                            var o = new
                            {
                                F_CheckTime = flow.F_Time.ToString(@"hh\:mm\:ss"),
                                F_CheckInfo = flow.F_Memo,
                                F_CheckType = getCheckTypeStr(flow.F_Memo)
                            };
                            times.Add(o);
                        }

                        //计算F_CheckStatus
                        var F_CheckStatus = "";
                        if (sa.F_Qj > 0)
                            F_CheckStatus = "3";
                        else if (sa.F_Qk > 0 || sa.F_Qk2 > 0 || sa.F_Zt > 0 || sa.F_Cd > 0)
                            F_CheckStatus = "2";
                        else
                            F_CheckStatus = "1";

                        var res = new
                        {
                            F_CheckStatus = F_CheckStatus,
                            F_DeviceName = sa.F_DeviceName,
                            F_CheckDate = ((DateTime)sa.F_CreatorTime).ToString("yyyy-MM-dd"),
                            F_CheckTime = times
                        };
                        re.Add(res);
                    }
                    else
                    {
                        var period = DateHelper.GetWeekNumberOfDay(new DateTime(date.Year, date.Month, dayNo));
                        //考勤规则 查看是否需要考勤
                        if (!rule.F_AttendanceTime1.IsEmpty() && rule.F_AttendanceTime1.IndexOf(period) == -1)
                        {
                        }
                        //else if (!rule.F_AttendanceTime2.IsEmpty() && rule.F_AttendanceTime2.IndexOf(period) == -1)
                        //{
                        //}
                        //else if (!rule.F_AttendanceTime3.IsEmpty() && rule.F_AttendanceTime3.IndexOf(period) == -1)
                        //{
                        //}
                        else
                        {
                            var res = new
                            {
                                F_CheckStatus = "2",
                                F_DeviceName = "",
                                F_CheckDate = new DateTime(date.Year, date.Month, dayNo).ToString("yyyy-MM-dd"),
                                F_CheckTime = "[]"
                            };
                            re.Add(res);
                        }
                    }
                }

                db.Commit();
                return re.ToJson();
            }
        }

        private string getCheckTypeStr(string result)
        {
            //迟到
            if ("迟到".Equals(result))
                return "3";
            else if ("早退".Equals(result))
                return "4";
            else
                return "";
        }

        public string GetClassSignInfo(string F_CLass, string F_Date)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取班级总考勤统计
                var date = DateTime.Parse(F_Date);
                var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                var period = DateHelper.GetWeekNumberOfDay(date);

                var expression = ExtLinq.True<AttendanceStuCount>();
                expression = expression.And(t => t.F_Class == F_CLass);
                expression = expression.And(t => t.F_CreatorTime >= start);
                expression = expression.And(t => t.F_CreatorTime <= end);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();
                //获取考勤规则
                AttendanceRule rule = null;
                //获取所有班级学生
                var student = db.QueryAsNoTracking<Student>().Where(t => t.F_Class_ID == F_CLass);
                if (student.Count() > 0)
                {
                    var stu = student.First();
                    rule = db.FindEntity<AttendanceRule>(t =>
                        t.F_DepartmentId == stu.F_Divis_ID && t.F_EnabledMark == true);
                    if (rule == null)
                        throw new Exception("没有设置考勤规则");
                }

                var res = new List<Result>();
                foreach (var s in student)
                {
                    var e = list.Where(t => t.F_Student == s.F_Id);
                    if (e.Count() == 1)
                    {
                        var t = new Result();
                        t.F_Student = s.F_Id;
                        t.F_Name = s.F_Name;
                        t.F_HeadPic = s.F_FacePic_File;
                        t.F_CheckType = e.First().F_CheckType;
                        res.Add(t);
                    }
                    else
                    {
                        var t = new Result();
                        t.F_Student = s.F_Id;
                        t.F_Name = s.F_Name;
                        t.F_HeadPic = s.F_FacePic_File;
                        t.F_CheckType = "6";
                        res.Add(t);
                    }
                }

                //F_CheckCount 实到人数 正常+迟到+早退
                //F_DelayCount 迟到人数
                //F_NoCheckCount 缺卡人数
                //F_studentCount 应到人数/总人数
                //F_Delay 迟到人列表
                //F_NoCheck 缺卡人列表

                //计算结果
                var wjl = student.Count() - list.Count();
                //var qqs = 0; //缺勤数
                var cds = list.Count(t => t.F_Cd > 0); ; //迟到数
                var zts = list.Count(t => t.F_Zt > 0); //早退数
                //var wxs = 0; //无效签到打卡数
                var qjs = list.Count(t => t.F_Qj > 0); //请假数
                var qks = list.Sum(t => t.F_Qk) + list.Sum(t => t.F_Qk2) + (wjl * 2); //缺卡数 = 日志缺卡数 + （无记录人数*2）
                var zcs = list.Count(t => t.F_Cd == 0 && t.F_Qk == 0 && t.F_Qk2 == 0 && t.F_Qj == 0 && t.F_Zt == 0);

                var result = new
                {
                    F_CheckCount = res.Count(t =>
                        t.F_CheckType.Contains("3") || t.F_CheckType.Contains("1") || t.F_CheckType.Contains("4")),
                    F_DelayCount = cds,
                    F_NoCheckCount = qks,
                    F_studentCount = student.Count(),
                    F_LeaveCount = qjs,
                    F_Delay = res.Where(t => t.F_CheckType.Contains("3")),
                    F_NoCheck = res.Where(t => t.F_CheckType.Contains("6"))
                };
                db.Commit();
                return result.ToJson();
            }
        }

        public string GetClassSignInfoV2(string F_CLass, string F_Date)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取班级总考勤统计
                var date = DateTime.Parse(F_Date);
                var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                var period = DateHelper.GetWeekNumberOfDay(date);

                var expression = ExtLinq.True<AttendanceStuCount>();
                expression = expression.And(t => t.F_Class == F_CLass);
                expression = expression.And(t => t.F_CreatorTime >= start);
                expression = expression.And(t => t.F_CreatorTime <= end);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();
                //获取考勤规则
                AttendanceRule rule = null;
                //获取所有班级学生
                var student = db.QueryAsNoTracking<Student>().Where(t => t.F_Class_ID == F_CLass);
                if (student.Count() > 0)
                {
                    var stu = student.First();
                    rule = db.FindEntity<AttendanceRule>(t =>
                        t.F_DepartmentId == stu.F_Divis_ID && t.F_EnabledMark == true);
                    if (rule == null)
                        throw new Exception("没有设置考勤规则");
                }

                var res = new List<Result>();
                foreach (var s in student)
                {
                    var e = list.Where(t => t.F_Student == s.F_Id);
                    if (e.Count() == 1)
                    {
                        var t = new Result();
                        t.F_Student = s.F_Id;
                        t.F_Name = s.F_Name;
                        t.F_HeadPic = s.F_FacePic_File;
                        t.F_CheckType = e.First().F_CheckType;
                        res.Add(t);
                    }
                    else
                    {
                        var t = new Result();
                        t.F_Student = s.F_Id;
                        t.F_Name = s.F_Name;
                        t.F_HeadPic = s.F_FacePic_File;
                        t.F_CheckType = "6";
                        res.Add(t);
                    }
                }

                //F_CheckCount 实到人数 正常+迟到+早退
                //F_DelayCount 迟到人数
                //F_NoCheckCount 缺卡人数
                //F_studentCount 应到人数/总人数
                //F_Delay 迟到人列表
                //F_NoCheck 缺卡人列表

                //计算结果
                var wjl = student.Count() - list.Count();
                //var qqs = 0; //缺勤数
                var cds = list.Count(t => t.F_Cd > 0); ; //迟到数
                var zts = list.Count(t => t.F_Zt > 0); //早退数
                //var wxs = 0; //无效签到打卡数
                var qjs = list.Count(t => t.F_Qj > 0); //请假数
                var qks = list.Sum(t => t.F_Qk) + list.Sum(t => t.F_Qk2) + (wjl * 2); //缺卡数 = 日志缺卡数 + （无记录人数*2）
                var zcs = list.Count(t => t.F_Cd == 0 && t.F_Qk == 0 && t.F_Qk2 == 0 && t.F_Qj == 0 && t.F_Zt == 0);

                var result = new
                {
                    //F_CheckCount = res.Count(t =>
                    //    t.F_CheckType.Contains("3") || t.F_CheckType.Contains("1") || t.F_CheckType.Contains("4")),
                    F_DelayCount = cds,
                    F_NoCheckCount = qks,
                    //F_studentCount = student.Count(),
                    F_LeaveCount = qjs,
                    F_Delay = res.Where(t => t.F_CheckType.Contains("3")),
                    F_NoCheck = res.Where(t => t.F_CheckType.Contains("6")),
                    F_Leaves = res.Where(t => t.F_CheckType.Contains("7"))
                };
                db.Commit();
                return result.ToJson();
            }
        }

        public string GetClassSignDetailInfo(string F_CLass, string F_Date)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取班级总考勤统计
                var date = DateTime.Parse(F_Date);
                var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                var period = DateHelper.GetWeekNumberOfDay(date);

                var expression = ExtLinq.True<AttendanceStuCount>();
                expression = expression.And(t => t.F_Class == F_CLass);
                expression = expression.And(t => t.F_CreatorTime >= start);
                expression = expression.And(t => t.F_CreatorTime <= end);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();
                //获取考勤规则
                AttendanceRule rule = null;
                //获取所有班级学生
                var student = db.QueryAsNoTracking<Student>().Where(t => t.F_Class_ID == F_CLass);
                if (student.Count() > 0)
                {
                    var stu = student.First();
                    rule = db.FindEntity<AttendanceRule>(t =>
                        t.F_DepartmentId == stu.F_Divis_ID && t.F_EnabledMark == true);
                    if (rule == null)
                        throw new Exception("没有设置考勤规则");
                }

                var res = new List<Result>();
                foreach (var s in student)
                {
                    var e = list.Where(t => t.F_Student == s.F_Id);
                    if (e.Count() == 1)
                    {
                        var t = new Result();
                        t.F_Student = s.F_Id;
                        t.F_Name = s.F_Name;
                        t.F_HeadPic = s.F_FacePic_File;
                        t.F_CheckType = e.First().F_CheckType;
                        res.Add(t);
                    }
                    else
                    {
                        //缺勤
                        var t = new Result();
                        t.F_Student = s.F_Id;
                        t.F_Name = s.F_Name;
                        t.F_HeadPic = s.F_FacePic_File;
                        t.F_CheckType = "2";
                        res.Add(t);
                    }
                }

                //F_CheckCount 实到人数 正常+迟到+早退
                //F_DelayCount 迟到人数
                //F_NoCheckCount 缺卡人数
                //F_studentCount 应到人数/总人数
                //F_Delay 迟到人列表
                //F_NoCheck 缺卡人列表
                var result = new
                {
                    F_CheckCount = res.Count(t =>
                        t.F_CheckType.Contains("3") || t.F_CheckType.Contains("1") || t.F_CheckType.Contains("4")),
                    F_DelayCount = res.Count(t => t.F_CheckType.Contains("3")),
                    F_NoCheckCount = res.Count(t => t.F_CheckType.Contains("2")),
                    F_studentCount = student.Count(),
                    F_Students = res
                };
                db.Commit();
                return result.ToJson();
            }
        }

        public string GetClassSignDetailInfoV2(string F_CLass, string F_Date)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取班级总考勤统计
                var date = DateTime.Parse(F_Date);
                var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                var period = DateHelper.GetWeekNumberOfDay(date);

                var expression = ExtLinq.True<AttendanceStuCount>();
                expression = expression.And(t => t.F_Class == F_CLass);
                expression = expression.And(t => t.F_CreatorTime >= start);
                expression = expression.And(t => t.F_CreatorTime <= end);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();
                //获取考勤规则
                AttendanceRule rule = null;
                //获取所有班级学生
                var student = db.QueryAsNoTracking<Student>().Where(t => t.F_Class_ID == F_CLass);
                if (student.Count() > 0)
                {
                    var stu = student.First();
                    rule = db.FindEntity<AttendanceRule>(t =>
                        t.F_DepartmentId == stu.F_Divis_ID && t.F_EnabledMark == true);
                    if (rule == null)
                        throw new Exception("没有设置考勤规则");
                }

                var res = new List<Object>();
                foreach (var s in student)
                {
                    var e = list.Where(t => t.F_Student == s.F_Id);
                    if (e.Count() == 1)
                    {
                        var item = e.First();
                        //计算F_CheckStatus
                        var F_CheckStatus = "";
                        if (item.F_Qj > 0)
                            F_CheckStatus = "3";
                        else if (item.F_Qk > 0 || item.F_Qk2 > 0 || item.F_Zt > 0 || item.F_Cd > 0)
                            F_CheckStatus = "2";
                        else
                            F_CheckStatus = "1";

                        var t = new
                        {
                            F_Student = s.F_Id,
                            F_Name = s.F_Name,
                            F_HeadPic = s.F_FacePic_File,
                            F_CheckStatus = F_CheckStatus
                        };

                        res.Add(t);
                    }
                    else
                    {
                        var t = new
                        {
                            F_Student = s.F_Id,
                            F_Name = s.F_Name,
                            F_HeadPic = s.F_FacePic_File,
                            F_CheckStatus = "2"
                        };
                        res.Add(t);
                    }
                }
                //计算结果
                var wjl = student.Count() - list.Count();
                //var qqs = 0; //缺勤数
                var cds = list.Count(t => t.F_Cd > 0); ; //迟到数
                var zts = list.Count(t => t.F_Zt > 0); //早退数
                //var wxs = 0; //无效签到打卡数
                var qjs = list.Count(t => t.F_Qj > 0); //请假数
                var qks = list.Sum(t => t.F_Qk) + list.Sum(t => t.F_Qk2) + (wjl * 2); //缺卡数 = 日志缺卡数 + （无记录人数*2）
                var zcs = list.Count(t => t.F_Cd == 0 && t.F_Qk == 0 && t.F_Qk2 == 0 && t.F_Qj == 0 && t.F_Zt == 0);

                var result = new
                {
                    F_DelayCount = cds,
                    F_NoCheckCount = qks,
                    F_LeaveCount = qjs,
                    F_Students = res
                };
                db.Commit();
                return result.ToJson();
            }
        }

        public string GetClassSignEearlyList(string F_Sn, string F_Date,int F_LastNum)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取班级总考勤统计
                var date = DateTime.Parse(F_Date);
                var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                var period = DateHelper.GetWeekNumberOfDay(date);

                var expression = ExtLinq.True<AttendanceStuCount>();
                if (!string.IsNullOrEmpty(F_Sn))
                {
                    expression = expression.And(t => t.F_DeviceName == F_Sn);
                }
                expression = expression.And(t => t.F_CreatorTime >= start);
                expression = expression.And(t => t.F_CreatorTime <= end);
                expression = expression.And(t => t.F_Start1_Result == "1");

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).Take(F_LastNum).ToList();
             
                var result = new List<Result>();

                // 排名
                var rank = 1;

                foreach (var item in list)
                {
                    var s = db.QueryAsNoTracking<Student>().Where(t => t.F_Id == item.F_Student).FirstOrDefault();

                    if (s != null)
                    {
                        var r = new Result();
                        r.F_Student = item.F_Student;
                        r.F_Name = s.F_Name;
                        r.F_HeadPic = s.F_FacePic_File;
                        r.F_Rank = rank;
                        r.F_Time = item.F_Start1_Check.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        result.Add(r);

                        rank++;
                    }
                }

                return result.Select(t => new { t.F_Name, t.F_HeadPic, t.F_Rank, t.F_Time }).ToList().ToJson();
            }
        }

        public object GetClassSignInfoObj(string F_CLass, string F_Date)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取班级总考勤统计
                var date = DateTime.Parse(F_Date);
                var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                var period = DateHelper.GetWeekNumberOfDay(date);

                var expression = ExtLinq.True<AttendanceStuCount>();
                expression = expression.And(t => t.F_Class == F_CLass);
                expression = expression.And(t => t.F_CreatorTime >= start);
                expression = expression.And(t => t.F_CreatorTime <= end);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();
                //获取考勤规则
                AttendanceRule rule = null;
                //获取所有班级学生
                var student = db.QueryAsNoTracking<Student>().Where(t => t.F_Class_ID == F_CLass);
                if (student.Count() > 0)
                {
                    var stu = student.First();
                    rule = db.FindEntity<AttendanceRule>(t =>
                        t.F_DepartmentId == stu.F_Divis_ID && t.F_EnabledMark == true);
                    if (rule == null)
                        throw new Exception("没有设置考勤规则");
                }

                var res = new List<object>();
                foreach (var s in student)
                {
                    var e = list.Where(t => t.F_Student == s.F_Id);
                    if (e.Count() == 1)
                    {
                        var t = new
                        {
                            F_Student = s.F_Id,
                            F_Name = s.F_Name,
                            F_HeadPic = s.F_FacePic_File,
                            F_CheckType = e.First().F_CheckType
                        };

                        res.Add(t);
                    }
                    else
                    {
                        var t = new
                        {
                            F_Student = s.F_Id,
                            F_Name = s.F_Name,
                            F_HeadPic = s.F_FacePic_File,
                            F_CheckType = "6"
                        };
                        res.Add(t);
                    }
                }

                db.Commit();
                return res;
            }
        }
        public List<ClassAttResult> GetAllClassSign(string divisId, string gradeId,string classId, string startTime, string endTime)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取班级总考勤统计
                var classAttResultList = new List<ClassAttResult>();
                var ruleList = db.QueryAsNoTracking<AttendanceRule>();
                var studentList = db.QueryAsNoTracking<Student>();
                var attendanceList = db.QueryAsNoTracking<AttendanceStuCount>();
                if (!string.IsNullOrEmpty(classId))
                {
                    attendanceList= attendanceList.Where(p => p.School_Students_Entity.F_Class_ID.Equals(classId));
                }
                else
                {
                    if (!string.IsNullOrEmpty(gradeId))
                    {
                        attendanceList= attendanceList.Where(p => p.School_Students_Entity.F_Grade_ID.Equals(gradeId));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(divisId))
                        {
                            attendanceList= attendanceList.Where(p => p.School_Students_Entity.F_Divis_ID.Equals(divisId));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(startTime))
                {
                    var start = DateTime.Parse(startTime + " 00:00:00");
                    attendanceList = attendanceList.Where(p => p.F_CreatorTime >= start);
                }
                if (!string.IsNullOrEmpty(endTime))
                {
                    var end = DateTime.Parse(endTime + " 23:59:59");
                    attendanceList = attendanceList.Where(p => p.F_CreatorTime <= end);
                }
                var group = attendanceList.GroupBy(p => p.School_Students_Entity.F_Class_ID).ToList();
                foreach (var item in group)
                {
                    if (!string.IsNullOrEmpty( item.Key))
                    {
                        var students = studentList.Where(t => t.F_Class_ID.Equals(item.Key)).ToList();
                        var wjl = students.Count() - item.Count();//无记录数
                        var ycs = item.Count(t => t.F_Cd > 0 || t.F_Zt > 0);//异常数
                        var qjs = item.Count(t => t.F_Qj > 0); //请假数
                        var qks = item.Sum(t => t.F_Qk) + item.Sum(t => t.F_Qk2) + (wjl * 2); //缺卡数 = 日志缺卡数 + （无记录人数*2）
                        var zcs = item.Count(t => t.F_Cd == 0 && t.F_Qk == 0 && t.F_Qk2 == 0 && t.F_Qj == 0 && t.F_Zt == 0);//正常数
                        classAttResultList.Add
                            (new ClassAttResult()
                            {
                                ClassId = item.Key,
                                GradeId = item.First().School_Students_Entity.F_Grade_ID,
                                DivisId = item.First().School_Students_Entity.F_Divis_ID,
                                Time=Convert.ToDateTime( item.First().F_CreatorTime),
                                StuCount = students.Count(),
                                NormalCount = zcs,
                                AbnormalCount = ycs,
                                LeaveCount = qjs,
                                NoCheckCount = qks.Value,
                            }); 
                    }
                }
                db.Commit();
                return classAttResultList;
            }
        }
        public Dictionary<string, int> GetClassCheckInfo(string F_CLass, DateTime F_Date)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //获取班级总考勤统计
                var date = F_Date;
                var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                var period = DateHelper.GetWeekNumberOfDay(date);

                var expression = ExtLinq.True<AttendanceStuCount>();
                expression = expression.And(t => t.F_Class == F_CLass);
                expression = expression.And(t => t.F_CreatorTime >= start);
                expression = expression.And(t => t.F_CreatorTime <= end);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();
                //获取考勤规则
                AttendanceRule rule = null;
                //获取所有班级学生
                var student = db.QueryAsNoTracking<Student>().Where(t => t.F_Class_ID == F_CLass);
                if (student.Count() > 0)
                {
                    var stu = student.First();
                    rule = db.FindEntity<AttendanceRule>(t =>
                        t.F_DepartmentId == stu.F_Divis_ID && t.F_EnabledMark == true);
                    if (rule == null)
                        throw new Exception("没有设置考勤规则");
                }

                //计算结果
                var res = new Dictionary<string, int>();
                var wjl = student.Count() - list.Count();
                //var qqs = 0; //缺勤数
                var cds = list.Count(t => t.F_Cd > 0); ; //迟到数
                var zts = list.Count(t => t.F_Zt > 0); //早退数
                //var wxs = 0; //无效签到打卡数
                var qjs = list.Count(t => t.F_Qj > 0); //请假数
                var qks = list.Sum(t => t.F_Qk) + list.Sum(t => t.F_Qk2) + (wjl * 2); //缺卡数 = 日志缺卡数 + （无记录人数*2）
                var zcs = list.Count(t => t.F_Cd == 0 && t.F_Qk == 0 && t.F_Qk2 == 0 && t.F_Qj == 0 && t.F_Zt == 0);
                //foreach (var s in student)
                //{
                //    var e = list.Where(t => t.F_Student == s.F_Id);
                //    if (e.Count() == 1)
                //    {
                //        if (e.First().F_CheckType.IndexOf("1") != -1)
                //            zcs++;
                //        if (e.First().F_CheckType.IndexOf("3") != -1)
                //            cds++;
                //        if (e.First().F_CheckType.IndexOf("4") != -1)
                //            zts++;
                //        if (e.First().F_CheckType.IndexOf("6") != -1)
                //            qks++;
                //    }
                //    else
                //    {
                //        //考勤规则 查看是否需要考勤
                //        //if (!rule.F_AttendanceTime1.IsEmpty() && rule.F_AttendanceTime1.IndexOf(period) == -1)
                //        //    qqs++;
                //        //else if (!rule.F_AttendanceTime2.IsEmpty() && rule.F_AttendanceTime2.IndexOf(period) == -1)
                //        //    qqs++;
                //        //else if (!rule.F_AttendanceTime3.IsEmpty() &&
                //        //         rule.F_AttendanceTime3.IndexOf(period) == -1) qqs++;
                //        qqs++;
                //    }
                //}

                db.Commit();
                //res.Add("1", zcs + zts + cds);
                //由于我的班级后台没有早退项
                res.Add("1", zcs);
                //res.Add("2", qqs + qks);
                res.Add("3", cds);
                res.Add("4", zts);
                //res.Add("5", wxs);
                res.Add("6", qks.Value);
                res.Add("7", qjs);
                return res;
            }
        }

        //10.7.获取教室日课某节课的考勤信息
        public Dictionary<string, int> GetRoomCourseSignInfoByLesson(string F_Sn, string F_CourseId)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var expression = ExtLinq.True<AttendanceStuFlow>();
                expression = expression.And(t => t.F_Course_PrepareID == F_CourseId);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();

                //获取所有班级学生
                var School_Schedules_MoveCourse_Entity =
                    db.FindEntity<SchScheduleMoveCourse>(t => t.F_Course_PrepareID == F_CourseId);
                var res = new Dictionary<string, int>();

                if (School_Schedules_MoveCourse_Entity != null)
                {
                    var students = db.QueryAsNoTracking<Schedule_MoveClassStudent_Entity>(t =>
                        t.F_MoveClassId == School_Schedules_MoveCourse_Entity.F_Class).ToList();

                    //计算结果
                    var zcs = 0; //正常数
                    //int qqs = 0; //缺勤数            /* 没用上,我先给注掉  2018年12月20日13点55分,余金锋*/
                    var cds = 0; //迟到数
                    //int zts = 0; //早退数             /*没用上,我先给注掉  2018年12月20日13点55分,余金锋*/
                    //int wxs = 0; //无效签到打卡数     /* 没用上,我先给注掉  2018年12月20日13点55分,余金锋*/
                    var qks = 0; //缺卡数

                    foreach (var s in students)
                    {
                        var ifQk = false;
                        var ifCd = false;
                        var ifZc = false;
                        var e = list.Where(t => t.F_Student == s.School_Students_Entity.F_Id);
                        if (e.Count() == 0)
                        {
                            ifQk = true;
                        }
                        else
                        {
                            ifZc = true;
                            foreach (var flow in e)
                            {
                                if ("1".Equals(flow.F_Qk))
                                {
                                    ifQk = true;
                                    continue;
                                }

                                if ("1".Equals(flow.F_Cd))
                                {
                                    ifCd = true;
                                    continue;
                                }
                            }
                        }

                        if (ifCd) cds++;
                        else if (ifQk) qks++;
                        else if (ifZc) zcs++;
                    }

                    res.Add("F_studentCount", zcs + qks + cds);
                    res.Add("F_CheckCount", zcs + cds);
                    res.Add("F_DelayCount", cds);
                    res.Add("F_NoCheckCount", qks);
                }
                else
                {
                    throw new Exception("无效课程");
                }

                db.Commit();
                return res;
            }
        }

        //10.8.获取教室日课某节课的考勤信息（新）
        public Dictionary<string, int> GetRoomCourseSignInfoByLessonV2(string F_Sn, string F_CourseId)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var expression = ExtLinq.True<AttendanceStuFlow>();
                expression = expression.And(t => t.F_Course_PrepareID == F_CourseId);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();

                //请假记录
                //获取班级总考勤统计
                var date = DateTime.Now;
                var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                var qjexpression = ExtLinq.True<AttendanceStuCount>();
                qjexpression = qjexpression.And(t => t.F_CreatorTime >= start);
                qjexpression = qjexpression.And(t => t.F_CreatorTime <= end);
                qjexpression = qjexpression.And(t => t.F_Qj > 0);

                //获取当期所有考勤记录
                var qjlist = db.QueryAsNoTracking(qjexpression).OrderBy(t => t.F_CreatorTime).ToList();

                //获取所有班级学生
                var School_Schedules_MoveCourse_Entity =
                    db.FindEntity<SchScheduleMoveCourse>(t => t.F_Course_PrepareID == F_CourseId);
                var res = new Dictionary<string, int>();

                if (School_Schedules_MoveCourse_Entity != null)
                {
                    var students = db.QueryAsNoTracking<Schedule_MoveClassStudent_Entity>(t =>
                        t.F_MoveClassId == School_Schedules_MoveCourse_Entity.F_Class).ToList();

                    //计算结果
                    var zcs = 0; //正常数
                    var cds = 0; //迟到数
                    var qks = 0; //缺卡数
                    var qjs = 0; //请假数

                    foreach (var s in students)
                    {
                        var ifQk = false;
                        var ifCd = false;
                        var ifZc = false;
                        var ifQj = false;

                        var e = list.Where(t => t.F_Student == s.School_Students_Entity.F_Id);
                        var qj = qjlist.Where(t => t.F_Student == s.School_Students_Entity.F_Id);
                        if (qj.Count() > 0)
                        {
                            ifQj = true;
                        }
                        else if (e.Count() == 0)
                        {
                            ifQk = true;
                        }
                        else
                        {
                            ifZc = true;
                            foreach (var flow in e)
                            {
                                if ("1".Equals(flow.F_Qk))
                                {
                                    ifQk = true;
                                    continue;
                                }

                                if ("1".Equals(flow.F_Cd))
                                {
                                    ifCd = true;
                                    continue;
                                }
                            }
                        }

                        if (ifCd) cds++;
                        else if (ifQk) qks++;
                        else if (ifZc) zcs++;
                        else if (ifQj) qjs++;
                    }

                    res.Add("F_LeaveCount", qjs);
                    res.Add("F_DelayCount", cds);
                    res.Add("F_NoCheckCount", qks);
                }
                else
                {
                    throw new Exception("无效课程");
                }

                db.Commit();
                return res;
            }
        }

        public object GetRoomCourseDetailSignInfoByLesson(string F_Sn, string F_CourseId)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var expression = ExtLinq.True<AttendanceStuFlow>();
                expression = expression.And(t => t.F_Course_PrepareID == F_CourseId);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();

                //获取所有班级学生
                var School_Schedules_MoveCourse_Entity =
                    db.FindEntity<SchScheduleMoveCourse>(t => t.F_Course_PrepareID == F_CourseId);
                if (School_Schedules_MoveCourse_Entity == null)
                    throw new Exception("无效课程");
                var students = db.QueryAsNoTracking<Schedule_MoveClassStudent_Entity>(t =>
                    t.F_MoveClassId == School_Schedules_MoveCourse_Entity.F_Class).ToList();

                //请假记录
                //获取班级总考勤统计
                var date = DateTime.Now;
                var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                var qjexpression = ExtLinq.True<AttendanceStuCount>();
                qjexpression = qjexpression.And(t => t.F_CreatorTime >= start);
                qjexpression = qjexpression.And(t => t.F_CreatorTime <= end);
                qjexpression = qjexpression.And(t => t.F_Qj > 0);

                //获取当期所有考勤记录
                var qjlist = db.QueryAsNoTracking(qjexpression).OrderBy(t => t.F_CreatorTime).ToList();

                //计算结果
                var res = new Dictionary<string, object>();
                var zcs = 0; //正常数
                var cds = 0; //迟到数
                var qks = 0; //缺卡数
                var qjs = 0; //请假数

                var qkList = new List<object>();
                var cdList = new List<object>();
                var qjList = new List<object>();

                foreach (var s in students)
                {
                    var ifQk = false;
                    var ifCd = false;
                    var ifZc = false;
                    var ifQj = false;

                    var e = list.Where(t => t.F_Student == s.School_Students_Entity.F_Id);
                    var qj = qjlist.Where(t => t.F_Student == s.School_Students_Entity.F_Id);
                    var student = new
                    {
                        F_Student = s.School_Students_Entity.F_Id,
                        F_Name = s.School_Students_Entity.F_Name,
                        F_HeadPic = s.School_Students_Entity.F_FacePic_File
                    };

                    if (qj.Count() > 0)
                    {
                        ifQj = true;
                        qjList.Add(student);
                    }
                    else if (e.Count() == 0)
                    {
                        ifQk = true;
                    }
                    else
                    {
                        ifZc = true;
                        foreach (var flow in e)
                        {
                            if ("1".Equals(flow.F_Qk))
                            {
                                ifQk = true;
                                qkList.Add(student);
                                continue;
                            }

                            if ("1".Equals(flow.F_Cd))
                            {
                                ifCd = true;
                                cdList.Add(student);
                            }
                        }
                    }

                    if (ifCd) cds++;
                    else if (ifQk) qks++;
                    else if (ifZc) zcs++;
                    else if (ifQj) qjs++;
                }

                db.Commit();
                //res.Add("F_studentCount", zcs + qks + cds);
                //res.Add("F_CheckCount", zcs + cds);
                res.Add("F_DelayCount", cds);
                res.Add("F_NoCheckCount", qks);
                res.Add("F_LeaveCount", qjs);
                res.Add("F_Delay", cdList);
                res.Add("F_NoCheck", qkList);
                res.Add("F_Leaves", qjList);
                return res;
            }
        }

        public object GetCourseDetailSignInfoByLessonStu(string F_Sn, string F_CourseId, string F_StudentId)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var expression = ExtLinq.True<AttendanceStuFlow>();
                expression = expression.And(t => t.F_Course_PrepareID == F_CourseId);
                expression = expression.And(t => t.F_Student == F_StudentId);

                //获取当期所有考勤记录
                var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();

                //获取所有班级学生
                var schoolSchedulesMoveCourseEntity =
                    db.FindEntity<SchScheduleMoveCourse>(t => t.F_Course_PrepareID == F_CourseId);
                if (schoolSchedulesMoveCourseEntity == null)
                    throw new Exception("无效课程");
                var student = db.FindEntity<Student>(t => t.F_Id == F_StudentId);

                //计算结果
                var res = new List<object>();
                var e = list.Where(t => t.F_Student == student.F_Id);
                if (e.Count() == 0)
                {
                    var obj = new
                    {
                        F_CheckType = "2",
                        F_CheckTime = "",
                        F_Desprition = "缺卡"
                    };
                    res.Add(obj);
                }
                else
                {
                    foreach (var flow in e)
                    {
                        if ("1".Equals(flow.F_Qk))
                        {
                            var qker = new
                            {
                                F_CheckType = "2",
                                F_CheckTime = flow.F_Time.ToString(@"hh\:mm\:ss"),
                                F_Desprition = flow.F_Memo
                            };
                            res.Add(qker);
                            continue;
                        }

                        if ("1".Equals(flow.F_Cd))
                        {
                            var cder = new
                            {
                                F_CheckType = "3",
                                F_CheckTime = flow.F_Time.ToString(@"hh\:mm\:ss"),
                                F_Desprition = flow.F_Memo
                            };
                            res.Add(cder);
                            continue;
                        }

                        var zcer = new
                        {
                            F_CheckType = "1",
                            F_CheckTime = flow.F_Time.ToString(@"hh\:mm\:ss"),
                            F_Desprition = flow.F_Memo
                        };
                        res.Add(zcer);
                    }
                }

                db.Commit();
                return res;
            }
        }

        public object GetCourseStudentByDateAndLesson(string F_Sn, string F_CourseId)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var course = db.QueryAsNoTracking<SchScheduleMoveCourse>(t => t.F_Course_PrepareID == F_CourseId).ToList();

                //if (course == null) throw new Exception("没有课程");

                //if (!course.F_IsMoveCourse.ToBool()) throw new Exception("非考勤课程");

                //计算结果
                var re = new List<Dictionary<string, object>>();
                var res = new Dictionary<string, object>();
                var student = new List<object>();
                var zcs = 0; //正常数
                var cds = 0; //迟到数
                var qks = 0; //缺卡数
                var qjs = 0; //请假数

                if (course != null)
                {
                    foreach (var c in course)
                    {
                        if (c.F_IsMoveCourse.ToBool())
                        {
                            var expression = ExtLinq.True<AttendanceStuFlow>();
                            expression = expression.And(t => t.F_Course_PrepareID == c.F_Course_PrepareID);
                            //获取当期所有考勤记录
                            var list = db.QueryAsNoTracking(expression).OrderBy(t => t.F_CreatorTime).ToList();

                            //获取所有班级学生
                            var students =
                                db.QueryAsNoTracking<Schedule_MoveClassStudent_Entity>(t => t.F_MoveClassId == c.F_Class)
                                .Select(t => t.School_Students_Entity)
                                    .ToList();
                            students.AddRange(db.QueryAsNoTracking<Student>(t => t.F_Id == c.F_Class).ToList());

                            //请假记录
                            //获取班级总考勤统计
                            var date = DateTime.Now;
                            var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                            var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                            var qjexpression = ExtLinq.True<AttendanceStuCount>();
                            qjexpression = qjexpression.And(t => t.F_CreatorTime >= start);
                            qjexpression = qjexpression.And(t => t.F_CreatorTime <= end);
                            qjexpression = qjexpression.And(t => t.F_Qj > 0);

                            //获取当期所有考勤记录
                            var qjlist = db.QueryAsNoTracking(qjexpression).OrderBy(t => t.F_CreatorTime).ToList();

                            foreach (var s in students)
                            {
                                var ifQk = false;
                                var ifCd = false;
                                //var ifZc = false;
                                var ifQj = false;

                                var e = list.Where(t => t.F_Student == s.F_Id);
                                var qj = qjlist.Where(t => t.F_Student == s.F_Id);

                                if (qj.Count() > 0)
                                {
                                    ifQj = true;
                                }
                                else if (e.Count() == 0)
                                {
                                    ifQk = true;
                                }
                                else
                                {
                                    //ifZc = true;
                                    foreach (var flow in e)
                                    {
                                        if ("1".Equals(flow.F_Qk))
                                        {
                                            ifQk = true;
                                            continue;
                                        }

                                        if ("1".Equals(flow.F_Cd))
                                        {
                                            ifCd = true;
                                        }
                                    }
                                }

                                var F_CheckStatus = "1";
                                if (ifCd)
                                {
                                    cds++;
                                    F_CheckStatus = "2";
                                }
                                else if (ifQk)
                                {
                                    qks++;
                                    F_CheckStatus = "2";
                                }
                                else if (ifQj)
                                {
                                    qjs++;
                                    F_CheckStatus = "3";
                                }
                                else
                                {
                                    zcs++;
                                }

                                var o = new
                                {
                                    F_Id = s.F_Id,
                                    F_StudentNum = s.F_StudentNum,
                                    F_CardNo = s.F_Mac_No,
                                    F_Name = s.F_Name,
                                    F_HeadPic = s.F_FacePic_File,
                                    F_CheckStatus = F_CheckStatus
                                };

                                student.Add(o);
                            }
                            var moveClassName = db.FindEntity<Schedule_MoveClass_Entity>(t => t.F_Id == c.F_Class);
                            res.Add("F_Name", moveClassName == null ? "" : moveClassName.F_Name);
                            res.Add("F_DelayCount", cds);
                            res.Add("F_NoCheckCount", qks);
                            res.Add("F_LeaveCount", qjs);
                            res.Add("F_Students", student);
                            re.Add(res);
                        }
                        else
                        {
                            var classroomId = db.QueryAsNoTracking<Classroom>().Where(p => p.F_Classroom_Device_ID.Equals(F_Sn)).Select(p => p.F_Id).FirstOrDefault();
                            var cinfo = db.QueryAsNoTracking<ClassInfo>().Where(p => p.F_Classroom.Equals(classroomId)).FirstOrDefault();
                            //获取班级总考勤统计
                            var date = DateTime.Now;
                            var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                            var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                            var period = DateHelper.GetWeekNumberOfDay(date);

                            var expression = ExtLinq.True<AttendanceStuCount>();
                            expression = expression.And(t => t.F_Class == cinfo.F_ClassID);
                            expression = expression.And(t => t.F_CreatorTime >= start);
                            expression = expression.And(t => t.F_CreatorTime <= end);

                            //获取当期所有考勤记录
                            var list = db.QueryAsNoTracking(expression).ToList();
                            //获取考勤规则
                            AttendanceRule rule = null;
                            //获取所有班级学生
                            var studentList = db.QueryAsNoTracking<Student>().Where(t => t.F_Class_ID == cinfo.F_ClassID).ToList();
                            if (studentList.Count() > 0)
                            {
                                var stu = studentList.First();
                                rule = db.FindEntity<AttendanceRule>(t =>
                                    t.F_DepartmentId == stu.F_Divis_ID && t.F_EnabledMark == true);
                                if (rule == null)
                                    throw new Exception("没有设置考勤规则");
                            }
                            foreach (var s in studentList)
                            {
                                var e = list.Where(t => t.F_Student == s.F_Id);
                                if (e.Count() == 1)
                                {
                                    var item = e.First();
                                    //计算F_CheckStatus
                                    var F_CheckStatus = "";
                                    if (item.F_Qj > 0)
                                        F_CheckStatus = "3";
                                    else if (item.F_Qk > 0 || item.F_Qk2 > 0 || item.F_Zt > 0 || item.F_Cd > 0)
                                        F_CheckStatus = "2";
                                    else
                                        F_CheckStatus = "1";

                                    var t = new
                                    {
                                        F_Id = s.F_Id,
                                        F_StudentNum = s.F_StudentNum,
                                        F_CardNo = s.F_Mac_No,
                                        F_Name = s.F_Name,
                                        F_HeadPic = s.F_FacePic_File,
                                        F_CheckStatus = F_CheckStatus
                                    };

                                    student.Add(t);
                                }
                                else
                                {
                                    var t = new
                                    {
                                        F_Id = s.F_Id,
                                        F_StudentNum = s.F_StudentNum,
                                        F_CardNo = s.F_Mac_No,
                                        F_Name = s.F_Name,
                                        F_HeadPic = s.F_FacePic_File,
                                        F_CheckStatus = "2"
                                    };
                                    student.Add(t);
                                }
                            }
                            //计算结果
                            var wjl = student.Count() - list.Count();
                            //var qqs = 0; //缺勤数
                            cds = list.Count(t => t.F_Cd > 0); ; //迟到数
                                                                 //var zts = list.Count(t => t.F_Zt > 0); //早退数
                                                                 //var wxs = 0; //无效签到打卡数
                            qjs = list.Count(t => t.F_Qj > 0); //请假数
                            qks = list.Sum(t => (int)t.F_Qk) + list.Sum(t => (int)t.F_Qk2) + (wjl * 2); //缺卡数 = 日志缺卡数 + （无记录人数*2）
                            //var zcs = list.Count(t => t.F_Cd == 0 && t.F_Qk == 0 && t.F_Qk2 == 0 && t.F_Qj == 0 && t.F_Zt == 0);
                            res.Add("F_Name", cinfo.F_Name == null ? "" : cinfo.F_Name);
                            res.Add("F_DelayCount", cds);
                            res.Add("F_NoCheckCount", qks);
                            res.Add("F_LeaveCount", qjs);
                            res.Add("F_Students", student);
                            re.Add(res);
                        }
                    }
                }
                db.Commit();
                return re;
            }
        }

        public object GetRoomCourseOfDay(Dictionary<string, Dictionary<string, string>> SCHEDULESTIME, string F_Sn,
            string F_Date)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var date = DateTime.Parse(F_Date);

                var deventity = db.FindEntity<ElectronicBoard>(t => t.F_Sn == F_Sn); //设备
                //课程
                var rows = db.QueryAsNoTracking<SchScheduleMoveCourse>(t =>
                    t.F_Date == date && t.F_Classroom == deventity.F_Classroom);
                var res = new List<object>(); //结果集

                //课程信息

                foreach (var item in rows)
                {
                    if (item == null) continue;
                    //int zcs = 0; //正常数
                    //int qqs = 0; //缺勤数
                    //int cds = 0; //迟到数
                    //int zts = 0; //早退数
                    //int wxs = 0; //无效签到打卡数
                    //int qks = 0; //缺卡数
                    ////走班课计算考勤
                    //if (item.F_IsMoveCourse.ToBool())
                    //{
                    //    //计算结果
                    //    List<object> student = new List<object>();
                    //    //计算考勤
                    //    var expression = ExtLinq.True<AttendanceStuFlow>();
                    //    expression = expression.And(t => t.F_Course_PrepareID == item.F_Course_PrepareID);

                    //    //获取当期所有考勤记录
                    //    List<AttendanceStuFlow> list = db.IQueryable<AttendanceStuFlow>(expression).OrderBy(t => t.F_CreatorTime).ToList();

                    //    //获取所有班级学生
                    //    List<Schedule_MoveClassStudent_Entity> students = db.IQueryable<Schedule_MoveClassStudent_Entity>(t => t.F_MoveClassId == item.F_Class).ToList();

                    //    bool ifQk = false;
                    //    bool ifCd = false;
                    //    bool ifZc = false;

                    //    foreach (Schedule_MoveClassStudent_Entity s in students)
                    //    {
                    //        ifQk = false;
                    //        ifCd = false;
                    //        ifZc = false;
                    //        var e = list.Where(t => t.F_Student == s.School_Students_Entity.F_Id);
                    //        if (e.Count() == 0)
                    //        {
                    //            ifQk = true;
                    //        }
                    //        else
                    //        {
                    //            ifZc = true;
                    //            foreach (AttendanceStuFlow flow in e)
                    //            {
                    //                if ("1".Equals(flow.F_Qk))
                    //                {
                    //                    ifQk = true;
                    //                    continue;
                    //                }

                    //                if ("1".Equals(flow.F_Cd))
                    //                {
                    //                    ifCd = true;
                    //                    continue;
                    //                }
                    //            }
                    //        }

                    //        if (ifCd) { cds++; }
                    //        else if (ifQk) { qks++; }
                    //        else if (ifZc) { zcs++; }
                    //    }
                    //}

                    //课程时间段
                    var F_TimeSect = "";
                    var dic = new Dictionary<string, string>();
                    SCHEDULESTIME.TryGetValue(item.F_GradeId + item.F_Semester, out dic);
                    dic.TryGetValue(item.F_CourseIndex.ToString(), out F_TimeSect);
                    SchCourse c;

                    c = item?.Course;

                    Teacher t;

                    t = item?.Teacher;

                    var movecourse = new
                    {
                        //F_CheckCount = zcs + cds,
                        //F_DelayCount = cds,
                        //F_NoCheckCount = qks,
                        //F_studentCount = zcs + qks + cds,
                        F_LessonNo = item.F_CourseIndex,
                        F_TimeSect = F_TimeSect,
                        F_CourseName = c == null ? "" : c.F_Name,
                        F_Teacher_ID = t == null ? "" : item.F_Teacher,
                        F_Teacher_Name = t == null ? "" : t.F_Name,
                        F_Teacher_HeadPic = t == null ? "" : t.teacherSysUser.F_HeadIcon,
                        F_CourseId = item.F_Course,
                        F_BMoveCourse = item.F_IsMoveCourse
                    };

                    res.Add(movecourse);
                }

                db.Commit();
                if (res.Count == 0)
                    return null;
                return res;
            }
        }
    }
}