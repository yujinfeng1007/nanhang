using System.Collections.Generic;
using System.Text;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    public static class TreeViewExt
    {
        public static string TreeViewJson(this List<ViewTree> data, string parentId = "0")
        {
            var strJson = new StringBuilder();
            var item = data.FindAll(t => t.ParentId == parentId);
            strJson.Append("[");
            if (item.Count > 0)
            {
                foreach (var entity in item)
                {
                    strJson.Append("{");
                    strJson.Append("\"id\":\"" + entity.Id + "\",");
                    strJson.Append("\"text\":\"" + entity.Text.Replace("&nbsp;", "") + "\",");
                    strJson.Append("\"value\":\"" + entity.Value + "\",");
                    if (entity.Title != null && !string.IsNullOrEmpty(entity.Title.Replace("&nbsp;", "")))
                        strJson.Append("\"title\":\"" + entity.Title.Replace("&nbsp;", "") + "\",");
                    if (entity.Img != null && !string.IsNullOrEmpty(entity.Img.Replace("&nbsp;", "")))
                        strJson.Append("\"img\":\"" + entity.Img.Replace("&nbsp;", "") + "\",");
                    if (entity.Checkstate != null) strJson.Append("\"checkstate\":" + entity.Checkstate + ",");
                    if (entity.ParentId != null) strJson.Append("\"parentnodes\":\"" + entity.ParentId + "\",");
                    strJson.Append("\"showcheck\":" + entity.Showcheck.ToString().ToLower() + ",");
                    strJson.Append("\"isexpand\":" + entity.Isexpand.ToString().ToLower() + ",");
                    if (entity.Complete) strJson.Append("\"complete\":" + entity.Complete.ToString().ToLower() + ",");
                    strJson.Append("\"hasChildren\":" + entity.HasChildren.ToString().ToLower() + ",");
                    strJson.Append("\"ChildNodes\":" + TreeViewJson(data, entity.Id) + "");
                    strJson.Append("},");
                }

                strJson = strJson.Remove(strJson.Length - 1, 1);
            }

            strJson.Append("]");
            return strJson.ToString();
        }
    }
}