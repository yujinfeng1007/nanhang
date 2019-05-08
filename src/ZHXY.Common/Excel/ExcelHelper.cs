using NPOI.SS.UserModel;

namespace ZHXY.Common
{
    public static class ExcelHelper
    {
        public static string GetCellValue(this ICell cell)
        {
            if (cell == null)
                return null;
            cell.SetCellType(CellType.String);
            return cell.StringCellValue;
        }
    }
}