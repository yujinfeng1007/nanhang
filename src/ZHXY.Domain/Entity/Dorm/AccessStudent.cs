namespace ZHXY.Domain
{
    public class AccessStudent : Entity
    {

        public string F_DeviceId { get; set; }

        public string F_UserName { get; set; }

        public string F_UserType { get; set; }

        public string F_UserNum { get; set; }

        public string F_UserId { get; set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Gate Device { get; set; }
    }
}
