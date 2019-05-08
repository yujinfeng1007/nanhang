using System.Collections.Generic;
using System.Reflection;

namespace ZHXY.Common
{
    public static class ExtObj
    {
        /// <summary>
        ///     获取表里某页的数据
        /// </summary>
        /// <returns> 返回当页表数据 </returns>
        public static T ClonePropValue<T>(object source, T desc)
        {
            var props = typeof(T).GetProperties();
            var props2 = source.GetType().GetRuntimeProperties();
            var extList = new List<string>
            {
                "F_Id",
                "F_SortCode",
                "F_DepartmentId",
                "F_DeleteMark",
                "F_EnabledMark",
                "F_CreatorTime",
                "F_CreatorUserId",
                "F_LastModifyTime",
                "F_LastModifyUserId",
                "F_DeleteTime",
                "F_DeleteUserId"
            };

            foreach (var p in props)
            {
                if (extList.Contains(p.Name)) continue;
                else
                    foreach (var p2 in props2)
                        if (p.Name.Equals(p2.Name))
                        {
                            //空值不复制
                            if (p2.GetValue(source).IsEmpty())
                                continue;
                            //数据类型不对
                            if (p.PropertyType.FullName != null && !p.PropertyType.FullName.Equals(p2.PropertyType.FullName))
                                continue;
                            p.SetValue(desc, p2.GetValue(source));
                            break;
                        }
            }
            return desc;
        }

        public static T ClonePropValueGrade<T>(object source, T desc)
        {
            var props = typeof(T).GetProperties();
            var props2 = source.GetType().GetRuntimeProperties();
            var extList = new List<string>();
            foreach (var p in props)
                if (extList.Contains(p.Name))
                    continue;
                else
                    foreach (var p2 in props2)
                        if (p.Name.Equals(p2.Name))
                        {
                            //空值不复制
                            if (p2.GetValue(source).IsEmpty())
                                continue;
                            //数据类型不对
                            if (p.PropertyType.FullName != null && !p.PropertyType.FullName.Equals(p2.PropertyType.FullName))
                                continue;
                            p.SetValue(desc, p2.GetValue(source));
                            break;
                        }

            return desc;
        }
    }
}