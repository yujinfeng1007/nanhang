namespace ZHXY.Domain
{
    /// <summary>
    /// 订单类型
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public enum OrderType : byte
    {
        入库 = 1,
        出库 = 2,
        采购 = 3,
        调拨 = 4,
        报废 = 5,
        领用 = 6,
        盘点 = 7,
        借用 = 8,
        维修 = 9,
        续用 = 10
    }
}