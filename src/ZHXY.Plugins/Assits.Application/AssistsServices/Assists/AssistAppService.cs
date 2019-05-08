using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Assists.Entity;
using ZHXY.Common;
using ZHXY.Repository;

namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 辅助屏服务
    ///
    /// </summary>
    public partial class AssistAppService : AppService
    {
        private SysUserAppService UserAppService { get; }

        public AssistAppService(IAssistManageRepository repos, SysUserAppService userAppService)
        {
            R = repos;
            UserAppService = userAppService;
        }

        public AssistAppService()
        {
            R = new AssistManageRepository();
            UserAppService = new SysUserAppService();
        }

        #region ApiService

        /// <summary>
        /// 1.2.辅助屏绑定教室
        /// </summary>
        public BindRoomOutput Bind(BindRoominput input)
        {
            if (!Read<Classroom>(p => p.F_Id.Equals(input.F_ClassRoom_Id)).Any()) throw new Exception($"未找到教室:教室Id={input.F_ClassRoom_Id}");
            if (Read<DevicesEntity>(p => p.F_Classroom.Equals(input.F_ClassRoom_Id)).Any()) throw new Exception("教室已被其他设备绑定,请检查!");
            var device = Query<DevicesEntity>(p => p.F_Sn.Equals(input.F_Sn)).FirstOrDefault();
            if (null == device)
            {
                device = new DevicesEntity();
                device.Create();
                device.F_Sn = input.F_Sn;
                device.F_Classroom = input.F_ClassRoom_Id;
                Add(device);
            }
            else
            {
                if (!string.IsNullOrEmpty(device.F_Classroom)) throw new Exception("班牌已绑定教室,请先解绑!");
                device.F_Classroom = input.F_ClassRoom_Id;
            }

            SaveChanges();
            return GetBindRoom(input.F_ClassRoom_Id);
        }

        /// <summary>
        /// 1.3.辅助屏教师登录
        /// </summary>
        public TeacherLoginOutput TeacherLogin(TeacherLoginInput input)
        {
            var user = UserAppService.CheckLogin(input.F_User, input.F_Pwd);

            return new TeacherLoginOutput
            {
                F_Id = user.F_Id,
                F_Name = user.F_RealName,
                F_HeadPic = user.F_HeadIcon
            };
        }

        /// <summary>
        /// 1.4.获取教室当天课程表
        /// </summary>
        public List<GetRoomCourseOfDayOutput> GetRoomCourseOfDay(GetRoomCourseOfDayInput input)
        {
            var date = DateTime.Parse(input.F_Date).Date;
            var classroomId = Read<DevicesEntity>(p => p.F_Sn.Equals(input.F_Sn)).Select(p => p.F_Classroom).FirstOrDefault();

            return Read<SchScheduleMoveCourse>(p => p.F_Classroom.Equals(classroomId) && p.F_Date == date).Select(p => new GetRoomCourseOfDayOutput
            {
                F_CourseId = p.F_Course_PrepareID,
                F_CourseName = p.Course.F_Name,
                F_LessonNo = p.F_CourseIndex.ToString(),
                F_TimeSelect = ""
            }).OrderBy(p => p.F_LessonNo).ToListAsync().Result;
        }

        /// <summary>
        /// 1.5.获取课程学生信息
        /// </summary>
        public List<GetCourseStudentsOutput> GetCourseStudents(GetCourseStudentsInput input)
        {
            var classroomId = Read<DevicesEntity>(p => p.F_Sn.Equals(input.F_Sn)).Select(p => p.F_Classroom).FirstOrDefault();
            var classId = Read<ClassInfo>(p => p.F_Classroom.Equals(classroomId)).Select(p => p.F_ClassID).FirstOrDefault();
            var students = Read<Student>(p => p.F_Class_ID.Equals(classId)).Select(p => new GetCourseStudentsOutput
            {
                F_Id = p.F_Id,
                F_Name = p.F_Name,
                F_HeadPic = p.F_FacePic_File
            }).ToList();
            var type = input.F_Type;
            switch (type)
            {
                case GetCourseStudentsInput.Type.已签到:
                    {
                        // 剔除未签到的
                        break;
                    }
                case GetCourseStudentsInput.Type.未签到:
                    {
                        //剔除已签到的
                        break;
                    }
            }
            return students;
        }

        /// <summary>
        /// 1.6.获取课堂最新问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<GetCourseLatestQuestionsOutput> GetCourseLatestQuestions(GetCourseLatestQuestionsInput input)
        {
            var classroomId = Read<DevicesEntity>(p => p.F_Sn.Equals(input.F_Sn)).Select(p => p.F_Classroom).FirstOrDefault();
            var classId = Read<ClassInfo>(p => p.F_Classroom.Equals(classroomId)).Select(p => p.F_Id).FirstOrDefault();
            var studentIds = Read<Student>(p => p.F_Class_ID.Equals(classId)).Select(p => p.F_Users_ID).ToArray();
            return Read<Question>(p => p.CourseId.Equals(input.F_CourseId) && studentIds.Contains(p.AskerId)).OrderByDescending(p => p.CreatedTime)
                .Select(p => new GetCourseLatestQuestionsOutput
                {
                    F_Id = p.Id,
                    F_Title = p.Content,
                    F_Time = p.CreatedTime,
                    F_Student = new StudentDto
                    {
                        F_Id = p.Asker.F_Id,
                        F_Name = p.Asker.F_RealName,
                        F_HeadPic = p.Asker.F_HeadIcon
                    }
                }).ToListAsync().Result;
        }

        /// <summary>
        /// 1.7.下发课件
        /// </summary>
        /// <param name="input"></param>
        public void DistributeCourseware(DistributeCoursewareInput input)
        {
        }

        /// <summary>
        /// 1.9.获取未绑定辅助屏的教室信息
        /// </summary>
        public GetUnBindRoomOutput GetUnboundRoom(BaseApiInput input)
        {
            var result = new GetUnBindRoomOutput
            {
                F_Is_Binded = Read<DevicesEntity>(p => p.F_Sn.Equals(input.F_Sn) && !string.IsNullOrEmpty(p.F_Classroom)).AnyAsync().Result
            };
            if (result.F_Is_Binded)
            {
                var classroomId = Read<DevicesEntity>(p => p.F_Sn.Equals(input.F_Sn)).Select(p => p.F_Classroom).FirstOrDefaultAsync().Result;
                result.F_RoomInfo = GetBindRoom(classroomId);
                return result;
            }
            var bindedRooms = Read<DevicesEntity>(p => !string.IsNullOrEmpty(p.F_Classroom)).Select(p => p.F_Classroom).ToArrayAsync().Result;
            result.F_Rooms = Read<Classroom>(p => !bindedRooms.Contains(p.F_Id)).Select(p => new RoomOutput
            {
                F_Id = p.F_Id,
                F_Building_No = p.F_Building_No,
                F_Floor_No = p.F_Floor_No,
                F_Classroom_No = p.F_Classroom_No,
                F_Name = p.F_Name
            }).ToListAsync().Result;
            return result;
        }

        #endregion ApiService

        #region MvcService

        public void Unbind(string id)
        {
            var entity = Get<DevicesEntity>(id);
            entity.F_Classroom = null;
            SaveChanges();
        }

        public void Update(UpdateDeviceDto dto)
        {
            var device = Get<DevicesEntity>(dto.Id);
            if (device == null) return;
            device.F_Device_Code = dto.DeviceCode;
            device.F_Device_Name = dto.DeviceName;
            device.F_Display_Style = dto.DisplayMode;
            device.F_Device_Status = dto.Status;
            device.F_Brand = dto.Brand;
            device.F_Size = dto.Size;
            device.F_TeaMac = dto.Mac;
            SaveChanges();
            PushService.Push( 9,new[] { device.F_Sn });
        }

        public dynamic Get(string id)
        {
            return Read<DevicesEntity>(t => t.F_Id == id).Select(p => new
            {
                Id = p.F_Id,
                DeviceCode = p.F_Device_Code,
                DeviceName = p.F_Device_Name,
                DisplayMode = p.F_Display_Style,
                Status = p.F_Device_Status,
                Brand = p.F_Brand,
                Size = p.F_Size,
                Mac = p.F_TeaMac,
                DeviceIp = p.F_IP,
                Sn = p.F_Sn,
                SysVersion = p.F_SystemNo,
                ApkVersion = p.F_ApkNo
            }).FirstOrDefaultAsync().Result;
        }

        public void Delete(string id)
        {
            DelAndSave<DevicesEntity>(id);
        }

        public dynamic GetBindInfo(string id)
        {
            var dev = Read<DevicesEntity>(p => p.F_Id.Equals(id)).FirstOrDefault();
            if (dev == null) return null;
            var classroom = Read<Classroom>(p => p.F_Id.Equals(dev.F_Classroom)).Select(p => new { Id = p.F_Id, Name = p.F_Name }).FirstOrDefault();
            if (null == classroom) return null;
            return new
            {
                ClassroomName = classroom.Name,
                ClassName = Read<ClassInfo>(p => p.F_Classroom.Equals(classroom.Id)).Select(p => p.F_Name).FirstOrDefault()
            };
        }

        public object GetList(Pagination pagination, string keyword, string displayStyle, string status)
        {
            var query = Read<DevicesEntity>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.F_Sn.Contains(keyword) || p.F_Device_Code.Contains(keyword) || p.F_Device_Name.Contains(keyword));
            query = string.IsNullOrEmpty(displayStyle) ? query : query.Where(p => p.F_Display_Style.Equals(displayStyle));
            query = string.IsNullOrEmpty(displayStyle) ? query : query.Where(p => p.F_Device_Status.Equals(status));
            pagination.CheckSort<DevicesEntity>();
            return query.OrderBy(pagination.Sidx)
                .Select(p => new
                {
                    Id = p.F_Id,
                    DeviceCode = p.F_Device_Code,
                    Sn = p.F_Sn,
                    Status = p.F_Device_Status,
                    Address = p.Classroom.F_Name,
                    DisplayMode = p.F_Display_Style
                })
                .Skip(pagination.Skip).Take(pagination.Rows).ToListAsync().Result;
        }

        #endregion MvcService

        private BindRoomOutput GetBindRoom(string classroomId)
        {
            var school = Read<SchInfo>()
                .Select(p => new { Name = p.F_Name, Logo = p.F_Logo })
                .FirstOrDefault();
            var room = Read<Classroom>(p => p.F_Id.Equals(classroomId))
                .Select(p => new { Id = p.F_Id, Name = p.F_Name })
                .FirstOrDefault();
            return new BindRoomOutput
            {
                F_SchoolName = school?.Name,
                F_Logo = school?.Logo,
                F_Id = room?.Id,
                F_RoomName = room?.Name
            };
        }
    }
}