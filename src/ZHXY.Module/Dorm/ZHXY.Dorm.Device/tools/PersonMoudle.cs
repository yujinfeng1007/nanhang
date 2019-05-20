using System;

namespace ZHXY.Dorm.Device.tools
{
    public class PersonMoudle
    {
        public int id { get; set; }
        public string accessCardsn { get; set; } //门禁卡号
        public string code { get; set; } //学工号
        public string colleageClass { get; set; } //班级编号
        public string colleageCode { get; set; } //院系编号
        public string colleageGrade { get; set; } //年级编号
        public string colleageMajor { get; set; } //专业编号
        public string contactNum { get; set; } //联系电话
        public string dormitoryBed { get; set; } //床位编号
        public string dormitoryCode { get; set; } //宿舍楼栋编号
        public string dormitoryFloor { get; set; } //楼层编号 
        public string dormitoryRoom { get; set; } //寝室编号
        public string idCode { get; set; } //身份证号
        public string name { get; set; } //姓名
        public string orgId { get; set; } //所在组织编号
        public int pageNum { get; set; } //查询的起始页数
        public int pageSize { get; set; }//每页查询的条数
        public string param { get; set; } = "string"; //查询条件
        public string rfidCardsn { get; set; } //rfid卡号
        public string roleId { get; set; } //类型id
        public int sex { get; set; } = 0;//性别0未知1男2女
        public string colleageArea { get; set; } //校区编号
        public string dormitoryArea { get; set; } //园区编号
        public string dormitoryLeader { get; set; } //寝室长
        public string dormitoryLeaderPhone { get; set; } //寝室长电话
        public string emergencyPerson { get; set; } //紧急联系人
        public string emergencyPersonPhone { get; set; } //紧急联系人电话
        public string instructorCode { get; set; }//辅导员学工号
        public string instructorName { get; set; } //辅导员姓名
        public string instructorPhone { get; set; }//辅导员电话
        public Array managementClassList { get; set; } //辅导员管理班级
        public string photoBase64 { get; set; } //Base64格式的照片
        public string photoUrl { get; set; } //照片路径
        public string resume { get; set; } //履历

        public string dormitoryAreaName { get; set; } = "宿舍管理";
        public string dormitoryName { get; set; }
        public string dormitoryFloorName { get; set; }
        public string dormitoryRoomName { get; set; }

    }
}
