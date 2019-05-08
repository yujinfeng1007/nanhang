using System;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class NoSeedRepository : Data.Repository<NoSeed>, INoSeedRepository
    {
        private string getStrNum(string num, int leng = 4)
        {
            var len = num.Length;
            for (var i = 0; i < leng - len; i++) num = "0" + num;
            return num;
        }

        public string getStuNum(int F_Year, string F_Divis_ID)
        {
            //using (var db = new RepositoryBase().BeginTrans())
            //{
            var db = new Data.UnitWork();
            var org = db.FindEntity<SysOrganize>(t => t.F_Id == F_Divis_ID);
            var seed = db.FindEntity<NoSeed>(
                t => t.F_Divis == F_Divis_ID && t.F_Year == F_Year && t.F_Type == "student");

            var no = org.F_EnCode + F_Year;
            if (seed == null)
            {
                seed = new NoSeed();
                seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                seed.F_Divis = F_Divis_ID;
                seed.F_Year = F_Year;
                seed.F_Type = "student";
                seed.F_No = 1;
                db.Insert(seed);
            }
            else
            {
                seed.F_No = seed.F_No + 1;
                db.Update(seed);
            }

            no = no + getStrNum(Convert.ToString(seed.F_No));
            //db.Commit();
            return no;
            //}
        }

     

        /// <summary>
        ///     学生学号
        /// </summary>
        /// <param name="F_Year">年级</param>
        /// <param name="F_Divis_ID"></param>
        /// <returns></returns>
        string INoSeedRepository.GetStuNum(int F_Year, string F_Divis_ID)
        {
            //using (var db = new RepositoryBase().BeginTrans())
            //{
            var db = new Data.UnitWork();
            var org = db.FindEntity<SysOrganize>(t => t.F_Id == F_Divis_ID);
            var seed = db.FindEntity<NoSeed>(
                t => t.F_Divis == F_Divis_ID && t.F_Year == F_Year && t.F_Type == "student");

            var no = org.F_EnCode + F_Year;
            if (seed == null)
            {
                seed = new NoSeed
                {
                    F_Id = Guid.NewGuid().ToString("N").ToUpper(),
                    F_Divis = F_Divis_ID,
                    F_Year = F_Year,
                    F_Type = "student",
                    F_No = 1
                };
                db.Insert(seed);
            }
            else
            {
                seed.F_No = seed.F_No + 1;
                db.Update(seed);
            }

            no = no + getStrNum(Convert.ToString(seed.F_No));
            //db.Commit();
            return no;
            //}
        }

        /// <summary>
        ///     教职工工号
        /// </summary>
        /// <param name="F_Year"></param>
        /// <param name="F_Duty">部门编码</param>
        /// <param name="F_Divis_ID"></param>
        /// <returns></returns>
        string INoSeedRepository.GetTeacherNum(int F_Year, string F_Duty, string F_Divis_ID)
        {
            var no = F_Duty + F_Year;
            //using (var db = new RepositoryBase().BeginTrans())
            //{
            var db = new Data.UnitWork();
            var seed = db.FindEntity<NoSeed>(
                t => t.F_Divis == F_Divis_ID && t.F_Year == F_Year && t.F_Type == "teacher");
            if (seed == null)
            {
                seed = new NoSeed();
                seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                seed.F_Divis = F_Divis_ID;
                seed.F_Year = F_Year;
                seed.F_Type = "teacher";
                seed.F_No = 1;
                db.Insert(seed);
            }
            else
            {
                seed.F_No = seed.F_No + 1;
                db.Update(seed);
            }

            no += getStrNum(Convert.ToString(seed.F_No));
            //db.Commit();
            return no;
            // }
        }

        public string GetStuInNum(int F_Year, string F_Divis_ID) =>
            //string no = "SU-" + F_Divis_ID + F_Year;
            //using (var db = new RepositoryBase().BeginTrans())
            //{
            //    School_No_Seed_Entity seed = db.FindEntity<School_No_Seed_Entity>(t => t.F_Divis == F_Divis_ID && t.F_Year == F_Year && t.F_Type == "student");
            //    if (seed == null)
            //    {
            //        seed = new School_No_Seed_Entity();
            //        seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
            //        seed.F_Divis = F_Divis_ID;
            //        seed.F_Year = F_Year;
            //        seed.F_Type = "su";
            //        seed.F_No = 0;
            //        db.Insert(seed);
            //    }
            //    no = no + getStrNum(Convert.ToString(seed.F_No + 1),6);
            //    seed.F_No = seed.F_No + 1;
            //    db.Update(seed);
            //    db.Commit();
            //}
            //return no;
            "s" + NumberBuilder.Build_18bit();

        public string GetStuExamNum(int F_Year, string F_Divis_ID) =>
            //string no = "EX-" + F_Divis_ID + F_Year;
            //using (var db = new RepositoryBase().BeginTrans())
            //{
            //    School_No_Seed_Entity seed = db.FindEntity<School_No_Seed_Entity>(t => t.F_Divis == F_Divis_ID && t.F_Year == F_Year && t.F_Type == "student");
            //    if (seed == null)
            //    {
            //        seed = new School_No_Seed_Entity();
            //        seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
            //        seed.F_Divis = F_Divis_ID;
            //        seed.F_Year = F_Year;
            //        seed.F_Type = "su";
            //        seed.F_No = 0;
            //        db.Insert(seed);
            //    }
            //    no = no + getStrNum(Convert.ToString(seed.F_No + 1),6);
            //    seed.F_No = seed.F_No + 1;
            //    db.Update(seed);
            //    db.Commit();
            //}
            //return no;
            "e" + NumberBuilder.Build_18bit();

        public string GetShoMsgNum() =>
            //string no = "EX-" + F_Divis_ID + F_Year;
            //using (var db = new RepositoryBase().BeginTrans())
            //{
            //    School_No_Seed_Entity seed = db.FindEntity<School_No_Seed_Entity>(t => t.F_Divis == F_Divis_ID && t.F_Year == F_Year && t.F_Type == "student");
            //    if (seed == null)
            //    {
            //        seed = new School_No_Seed_Entity();
            //        seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
            //        seed.F_Divis = F_Divis_ID;
            //        seed.F_Year = F_Year;
            //        seed.F_Type = "su";
            //        seed.F_No = 0;
            //        db.Insert(seed);
            //    }
            //    no = no + getStrNum(Convert.ToString(seed.F_No + 1),6);
            //    seed.F_No = seed.F_No + 1;
            //    db.Update(seed);
            //    db.Commit();
            //}
            //return no;
            "m" + NumberBuilder.Build_18bit();

        public string GetBillNum() => "b" + NumberBuilder.Build_18bit();

        public string GetVoucherNum() => "v" + NumberBuilder.Build_18bit();

        public string getChangeBillNum() => "bc" + NumberBuilder.Build_18bit();

        public string GetFytzNum() => "fyc" + NumberBuilder.Build_18bit();

        public string GetXjydNum() => "xjc" + NumberBuilder.Build_18bit();

        public string GetTeachNum(string F_Type_Teacher, int F_EntryTime)
        {
            var db = new Data.UnitWork();
            var seed = db.FindEntity<NoSeed>(t =>
                t.F_Divis == F_Type_Teacher && t.F_Year == F_EntryTime && t.F_Type == "teacher");

            var no = F_Type_Teacher + F_EntryTime;
            if (seed == null)
            {
                seed = new NoSeed();
                seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                seed.F_Divis = F_Type_Teacher;
                seed.F_Year = Convert.ToInt32(F_EntryTime);
                seed.F_Type = "teacher";
                seed.F_No = 1;
                db.Insert(seed);
            }
            else
            {
                seed.F_No = seed.F_No + 1;
                db.Update(seed);
            }

            no = no + getStrNum(Convert.ToString(seed.F_No));
            //db.Commit();
            return no;
        }
    }
}