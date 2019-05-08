using Quartz;

namespace TaskApi.DH
{
    /// <summary>
    /// 创建宿舍（大华）
    /// </summary>
    class DHCreateDormJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ////创建楼栋
            //Dictionary<string, string> BuildDic = new Dictionary<string, string>();
            ////List<JObject> FloorList = new List<JObject>();
            //string SelectBuildSql = "SELECT F_Building_No FROM [dbo].[Dorm_DormInfo] GROUP BY F_Building_No;";
            //DataTable BuildDataTable = SqlHelper.ExecuteDataTable(SelectBuildSql);
            //string pid = "dormitory001"; //分院ID（南航大学）
            //foreach (DataRow row in BuildDataTable.Rows)
            //{
            //    string name = row[0].ToString();
            //    JObject jo = (JObject)JsonConvert.DeserializeObject(DHAccount.CREATE_DORMITORY(name + "栋", pid).ToString());
            //    bool flag = jo.Value<bool>("success");
            //    if (flag)
            //    {
            //        Console.WriteLine("创建 " + name + " 栋 成功！");
            //        string id = jo.Value<JObject>("data").Value<string>("id");
            //        BuildDic.Add(name, id);
            //    }
            //}

            ////通过楼栋创建楼层
            ////string SelectFloorSql = "SELECT F_Building_No, F_Floor_No FROM [dbo].[Dorm_DormInfo] GROUP BY F_Building_No, F_Floor_No;";
            ////DataTable FloorDataTable = SqlHelper.ExecuteDataTable(SelectFloorSql);
            ////foreach(DataRow row in FloorDataTable.Rows)
            ////{
            ////    string buildingNo = row[0].ToString();
            ////    string floorNo = row[1].ToString();
            ////    string FloorPid = BuildDic[buildingNo].ToString();
            ////    JObject jo = (JObject)JsonConvert.DeserializeObject(DHAccount.CREATE_DORMITORY(floorNo + "楼", FloorPid).ToString());
            ////    bool flag = jo.Value<bool>("success");
            ////    if (flag)
            ////    {
            ////        Console.WriteLine("创建 " + buildingNo + " 栋, " + floorNo + " 楼 成功！");
            ////        string id = jo.Value<JObject>("data").Value<string>("id");
            ////        JObject FloorJO = new JObject();
            ////        FloorJO.Add("id", id);
            ////        FloorJO.Add("name", floorNo);
            ////        FloorJO.Add("buildingNo", buildingNo);
            ////        FloorList.Add(FloorJO);
            ////    }
            ////}

            ////通过楼栋和楼层，创建宿舍
            //string SelectDormSql = "SELECT F_Building_No, F_Floor_No,F_Memo FROM [dbo].[Dorm_DormInfo] GROUP BY F_Building_No, F_Floor_No, F_Memo;";
            //DataTable DormDataTable = SqlHelper.ExecuteDataTable(SelectDormSql);
            //foreach (DataRow row in DormDataTable.Rows)
            //{
            //    string BuildName = row[0].ToString();
            //    string floorName = row[1].ToString();
            //    string dormName = row[2].ToString();

            //    string BuildId = BuildDic[BuildName];
            //    JObject jo = (JObject)JsonConvert.DeserializeObject(DHAccount.CREATE_DORMITORY(dormName, BuildId).ToString());
            //    bool flag = jo.Value<bool>("success");
            //    if (flag)
            //    {
            //        Console.WriteLine("创建 " + BuildName + " 栋, " + floorName + " 楼, " + dormName + " 成功！");
            //    }
            //    //foreach(JObject job in FloorList)
            //    //{
            //    //    string floorId = job.Value<string>("id");
            //    //    string floorNameP = job.Value<string>("name");
            //    //    string BuildNameP = job.Value<string>("buildingNo");

            //    //    if(floorName.Equals(floorNameP) && BuildName.Equals(BuildNameP))
            //    //    {
            //    //        JObject jo = (JObject)JsonConvert.DeserializeObject(DHAccount.CREATE_DORMITORY(dormName, floorId).ToString());
            //    //        bool flag = jo.Value<bool>("success");
            //    //        if (flag)
            //    //        {
            //    //            Console.WriteLine("创建 " + BuildNameP + " 栋, " + floorNameP + " 楼, " + dormName + " 成功！");
            //    //        }
            //    //    }
            //    //}
            //}
        }
    }
}
