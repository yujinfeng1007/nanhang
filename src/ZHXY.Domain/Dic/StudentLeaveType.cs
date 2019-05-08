namespace ZHXY.Domain
{
    /// <summary>
    /// 学生请假类型
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public enum StudentLeaveType
    {
        事假 = 1,
        病假 = 2,
    }

    /// <summary>
    /// 请假审批状态
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public enum StudentLeaveStatus
    {
        未审批 = 1,
        同意 = 2,
        不同意 = 3,
    }
}