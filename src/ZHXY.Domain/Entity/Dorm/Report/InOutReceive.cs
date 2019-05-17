
namespace ZHXY.Domain
{
    public class InOutReceive : IEntity
    {

        public string Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int? Type{ get; set; }
        
        /// <summary>
        /// 接收用户
        /// </summary>
        public string ReceiveUser{ get; set; }
        
                
    }
}
