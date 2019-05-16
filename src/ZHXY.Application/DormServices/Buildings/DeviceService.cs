using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 大屏设备
    /// </summary>
    public class DeviceService : AppService
    {
        public DeviceService(IZhxyRepository r) : base(r) { }

        /// <summary>
        /// 根据大屏设备号获取绑定的楼栋列表
        /// </summary>
        /// <param name="relevance"></param>
        /// <returns></returns>
        public List<Building> GetBindGate(Relevance relevance)
        {
            if (relevance == null)
            {
                return Read<Building>().ToList();
            }

            var relevances = Read<Relevance>(p => p.FirstKey == relevance.FirstKey && p.Name == "Device_Building").FirstOrDefault()?.SecondKey.Split(',').ToList();

            var list = Read<Building>(p => relevances.Contains(p.Id)).ToList();

            return list;
        }

        /// <summary>
        /// 大屏绑定楼栋信息
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <param name="buildingId"></param>
        public void BindBuildings(string deviceNumber, string buildingIds)
        {
            var relevance = new Relevance();
            relevance.Name = "Device_Building";
            relevance.FirstKey = deviceNumber;
            relevance.SecondKey = buildingIds;

            AddAndSave(relevance);
        }

        /// <summary>
        /// 大屏解绑楼栋信息
        /// </summary>
        /// <param name="deviceNumber"></param>
        public void UnBindBuildings(string deviceNumber)
        {
            var relevance = Read<Relevance>(p => p.Name == "Device_Building" && p.FirstKey == deviceNumber).FirstOrDefault();

            if (relevance != null)
            {
                DelAndSave(relevance);
            }
        }

        /// <summary>
        /// 获取楼栋学生基本外出在寝信息
        /// </summary>
        /// <param name="buildingIds"></param>
        /// <returns></returns>
        public dynamic GetDormOutInInfo(string buildingIds)
        {

            // 根据楼栋ID，查找所有宿舍信息
            var ids = buildingIds.Split(',').ToList();

            var list = new List<Object>();

            ids.ForEach(item =>
            {
                var dorms = Read<DormRoom>(t => t.UnitNumber ==item).Select(t => t.Id).ToList();

                // 根据宿舍查找学生宿舍对应表
                var students = Read<DormStudent>(t => dorms.Contains(t.DormId)).Select(t => t.StudentId).ToList();

                // 在寝人数
                var ins = Read<Student>(t => students.Contains(t.Id) && t.InOut == "0").Count();

                var outs = Read<Student>(t => students.Contains(t.Id) && t.InOut == "1").Count();

                list.Add(new
                {
                    F_BuildId = item,
                    F_OutNum = outs,
                    F_InNum = ins
                });
            });

            return list;
        }
    }
}
