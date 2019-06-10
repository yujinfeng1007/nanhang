namespace ZHXY.Common
{
    //南航项目- 学生信息生成Excel下放大华闸机
    public class DHStudentMoudle
    {
        public string name { get; set; }
        public string sex { get; set; }
        public string StudentNum { get; set; }
        public string OrgName { get; set; } = "南昌航空大学";
        public string CredNum { get; set; }
        public string type { get; set; } = "学生";
        public string ColleageCode { get; set; } = "宿舍管理";
        public string BuildName { get; set; }
        public string FloorName { get; set; }
        public string DormName { get; set; }

        public string DormId { get; set; }
    }

    //南航项目- 教师信息尼生成Excel下放大华闸机
    public class DHTeacherMoudle
    {
        public string name { get; set; }
        public string sex { get; set; }
        public string TeacherNo { get; set; }
        public string OrgName { get; set; } = "南昌航空大学";
        public string CredNum { get; set; }
        public string type { get; set; } = "教职工";
        public string ColleageCode { get; set; } = "";
        public string BuildName { get; set; }
        public string FloorName { get; set; }
        public string DormName { get; set; }
    }
}
