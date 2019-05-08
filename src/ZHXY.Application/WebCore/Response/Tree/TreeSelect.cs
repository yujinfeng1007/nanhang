using System.Collections.Generic;
using System.Text;
using ZHXY.Common;

namespace ZHXY.Application
{
    public static class TreeSelect
    {
        public static string TreeSelectJson(this List<TreeSelectModel> data)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            sb.Append(TreeSelectJson(data, "0", ""));
            sb.Append("]");
            return sb.ToString();
        }

        private static string TreeSelectJson(List<TreeSelectModel> data, string parentId, string blank)
        {
            var sb = new StringBuilder();
            var ChildNodeList = data.FindAll(t => t.parentId == parentId);
            var tabline = "";
            if (parentId != "0") tabline = "　　";
            if (ChildNodeList.Count > 0) tabline = tabline + blank;
            foreach (var entity in ChildNodeList)
            {
                entity.text = tabline + entity.text;
                var strJson = entity.ToJson();
                sb.Append(strJson);
                sb.Append(TreeSelectJson(data, entity.id, tabline));
            }

            return sb.ToString().Replace("}{", "},{");
        }
    }
}