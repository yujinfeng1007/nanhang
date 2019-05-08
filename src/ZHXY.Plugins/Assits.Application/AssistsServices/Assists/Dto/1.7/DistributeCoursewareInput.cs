using System.Collections.Generic;
using ZHXY.Application;
namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 1.7.下发课件接口输入
    /// author: 余金锋
    /// phone:  l33928l9OO7
    /// email:  2965l9653@qq.com
    /// </summary>
    public class DistributeCoursewareInput : BaseApiInput
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public string F_CourseId { get; set; }

        /// <summary>
        /// 教师Id
        /// </summary>
        public string F_TeacherId { get; set; }

        /// <summary>
        /// 文件流（可支持多个）
        /// </summary>
        public string F_File { get; set; }

        /// <summary>
        /// 下发学生对象Id
        /// </summary>
        public List<string> F_StudentIds { get; set; }
    }
}