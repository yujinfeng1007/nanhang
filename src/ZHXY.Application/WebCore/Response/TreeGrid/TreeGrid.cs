using System.Collections.Generic;
using System.Text;

namespace ZHXY.Application
{
    public static class TreeGrid
    {
        public static string TreeGridJson(this List<TreeGridModel> data)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            sb.Append(TreeGridJson(data, -1, "0"));
            sb.Append("]");
            return sb.ToString();
        }

        private static string TreeGridJson(List<TreeGridModel> data, int index, string parentId)
        {
            var sb = new StringBuilder();
            var ChildNodeList = data.FindAll(t => t.parentId == parentId);
            if (ChildNodeList.Count > 0) index++;
            foreach (var entity in ChildNodeList)
            {
                var strJson = entity.entityJson;
                strJson = strJson.Insert(1, "\"loaded\":" + (entity.loaded ? false : true).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"expanded\":" + entity.expanded.ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"isLeaf\":" + (entity.isLeaf ? false : true).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"parent\":\"" + parentId + "\",");
                strJson = strJson.Insert(1, "\"level\":" + index + ",");
                sb.Append(strJson);
                sb.Append(TreeGridJson(data, index, entity.id));
            }
            return sb.ToString().Replace("}{", "},{");
        }

        public static string LazyTreeGridJson(this List<TreeGridModel> data, int index, string parentId)
        {
            var sb = new StringBuilder();
            sb.Append("{ \"rows\": [");
            if (data.Count > 0) index++;
            foreach (var entity in data)
            {
                var strJson = entity.entityJson;
                strJson = strJson.Insert(1, "\"loaded\":" + "false" + ",");
                strJson = strJson.Insert(1, "\"expanded\":" + entity.expanded.ToString().ToLower() + ",");
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