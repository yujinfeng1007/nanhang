using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZHXY.Common;
using ZHXY.Domain.Entity;
using ZHXY.Data;

namespace ZHXY.Repository
{
    public class AttendanceRuleRepositoryThread
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
                    result = "打卡成功";
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

        public string MakeAttendanceLogs4EG(string schoolCode, Student student, string F_Class, string F_Sn,
            string F_Course_PrepareID, string F_TimeSpan, string F_Pos)
        {
            using (var db = new UnitWork(schoolCode).BeginTrans(schoolCode))
            {
                //保存流水
                var flow = new AttendanceStuFlow();
                flow.Create(false);
                flow.F_Student = student.F_Id;
                flow.F_Date = DateTime.Now.Date;
                flow.F_Time = DateTime.Now.TimeOfDay;
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

        public string MakeTeaAttendanceLogs4EG(string schoolCode, Teacher teacher, string F_Class, string F_Sn,
            string F_Course_PrepareID, string F_TimeSpan, string F_Pos)
        {
            using (var db = new UnitWork(schoolCode).BeginTrans(schoolCode))
            {
                //保存流水
                var flow = new AttendanceStuFlow();
                flow.Create(false);
                flow.F_Student = teacher.F_Id;
                flow.F_CreatorUserId = teacher.F_Id;
                flow.F_Date = DateTime.Now.Date;
                flow.F_Time = DateTime.Now.TimeOfDay;
                flow.F_Device = F_Sn;
                flow.F_Type = "2"; //刷卡
                flow.F_Source = "3"; //老师刷卡班牌
                //flow.F_Photo = F_Photo; //监控拍照
                flow.F_Cd = "0";
                flow.F_Zt = "0";
                flow.F_Qk = "0";
                //记录打卡结果
                flow.F_Memo = result;
                db.Insert(flow);
                db.Commit();
            }

            return result;
        }

        public string MakeAttendanceLogs(string schoolCode, Student student, string F_Sn, string F_Class, string F_Photo,
            string F_Course_PrepareID, string F_TimeSpan)
        {
            using (var db = new UnitWork(schoolCode).BeginTrans(schoolCode))
            {
                var device = db.FindEntity<ElectronicBoard>(t => t.F_Sn == F_Sn);
                if (device == null)
                    throw new Exception("没有这台设备");
                //保存流水
                var flow = new AttendanceStuFlow();
                flow.Create(false);
                flow.F_Student = student.F_Id;
                flow.F_Date = DateTime.Now.Date;
                flow.F_Time = DateTime.Now.TimeOfDay;
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
            using (var db = new UnitWork(schoolCode).BeginTrans(schoolCode))
            {
                var device = db.FindEntity<ElectronicBoard>(t => t.F_Sn == F_Sn);
                if (device == null)
                    throw new Exception("没有这台设备");
                //保存流水
                var flow = new AttendanceStuFlow();
                flow.Create(false);
                flow.F_Student = teacher.F_Id;
                flow.F_CreatorUserId = teacher.F_Id;
                flow.F_Date = DateTime.Now.Date;
                flow.F_Time = DateTime.Now.TimeOfDay;
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

        //学生签到统计
        public Dictionary<string, int> StatStudentCheckIn(List<string> schoolType, string statType)
        {
            var dic = new Dictionary<string, int>();
            var zcs = 0;
            var cds = 0;
            var qks = 0;
            var qjs = 0;
            var dates = getStatPeriod(statType);
            var start = dates.First();
            var end = dates.LastOrDefault();
            foreach (var schoolCode in schoolType)
            {
                using (var db = new UnitWork(schoolCode).BeginTrans(schoolCode))
                {
                    var list = db.QueryAsNoTracking<AttendanceStuCount>(t => t.F_CreatorTime >= start && t.F_CreatorTime <= end).ToList();
                    //只计算有卡号的学生
                    var studentNum = db.QueryAsNoTracking<Student>(t => t.F_Mac_No != null).ToList();
                    //计算结果
                    var wjl = studentNum.Count() - list.Count();
                    //var qqs = 0; //缺勤数
                    cds += list.Count(t => t.F_Cd > 0); //迟到数
                    //zts = list.Count(t => t.F_Zt > 0); //早退数
                    qjs += list.Count(t => t.F_Qj > 0); //请假数
                    qks += list.Sum(t => (int)t.F_Qk) + (wjl); //缺卡数 = 日志缺卡数 + （无记录人数*2）
                    zcs += list.Count(t => t.F_Cd == 0 && t.F_Qk == 0 && t.F_Qj == 0 && t.F_Zt == 0);
                    db.Commit();
                }
            }

            dic.Add("qks", qks);
            dic.Add("cds", cds);
            dic.Add("qjs", qjs);
            dic.Add("zcs", zcs);
            return dic;
        }

        //教师签到统计
        public Dictionary<string, int> StatTeacherCheckIn(List<string> schoolType, string statType)
        {
            var dic = new Dictionary<string, int>();
            var zcs = 0;
            var cds = 0;
            var qks = 0;
            var qjs = 0;
            var dates = getStatPeriod(statType);
            var start = dates.First();
            var end = dates.LastOrDefault();
            foreach (var schoolCode in schoolType)
            {
                using (var db = new UnitWork(schoolCode).BeginTrans(schoolCode))
                {
                    var list = db.QueryAsNoTracking<AttendanceStuFlow>(t => t.F_CreatorTime >= start && t.F_CreatorTime <= end && t.F_Source == "3").ToList();
                    var list2 = list.GroupBy(t => t.F_CreatorUserId).Select(t => t.First());
                    //只计算有卡号的老师
                    var teacherNum = db.QueryAsNoTracking<Teacher>(t => t.F_Mac_No != null).ToList();
                    //计算结果
                    var wjl = teacherNum.Count() - list2.Count();
                    //var qqs = 0; //缺勤数
                    cds += 0; //迟到数
                    //zts = list.Count(t => t.F_Zt > 0); //早退数
                    qjs += 0; //请假数
                    qks += wjl; //缺卡数 = 日志缺卡数 + （无记录人数*2）
                    zcs += list2.Count();
                    db.Commit();
                }
            }

            dic.Add("qks", qks);
            dic.Add("cds", cds);
            dic.Add("qjs", qjs);
            dic.Add("zcs", zcs);
            return dic;
        }

        //学生签退统计
        public Dictionary<string, int> StatStudentCheckOut(List<string> schoolType, string statType)
        {
            var dic = new Dictionary<string, int>();
            var qts = 0;
            var zts = 0;
            var qks = 0;
            var qjs = 0;
            var dates = getStatPeriod(statType);
            var start = dates.First();
            var end = dates.LastOrDefault();
            foreach (var schoolCode in schoolType)
            {
                using (var db = new UnitWork(schoolCode).BeginTrans(schoolCode))
                {
                    var list = db.QueryAsNoTracking<AttendanceStuCount>(t => t.F_CreatorTime >= start && t.F_CreatorTime <= end).ToList();
                    //只计算有卡号的学生
                    var studentNum = db.QueryAsNoTracking<Student>(t => t.F_Mac_No != null).ToList();
                    //计算结果
                    var wjl = studentNum.Count() - list.Count();
                    //cds += list.Count(t => t.F_Cd > 0); //迟到数
                    zts = list.Count(t => t.F_Zt > 0); //早退数
                    qjs += list.Count(t => t.F_Qj > 0); //请假数
                    qks += list.Sum(t => (int)t.F_Qk2) + (wjl); //缺卡数 = 日志缺卡数 + （无记录人数*2）
                    qts += list.Count(t => t.F_Qk2 == 0 && t.F_Qj == 0 && t.F_Zt == 0);
                    db.Commit();
                }
            }

            dic.Add("qks", qks);
            dic.Add("zts", zts);
            dic.Add("qjs", qjs);
            dic.Add("qts", qts);
            return dic;
        }

        //学生签到统计
        public Dictionary<string, int> StatToday(List<string> schoolType, DateTime date)
        {
            var dic = new Dictionary<string, int>();
            var zcs = 0;
            var cds = 0;
            var qks = 0;
            var qjs = 0;
            var zts = 0;
            //List<DateTime> dates = getStatPeriod("d");
            var start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
            var end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");

            foreach (var schoolCode in schoolType)
            {
                using (var db = new UnitWork(schoolCode).BeginTrans(schoolCode))
                {
                    var list = db.QueryAsNoTracking<AttendanceStuCount>(t => t.F_CreatorTime >= start && t.F_CreatorTime <= end).ToList();
                    //只计算有卡号的学生
                    var studentNum = db.QueryAsNoTracking<Student>(t => t.F_Mac_No != null).ToList();
                    //计算结果
                    var wjl = studentNum.Count() - list.Count();
                    //var qqs = 0; //缺勤数
                    cds += list.Count(t => t.F_Cd > 0); //迟到数
                    zts += list.Count(t => t.F_Zt > 0); //早退数
                    qjs += list.Count(t => t.F_Qj > 0); //请假数
                    qks += list.Sum(t => (int)t.F_Qk) + list.Sum(t => (int)t.F_Qk2) + (wjl * 2); //缺卡数 = 日志缺卡数 + （无记录人数*2）
                    zcs += list.Count(t => t.F_Cd == 0 && t.F_Qk == 0 && t.F_Qk2 == 0 && t.F_Qj == 0 && t.F_Zt == 0);
                    db.Commit();
                }
            }

            dic.Add("qts", qks);
            dic.Add("cds", cds);
            dic.Add("qjs", qjs);
            dic.Add("zcs", zcs);
            dic.Add("zts", zts);
            return dic;
        }

        //学生签到统计
        public Dictionary<string, int> StatActivity(List<string> schoolType, DateTime start, DateTime end)
        {
            var dic = new Dictionary<string, int>();
            var kqs = 0;

            foreach (var schoolCode in schoolType)
            {
                using (var db = new UnitWork(schoolCode).BeginTrans(schoolCode))
                {
                    var list = db.QueryAsNoTracking<AttendanceStuFlow>(t => t.F_CreatorTime >= start && t.F_CreatorTime <= end).ToList();
                    kqs += list.Count();
                    db.Commit();
                }
            }

            dic.Add("kqs", kqs);
            return dic;
        }

        private List<DateTime> getStatPeriod(string statType)
        {
            var list = new List<DateTime>();
            var date = DateTime.Now;
            var start = DateTime.Now;
            var end = DateTime.Now;

            switch (statType)
            {
                //本日统计
                case "d":
                    start = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 00:00:00");
                    end = DateTime.Parse(date.ToString("yyyy-MM-dd") + " 23:59:59");
                    break;
                //本周统计
                case "w":
                    start = GetWeekFirstDayMon(date);
                    end = GetWeekLastDaySun(date);
                    break;
                //本月统计
                default:
                    start = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
                    end = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);
                    break;
            }
            list.Add(start);
            list.Add(end);
            return list;
        }

        /// <summary>
        /// 得到本周第一天(以星期一为第一天)
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            //星期一为第一天
            var weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            var daydiff = (-1) * weeknow;

            //本周第一天
            var FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay + " 00:00:00");
        }

        /// <summary>
        /// 得到本周最后一天(以星期天为最后一天)
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private DateTime GetWeekLastDaySun(DateTime datetime)
        {
            //星期天为最后一天
            var weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            var daydiff = (7 - weeknow);

            //本周最后一天
            var LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(LastDay + " 23:59:59");
        }
    }
}