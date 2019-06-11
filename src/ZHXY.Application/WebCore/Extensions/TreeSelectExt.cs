using System.Collections.Generic;
using System.Text;
using ZHXY.Common;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    public static class TreeSelectExt
    {
        public static string TreeSelectJson(this List<SelectTree> data)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            sb.Append(TreeSelectJson(data, "0", ""));
            sb.Append("]");
            return sb.ToString();
        }

        private static string TreeSelectJson(List<SelectTree> data, string parentId, string blank)
        {
            var sb = new StringBuilder();
            var ChildNodeList = data.FindAll(t => t.ParentId == parentId);
            var tabline = "";
            if (parentId != "0") tabline = "　　";
            if (ChildNodeList.Count > 0) tabline = tabline + blank;
            foreach (var entity in ChildNodeList)
            {
                entity.Text = tabline + entity.Text;
                var strJson = entity.ToJson();
                sb.Append(strJson);
                sb.Append(TreeSelectJson(data, entity.Id, tabline));
            }

            return sb.ToString().Replace("}{", "},{");
        }
    }
}