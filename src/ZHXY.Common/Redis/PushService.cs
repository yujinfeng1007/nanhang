namespace ZHXY.Common
{
    /// <summary>
    ///     Redis操作
    /// </summary>
    public class PushService
    {
        /// <summary>
        /// 推送到班牌设备
        /// </summary>
        /// <param name="code">推送代码</param>
        /// <param name="deviceSnArr">设备Sn列表</param>
        public static void Push( int code, params string[] deviceSnArr)
        {
            if (deviceSnArr == null||deviceSnArr.Length==0) return;
            var redisDb = RedisHelper.GetDatabase();
            foreach (var item in deviceSnArr)
            {
                redisDb.ListRightPush("pushToDevice", new { DeviceSN = item, Mseeage = code }.ToJson());
            }
        }

    }
}