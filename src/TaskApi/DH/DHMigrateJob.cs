using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using ZHXY.Common;
using ZHXY.Domain;

namespace TaskApi.DH
{
    public class DHMigrateJob : IJob
    {
        public static string REDIS_LINE_RECORD_SET_KEY = "huawang_redis_key"; //流水数据存储Redis的key
        public static int REDIS_PROCESS_COUNT = 500; //每次处理的数据长度
        public static int REDIS_LINE_RECORD_DB_LEVEL = 15;

        public void Execute(IJobExecutionContext context)
        {
            IDatabase db = RedisHelper.GetDatabase(REDIS_LINE_RECORD_DB_LEVEL);
            RedisValue[] valueArr = db.SetRandomMembers(REDIS_LINE_RECORD_SET_KEY, REDIS_PROCESS_COUNT);
            Console.WriteLine("处理闸机流水信息,处理数据长度为： " + valueArr.Length + "   DateTime : " + DateTime.Now.ToString());
            List<LineRecordMoudle> ListLineRecord = new List<LineRecordMoudle>();
            for (int i = 0; i < valueArr.Length; i++)
            {
                JObject jo = ((JObject)JsonConvert.DeserializeObject(valueArr[i])).Value<JObject>("info");
                jo.Add("id", Guid.NewGuid().ToString());
                jo.Add("date", DateTime.Now);
                LineRecordMoudle line = JsonConvert.DeserializeObject<LineRecordMoudle>(JsonConvert.SerializeObject(jo));
                ListLineRecord.Add(line);
            }

            if(ListLineRecord.Count > 0)
            {
                InsertLineRecordToSqlServer(ListLineRecord); //入库 SqlServer
                db.SetRemove(REDIS_LINE_RECORD_SET_KEY, valueArr); //从Set集合中，删除部分已处理数据
            }
        }

        public void InsertLineRecordToSqlServer(List<LineRecordMoudle> lineRecordMoudles)
        {
            //有可能一次性获取的流水记录有当前月份和上个月的（月份交替，凌晨时分的数据）
            List<LineRecordMoudle> LastMonthList = new List<LineRecordMoudle>();  //上个月数据入库集合
            List<LineRecordMoudle> CurrentMonthList = new List<LineRecordMoudle>(); //当前月份数据入库集合

            DateTime dateTitle = GetDateTime(Int32.Parse(lineRecordMoudles[0].swipDate));
            int TableIntTitle = Int32.Parse(dateTitle.Year + "" + dateTitle.Month);
            string tableNameTitle = "DHFLOW_" + dateTitle.Year + dateTitle.Month.ToString().PadLeft(2, '0');
            string tableNameOther = null;
            foreach (LineRecordMoudle lineRecord in lineRecordMoudles)
            {
                DateTime date = GetDateTime(Int32.Parse(lineRecord.swipDate));
                int TableInt = Int32.Parse(date.Year + "" + date.Month);
                if(TableInt == TableIntTitle)
                {
                    LastMonthList.Add(lineRecord);
                }
                else
                {
                    tableNameOther = "DHFLOW_" + date.Year + date.Month.ToString().PadLeft(2, '0');
                    CurrentMonthList.Add(lineRecord);
                }
            }

            //批量插入
            if(LastMonthList.Count != 0)
            {
                InsertSql(LastMonthList, tableNameTitle);
            }

            if(CurrentMonthList.Count != 0)
            {
                InsertSql(CurrentMonthList, tableNameOther);
            }
        }

       public void  InsertSql(List<LineRecordMoudle> RecordList , string tableName)
        {
            SqlHelper.CheckExistsTable(tableName, CreateSqlStr(tableName));
            StringBuilder InsertSql = new StringBuilder("INSERT INTO [dbo].["+ tableName + "]([code],[id], [date], [channelCode], [channelName], " +
                "[departmentCode], [departmentName], [cardNum], [firstName], [lastName], [tel], [gender], [idNum], [personId], [cardType], " +
                "[inOut], [eventType], [deviceType], [swipDate], [picture1], [picture2], [picture3], [picture4], [memo], [alarmCode], [pictureUrl]) VALUES ");
            foreach(LineRecordMoudle record in RecordList)
            {
                InsertSql.Append("('"+ record .code+ "', '" + record.id + "', '" + record.date + "', '" + record.channelCode
                    + "', '"+record.channelName + "', '"+record.departmentCode + "', '"+record.departmentName 
                    + "', '"+record.cardNum + "', '"+record.firstName + "', '"+record.lastName+"', '"+record.tel 
                    + "', '"+record.gender + "', '"+record.idNum + "', '"+record.personId + "', '"+record.cardType 
                    + "', '"+record.inOut + "', '"+record.eventType + "', '"+record.deviceType + "', '"+record.swipDate 
                    + "', '"+record.picture1 + "', '"+record.picture2 + "', '"+record.picture3 + "', '"+record.picture4 + "', '"+record.memo + "', '"+record.alarmCode + "', '"+record.pictureUrl + "'),");
            }
            SqlHelper.ExecuteNonQuery(InsertSql.ToString().Substring(0, InsertSql.Length-1));
        }

        /// <summary>
        /// 获取时间戳Timestamp  
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private int GetTimeStamp(DateTime dt)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
            return timeStamp;
        }

        /// <summary>
        /// 时间戳Timestamp转换成日期
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime GetDateTime(int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = ((long)timeStamp * 10000000);
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime targetDt = dtStart.Add(toNow);
            return targetDt;
        }

        private string CreateSqlStr(string tableName)
        {
            return "if not exists (select * from sysobjects where id = object_id('"+tableName+"') "+
                    "and OBJECTPROPERTY(id, 'IsUserTable') = 1) "+
                    " CREATE TABLE [dbo].["+ tableName + "] (\n" +
                "  [id] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,\n" +
                "  [date] datetime  NULL,\n" +
                "  [code] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [channelCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [channelName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [departmentCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [departmentName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [cardNum] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [firstName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [lastName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [tel] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [gender] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [idNum] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [personId] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [cardType] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [inOut] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [eventType] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [deviceType] varchar(10) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [swipDate] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [picture1] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [picture2] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [picture3] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [picture4] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [memo] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [alarmCode] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  [pictureUrl] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,\n" +
                "  CONSTRAINT ["+"PK_"+tableName+"] PRIMARY KEY CLUSTERED ([id])\n" +
                "WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  \n" +
                "ON [PRIMARY]\n" +
                ") ";
        }
    }
}
