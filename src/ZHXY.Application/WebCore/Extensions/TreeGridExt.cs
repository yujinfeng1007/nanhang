using System.Collections.Generic;
using System.Text;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    public static class TreeGridExt
    {
        public static string TreeGridJson(this List<GridTree> data)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            sb.Append(TreeGridJson(data, -1, "0"));
            sb.Append("]");
            return sb.ToString();
        }

        private static string TreeGridJson(List<GridTree> data, int index, string parentId)
        {
            var sb = new StringBuilder();
            var ChildNodeList = data.FindAll(t => t.ParentId == parentId);
            if (ChildNodeList.Count > 0) index++;
            foreach (var entity in ChildNodeList)
            {
                var strJson = entity.EntityJson;
                strJson = strJson.Insert(1, "\"loaded\":" + (entity.Loaded ? false : true).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"expanded\":" + entity.Expanded.ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"isLeaf\":" + (entity.IsLeaf ? false : true).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"parent\":\"" + parentId + "\",");
                strJson = strJson.Insert(1, "\"level\":" + index + ",");
                sb.Append(strJson);
                sb.Append(TreeGridJson(data, index, entity.Id));
            }
            return sb.ToString().Replace("}{", "},{");
        }

        public static string LazyTreeGridJson(this List<GridTree> data, int index, string parentId)
        {
            var sb = new StringBuilder();
            sb.Append("{ \"rows\": [");
            if (data.Count > 0) index++;
            foreach (var entity in data)
            {
                var strJson = entity.EntityJson;
                strJson = strJson.Insert(1, "\"loaded\":" + "false" + ",");
                strJson = strJson.Insert(1, "\"expanded\":" + entity.Expanded.ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"isLeaf\":" + "false" + ",");
                strJson = strJson.Insert(1, "\"parent\":\"" + parentId + "\",");
                strJson = strJson.Insert(1, "\"level\":" + index + ",");
                sb.Append(strJson);
            }

            sb.Append("]}");
            return sb.ToString().Replace("}{", "},{");
        }
    }
}