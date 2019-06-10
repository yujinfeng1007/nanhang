namespace ZHXY.Domain
{
    /// <summary>
    /// 机构负责人
    /// </summary>
    public class OrgLeader:IEntity
    {
        public string OrgId { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Org Org { get; set; }
    }
}