using NFine.Application.SchoolManage;
using NFine.Code;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class MyClassController : ControllerBase
    {
        public ActionResult NewsForm()
        {
            return View();
        }

        public ActionResult djs()
        {
            return View();
        }

        public ActionResult NotiForm()
        {
            return View();
        }

        public ActionResult ActivityForm()
        {
            return View();
        }

        public ActionResult PresenceForm()
        {
            return View();
        }

        private School_Class_Info_App classinfoApp = new School_Class_Info_App();
        private School_Devices_App deviceApp = new School_Devices_App();
        private School_Attendance_Rules_App attendanceRulesApp = new School_Attendance_Rules_App();
        private School_Students_App studentApp = new School_Students_App();
        private School_Class_Info_Teacher_App school_Class_Info_Teacher_App = new School_Class_Info_Teacher_App();
        private School_Class_Duty_App schoolClassDutyApp = new School_Class_Duty_App();
        private School_Schedules_App schedulesApp = new School_Schedules_App();

        /// <summary>
        /// 我的班级主平台接口
        /// </summary>
        /// <returns>  </returns>
        public ActionResult MainIndex(string F_Class)
        {
            //当前用户
            OperatorModel teacherUser = OperatorProvider.Provider.GetCurrent();
            if (Ext.IsEmpty(teacherUser.Classes))
            {
                return Error("抱歉，没有找到您管理的班级。");
            }
            else
            {
                //班级信息
                //School_Class_Info_Entity classInfo = classinfoApp.GetFormByF_ClassID(teacherUser.Classes.First().Key);
                ClassInfo classInfo = classinfoApp.GetFormByF_ClassID(F_Class);
                //教室信息
                Classroom classroom = classInfo.Classroom;
                //设备信息
                Devices device = new Devices();
                if (classroom != null && !Ext.IsEmpty(classroom.F_Classroom_Device_ID))
                {
                    device = deviceApp.GetFormByF_Sn(classroom.F_Classroom_Device_ID);
                }

                //老师信息
                var leadTeacher = new
                {
                    name = teacherUser.UserName,
                    headPic = teacherUser.HeadIcon,
                    className = teacherUser.Classes.First().Value,
                    classRoom = classroom.F_Name
                };

                //班长
                var leadStudent = new object();
                if (classInfo.School_Students_F_Leader_Stu != null)
                {
                    leadStudent = new
                    {
                        name = classInfo.School_Students_F_Leader_Stu.F_Name,
                        headPic = classInfo.School_Students_F_Leader_Stu.F_FacePic_File,
                        userId = classInfo.School_Students_F_Leader_Stu.F_Id
                    };
                }

                //当日考勤统计信息
                var kq = new
                {
                    result = attendanceRulesApp.getClassCheckInfo(classInfo.F_ClassID, DateTime.Now),
                    details = attendanceRulesApp.getClassSignInfoObj(classInfo.F_ClassID, DateTime.Now.ToString("yyyy-MM-dd"))
                };

                //倒计时信息
                var djs = new object();
                if (device != null && DateTime.Now >= device.F_Countdown_StartTime && DateTime.Now <= device.F_Countdown_EndTime)
                {
                    djs = new
                    {
                        //标题
                        djsTitle = device.F_Countdown_Title,
                        //天数
                        djsDays = (device.F_Countdown_EndTime.ToDate() - DateTime.Now).Days.ToString() + "天",
                        F_Countdown_EndTime = device.F_Countdown_EndTime,
                        F_Countdown_StartTime = device.F_Countdown_StartTime
                    };
                }
                //班级成员
                List<Student> students = studentApp.GetList().Where(t => t.F_Class_ID == classInfo.F_ClassID).ToList();
                var stus = new List<object>();
                foreach (Student s in students)
                {
                    var o = new
                    {
                        name = s.F_Name,
                        headPic = s.F_FacePic_File
                    };
                    stus.Add(0);
                }

                //值日生
                var index = (int)DateTime.Now.DayOfWeek;
                index = index == 0 ? 7 : index;
                DateTime d1 = DateTime.Parse((DateTime.Now.AddDays(-(index - 1))).ToString("yyyy-MM-dd") + " 00:00:00");
                DateTime d2 = DateTime.Parse((DateTime.Now.AddDays(7 - index)).ToString("yyyy-MM-dd") + " 23:59:59");

                Class_Duty one = new Class_Duty();
                one.dateIndex = 1;
                one.date = d1.ToDateString();
                one.data = new List<Class_Duty_Item>();
                Class_Duty two = new Class_Duty();
                two.dateIndex = 2;
                two.date = d1.AddDays(1).ToDateString();
                two.data = new List<Class_Duty_Item>();
                Class_Duty three = new Class_Duty();
                three.dateIndex = 3;
                three.date = d1.AddDays(2).ToDateString();
                three.data = new List<Class_Duty_Item>();
                Class_Duty four = new Class_Duty();
                four.dateIndex = 4;
                four.date = d1.AddDays(3).ToDateString();
                four.data = new List<Class_Duty_Item>();
                Class_Duty five = new Class_Duty();
                five.dateIndex = 5;
                five.date = d1.AddDays(4).ToDateString();
                five.data = new List<Class_Duty_Item>();
                Class_Duty six = new Class_Duty();
                six.dateIndex = 6;
                six.date = d1.AddDays(5).ToDateString();
                six.data = new List<Class_Duty_Item>();
                Class_Duty seven = new Class_Duty();
                seven.date = d1.AddDays(6).ToDateString();
                seven.dateIndex = 7;
                seven.data = new List<Class_Duty_Item>();
                List<ClassDuty> classDutyList = schoolClassDutyApp.getClassDuty(classInfo.F_ClassID);
                foreach (ClassDuty d in classDutyList)
                {
                    Class_Duty_Item item = new Class_Duty_Item();
                    List<Student> student = students.Where(t => t.F_Id == d.F_Student).ToList();
                    if (student.Count() > 0)
                    {
                        Student s = student.First();
                        item.id = d.F_Student;
                        item.headPic = s.studentSysUser.F_HeadIcon;
                        item.name = s.F_Name;
                        item.ifLeader = (bool)d.F_IfLeader;
                        switch (d.F_Week)
                        {
                            case 1:
                                one.date = d.F_Date.ToDateString();
                                one.data.Add(item);
                                break;

                            case 2:
                                two.date = d.F_Date.ToDateString();
                                two.data.Add(item);
                                break;

                            case 3:
                                three.date = d.F_Date.ToDateString();
                                three.data.Add(item);
                                break;

                            case 4:
                                four.date = d.F_Date.ToDateString();
                                four.data.Add(item);
                                break;

                            case 5:
                                five.date = d.F_Date.ToDateString();
                                five.data.Add(item);
                                break;

                            case 6:
                                six.date = d.F_Date.ToDateString();
                                six.data.Add(item);
                                break;

                            case 7:
                                seven.date = d.F_Date.ToDateString();
                                seven.data.Add(item);
                                break;
                        }
                    }
                }
                List<Class_Duty> dutyList = new List<Class_Duty>();
                dutyList.Add(one); dutyList.Add(two); dutyList.Add(three); dutyList.Add(four);
                dutyList.Add(five); dutyList.Add(six); dutyList.Add(seven);
                //课程表信息

                //var schedules = schedulesApp.GetSchedulesDataObj(classInfo, d1, d2);

                //任课老师
                List<ClassTeacher> class_teacher = school_Class_Info_Teacher_App.GetFormByF_ClassID(classInfo.F_ClassID);
                var teachers = new List<object>();
                foreach (ClassTeacher s in class_teacher)
                {
                    var o = new
                    {
                        name = s.School_Teachers_Entity.F_Name,
                        headPic = s.School_Teachers_Entity.teacherSysUser.F_HeadIcon,
                        subject = s.School_Course_Entity.F_Name
                    };
                    teachers.Add(o);
                }

                var json = new
                {
                    leadTeacher = leadTeacher,
                    kq = kq,
                    djs = djs,
                    dutyList = dutyList,
                    students = students,
                    teachers = teachers,
                    //schedules = schedules,
                    leadStudent = leadStudent,
                    motto = classInfo.F_Motto
                };
                return Content(NFine.Code.Json.ToJson(json));
            }
        }

        /// <summary>
        /// 获取课程表
        /// </summary>
        /// <param name="entity">   </param>
        /// <param name="F_Class">  </param>
        /// <returns>  </returns>
        //[HttpPost]
        //[HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult getSchedules(string F_Class, DateTime? F_StartTime, DateTime? F_EndTime)
        {
            var index = (int)DateTime.Now.DayOfWeek;
            index = index == 0 ? 7 : index;
            DateTime d1 = DateTime.Parse((DateTime.Now.AddDays(-(index - 1))).ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime d2 = DateTime.Parse((DateTime.Now.AddDays(7 - index)).ToString("yyyy-MM-dd") + " 23:59:59");

            ClassInfo classInfo = classinfoApp.GetFormByF_ClassID(F_Class);

            object schedules = new object();
            try
            {
                if (F_StartTime == null)
                {
                    schedules = schedulesApp.GetSchedulesDataObj(classInfo, d1, d2);
                }
                else
                {
                    schedules = schedulesApp.GetSchedulesDataObj(classInfo, F_StartTime.ToDate(), F_EndTime.ToDate());
                }
                return Content(schedules.ToJson());
            }
            catch //(Exception e)
            {
                return Content(schedules.ToJson());
            }
        }

        /// <summary>
        /// 获取课程表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="F_Class"></param>
        /// <returns></returns>
        //[HttpPost]
        //[HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult getAttendanceInfo(string F_Class, DateTime F_Date)
        {
            try
            {
                var kq = new
                {
                    result = attendanceRulesApp.getClassCheckInfo(F_Class, F_Date),
                    details = attendanceRulesApp.getClassSignInfoObj(F_Class, F_Date.ToString("yyyy-MM-dd"))
                };
                return Content(kq.ToJson());
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult saveClassDuty(string entity, string F_Class)
        {
            if (entity == null)
            {
                return Error("参数错误");
            }
            else
            {
                IEnumerable<Class_Duty> e = NFine.Code.Json.ToObject<IEnumerable<Class_Duty>>(entity.ToString());
                List<ClassDuty> list = new List<ClassDuty>();
                foreach (Class_Duty duty in e)
                {
                    if (duty.data != null)
                    {
                        foreach (Class_Duty_Item i in duty.data)
                        {
                            ClassDuty d = new ClassDuty();
                            d.F_Week = duty.dateIndex;
                            d.F_Date = DateTime.Parse(duty.date);
                            d.F_Class = F_Class;
                            d.F_Student = i.id;
                            d.F_IfLeader = i.ifLeader;
                            list.Add(d);
                        }
                    }
                }
                schoolClassDutyApp.saveClassDuty(list);
                return Success("操作成功。");
            }
        }

        /// <summary>
        /// 倒计时设置
        /// </summary>
        /// <param name="classId">                </param>
        /// <param name="F_Countdown_Title">      </param>
        /// <param name="F_Countdown_StartTime">  </param>
        /// <param name="F_Countdown_EndTime">    </param>
        /// <returns>  </returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult djsSet(string F_Class, string F_Countdown_Title, DateTime? F_Countdown_StartTime, DateTime? F_Countdown_EndTime)
        {
            List<Devices> devices = deviceApp.GetList(t => t.F_Class == F_Class);
            string djsDays = "";
            if (devices.Count() == 1)
            {
                Devices device = devices.First();
                Devices tmp = new Devices();
                tmp.F_Countdown_Title = F_Countdown_Title;
                tmp.F_Countdown_StartTime = F_Countdown_StartTime;
                tmp.F_Countdown_EndTime = F_Countdown_EndTime;
                deviceApp.djsSet(tmp, device.F_Id);
                djsDays = (F_Countdown_EndTime.ToDate() - DateTime.Now).Days.ToString() + "天";
            }
            else
            {
                return Error("该班级没有绑定班牌");
            }
            return Success("操作成功", djsDays);
        }

        [HttpPost]
        //[HandlerAuthorize]
        //[HandlerAjaxOnly]
        public ActionResult clearDjs(string F_Class)
        {
            List<Devices> devices = deviceApp.GetList(t => t.F_Class == F_Class);
            if (devices.Count() == 1)
            {
                deviceApp.clearDjs(devices.First().F_Id);
            }
            else
            {
                return Error("该班级没有绑定班牌");
            }
            return Success("操作成功。");
        }

        /// <summary>
        /// 标语设置
        /// </summary>
        /// <param name="classId">  </param>
        /// <param name="F_Motto">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult bySet(string F_Class, string F_Motto)
        {
            List<Devices> devices = deviceApp.GetList(t => t.F_Class == F_Class);
            if (devices.Count() == 1)
            {
                ClassInfo classinfo = classinfoApp.GetFormByF_ClassID(F_Class);
                classinfo.F_Motto = F_Motto;
                classinfoApp.UpdateForm(classinfo);
            }
            else
            {
                return Error("该班级没有绑定班牌");
            }
            return Success("操作成功。");
        }
    }

    public class Class_Duty
    {
        public int dateIndex { get; set; }
        public string date { get; set; }
        public List<Class_Duty_Item> data { get; set; }
    }

    public class Class_Duty_Item
    {
        public string name { get; set; }
        public string id { get; set; }
        public string headPic { get; set; }
        public bool ifLeader { get; set; }
    }
}