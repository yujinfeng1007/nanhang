using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

//生成pdf
namespace ZHXY.Common
{
    public static class ITextSharpHelper
    {
        /// <summary>
        ///     根据模板单条打印pdf文件
        /// </summary>
        /// <param name="templatefile">模板路径</param>
        /// <param name="newFilePath">文件路径</param>
        /// <param name="para">Dictionary数据集</param>
        public static void GetTemplateToPDF(string templatefile, string newFilePath, Dictionary<string, string> para)
        {
            var reader = new PdfReader(templatefile);
            var pdfStamper =
                new PdfStamper(reader, new FileStream(newFilePath, FileMode.OpenOrCreate, FileAccess.Write));
            var pdfFormFields = pdfStamper.AcroFields; //获取域的集合
            var baseFT = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            pdfFormFields.AddSubstitutionFont(baseFT); //设置域的字体;生成文件几十K
            foreach (var parameter in para)
                //pdfFormFields.SetFieldProperty(parameter.Key, "textfont", baseFT, null);//生成文件过大(4.5MB左右) 摒弃掉了
                //为需要赋值的域设置值;
                if (parameter.Key != "F_StuPhoto")
                    pdfFormFields.SetField(parameter.Key, parameter.Value);
                else
                    pdfFormFields.SetField(parameter.Key, parameter.Value);
            pdfStamper.FormFlattening = true; //设置true PDF文件不能编辑
            pdfStamper.Close();
        }

        /// <summary>
        ///     根据模板批量打印pdf文件
        /// </summary>
        /// <param name="templatefile">模板文件</param>
        /// <param name="newFilePath">文件路径</param>
        /// <param name="newFileName">文件名称</param>
        /// <param name="listpara">List数据集</param>
        public static void GetTemplateToPDFByList(string templatefile, string newFilePath, string newFileName,
            List<Dictionary<string, string>> listpara)
        {
            var filename = newFilePath + "\\" + newFileName;
            var document = new Document();
            var copy = new PdfCopy(document, new FileStream(filename, FileMode.Create));
            document.Open();
            foreach (var item in listpara)
            {
                var tmppdffile = newFilePath + "\\tmp_" + newFileName;
                GetTemplateToPDF(templatefile, tmppdffile, item);
                var reader = new PdfReader(tmppdffile);
                //int n = reader.NumberOfPages;
                //模板有多页时 循环N
                document.NewPage();
                var page = copy.GetImportedPage(reader, 1);
                copy.AddPage(page);
                reader.Close();
                if (File.Exists(newFilePath + "\\tmp_" + newFileName))
                    File.Delete(newFilePath + "\\tmp_" + newFileName);
            }

            document.Close();
        }
    }
}