namespace ZHXY.Domain
{
    /// <summary>
    /// 智慧校园常量
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public class SmartCampusConsts
    {
        #region 系统相关

        public const string EntityMappingAssmblyName = "ZHXY.Mapping";

        #endregion 系统相关

        //缓存key
        public const string DATAITEMS = "dataItems";

        public const string DATAVALUES = "dataValues";
        public const string ORGANIZE = "organize";
        public const string ROLE = "role";
        public const string DUTY = "duty";
        public const string AUTHORIZEMENU = "authorizeMenu";
        public const string AUTHORIZEBUTTON = "authorizeButton";
        public const string AREA = "area";
        public const string SCHOOLAREA = "schoolarea";
        public const string AREACHILD = "areachild";
        public const string SCHOOLAREACHILD = "schoolareachild";
        public const string USERS = "users";
        public const string DEVICES = "devices";
        public const string SCHEDULESTIME = "schedulestime";
        public const string COURSE = "course";

        //数据权限
        //1	所有数据	all
        //7	仅个人数据	Personal
        //8	仅当前机构数据（不含下级）	CurrentDep
        //9	自定义	Diy
        public const string ALL = "all";

        public const string PERSONAL = "Personal";
        public const string CURRENTDEP = "CurrentDep";
        public const string DIY = "Diy";

        //智慧学校
        public const string SEMESTER = "semester";

        public const string CLASSTEACHERS = "classTeachers";

        public const string LEAVETYPE = "LeaveType";

        //邮件
        public const string InBox = "InBox";

        public const string OutBox = "OutBox";
        public const string TrashBox = "TrashBox";//回收站
        public const string DraftBox = "DraftBox";//草稿箱

        //资产管理

        public const string PurchaseApprover = "PurchaseApprover";//采购审批人
        public const string PurchaseReceiver = "PurchaseReceiver";//采购接收人



        // 关系名称
        /// <summary>
        /// 闸机和楼栋的关系
        /// </summary>
        public const string REL_GATE_BUILDING = "GATE_BUILDING";

    }
}