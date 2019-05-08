namespace ZHXY.Domain
{
    /// <summary>
    /// 订单状态
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public enum OrderStatus
    {
        新建 = 1,
        已作废 = 2,
        待审批 = 3,
        审批不通过 = 4,
        审批通过 = 5,
        完成 = 6
    }
}