using System.Collections.Generic;
using System.Reflection;
using ZHXY.Common;

namespace ZHXY.Api
{
    /// <summary>
    ///
    /// </summary>
    public class ClonePropValue
    {
        /// <summary>
        /// 获取表里某页的数据
        /// </summary>
        public static T clonePropValue<T>(object source, T desc, string display)
        {
            var props = typeof(T).GetProperties();
            var props2 = source.GetType().GetRuntimeProperties();
            var extList = new List<string>();
            extList.Add("F_Id");
            extList.Add("F_SortCode");
            extList.Add("F_DepartmentId");
            extList.Add("F_DeleteMark");
            extList.Add("F_EnabledMark");
            extList.Add("F_CreatorTime");
            extList.Add("F_CreatorUserId");
            extList.Add("F_LastModifyTime");
            extList.Add("F_LastModifyUserId");
            extList.Add("F_DeleteTime");
            extList.Add("F_DeleteUserId");
            if (!string.IsNullOrEmpty(display))
            {
                var dis = display.Split(',');
                for (var i = 0; i < dis.Length; i++)
                {
                    extList.Remove(dis[i].ToString());
                }
            }
            foreach (var p in props)
            {
                if (extList.Contains(p.Name))
                {
                    continue;
                }
                else
                {
                    foreach (var p2 in props2)
                    {
                        if (p.Name.Equals(p2.Name))
                        {
                            //空值不复制
                            if (p2.GetValue(source).IsEmpty())
                                continue;
                            //数据类型不对
                            if (!p.PropertyType.FullName.Equals(p2.PropertyType.FullName))
                                continue;
                            p.SetValue(desc, p2.GetValue(source));
                            break;
                        }
                    }
                }
            }
            return desc;
        }
    }
}