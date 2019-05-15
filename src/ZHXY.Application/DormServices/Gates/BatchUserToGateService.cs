using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Dorm.Device.DH;
using ZHXY.Dorm.Device.tools;

namespace ZHXY.Application.DormServices.Gates
{
    public class BatchUserToGateService : AppService
    {
        public BatchUserToGateService(IZhxyRepository r) : base(r) { }

        // 下载用户头像，并返回用户
        private DataTable downImage(string[] userId, string imgPath = null)
        {
            var stuList = Read<User>(t => userId.Contains(t.Id)).ToList();

            var dt = new DataTable();
            var dc1 = new DataColumn("姓名", Type.GetType("System.String"));
            var dc2 = new DataColumn("性别", Type.GetType("System.String"));
            var dc3 = new DataColumn("学工号", Type.GetType("System.String"));
            var dc4 = new DataColumn("所属组织", Type.GetType("System.String"));
            var dc5 = new DataColumn("身份证号", Type.GetType("System.String"));
            var dc6 = new DataColumn("类别", Type.GetType("System.String"));
            var dc7 = new DataColumn("联系电话", Type.GetType("System.String"));
            var dc8 = new DataColumn("紧急联系人", Type.GetType("System.String"));
            var dc9 = new DataColumn("紧急联系人电话", Type.GetType("System.String"));
            var dc10 = new DataColumn("门禁卡号", Type.GetType("System.String"));
            var dc11 = new DataColumn("院区", Type.GetType("System.String"));
            var dc12 = new DataColumn("宿舍", Type.GetType("System.String"));
            var dc13 = new DataColumn("楼层", Type.GetType("System.String"));
            var dc14 = new DataColumn("寝室", Type.GetType("System.String"));
            var dc15 = new DataColumn("校区", Type.GetType("System.String"));
            var dc16 = new DataColumn("院系", Type.GetType("System.String"));
            var dc17 = new DataColumn("专业", Type.GetType("System.String"));
            var dc18 = new DataColumn("年级", Type.GetType("System.String"));
            var dc19 = new DataColumn("班级", Type.GetType("System.String"));

            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            dt.Columns.Add(dc6);
            dt.Columns.Add(dc7);
            dt.Columns.Add(dc8);
            dt.Columns.Add(dc9);
            dt.Columns.Add(dc10);
            dt.Columns.Add(dc11);
            dt.Columns.Add(dc12);
            dt.Columns.Add(dc13);
            dt.Columns.Add(dc14);
            dt.Columns.Add(dc15);
            dt.Columns.Add(dc16);
            dt.Columns.Add(dc17);
            dt.Columns.Add(dc18);
            dt.Columns.Add(dc19);
            foreach (var d in stuList)
            {
                string studentNo = d.Name;
                string imgUri = d.HeadIcon;
                string gender = "女";
                string certificateNo = "";
                string userType = "访客";
                string ss = "";
                string lc = "";
                string ld = "";
                if (d.DutyId.Contains("student"))
                {
                    var stu = Read<Student>(p => p.UserId == d.Id).FirstOrDefault();
                    studentNo = stu?.StudentNumber;
                    //imgUri = stu?.F_FacePic_File;
                    gender = stu?.Gender == "0" ? "女" : "男";
                    certificateNo = stu?.CardNumber;
                    userType = "学生";
                    var ssdata = Query<DormStudent>(p => p.StudentId == stu.Id).FirstOrDefault();
                    ss = ssdata?.DormInfo?.Title;
                    lc = ssdata?.DormInfo?.FloorNumber;
                    var ldid = ssdata?.DormInfo?.BuildingId;
                    var lddata = Read<Building>(p => p.Id == ldid).FirstOrDefault();
                    ld = lddata?.BuildingNo;
                }
                if (d.DutyId.Contains("teacher"))
                {
                    var tea = Read<Teacher>(p => p.UserId == d.Id).FirstOrDefault();
                    studentNo = tea?.JobNumber;
                    imgUri = tea?.FacePhoto;
                    gender = tea?.Gender == "0" ? "女" : "男";
                    certificateNo = tea?.CredNum;
                    userType = "教职工";
                }
                string filepath = imgPath;
                if (string.IsNullOrEmpty(imgPath))
                    filepath = System.AppDomain.CurrentDomain.BaseDirectory + "\\HeadIcoTemp\\";

                if (Directory.Exists(filepath))
                {
                    Directory.Delete(filepath, true);
                }
                Directory.CreateDirectory(filepath);


                string fileName = filepath + studentNo + ".png";
                bool t = GetImageBase64Str.DownLoadPic(imgUri, fileName);
                if (t)
                {
                    var dr = dt.NewRow();
                    dr["姓名"] = d.Name;
                    dr["性别"] = gender;
                    dr["学工号"] = studentNo;
                    dr["所属组织"] = "南昌航空大学";
                    dr["身份证号"] = certificateNo;
                    dr["类别"] = userType;
                    dr["联系电话"] = "";
                    dr["紧急联系人"] = "";
                    dr["紧急联系人电话"] = "";
                    dr["门禁卡号"] = "";
                    dr["院区"] = "宿舍管理";
                    dr["宿舍"] = ld;
                    dr["楼层"] = lc;
                    dr["寝室"] = ss;
                    dr["校区"] = "";
                    dr["院系"] = "";
                    dr["专业"] = "";
                    dr["年级"] = "";
                    dr["班级"] = "";
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        
        // 发送用户头像到闸机
        public void SendUserHeadIco(string[] userId)
        {
            string filepath = System.AppDomain.CurrentDomain.BaseDirectory + "\\HeadIcoTemp\\";
            var dt = downImage(userId, filepath);
            string zipfile = zipFile(filepath);
            if (zipfile == null)
                throw new Exception("上传头像文件出错！");
            DHAccount.PUSH_DH_BATCHPHOTO_ZIP(zipfile);

            var result = new NPOIExcel().ToExcel(dt, "导入用户" + DateTime.Now.ToString("yyyyMMddHHmmss"), "用户", filepath + "user.xls");
            if (!result)
                throw new Exception("上传Excel出错！");
            DHAccount.PUSH_DH_PERSON_EXCEL(filepath + "user.xls");
        }

        #region 压缩  
        private string zipFile(string filePath)
        {
            string zipedFile = filePath + "\\imgs.zip";
            bool result = false;
            if (!Directory.Exists(filePath))
                return null;

            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
            zipStream.SetLevel(6);
            result = ZipDirectory(filePath, zipStream, "");

            zipStream.Finish();
            zipStream.Close();

            return result ? zipedFile : null;
        }

        /// <summary>   
        /// 递归压缩文件夹的内部方法   
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipStream">压缩输出流</param>   
        /// <param name="parentFolderName">此文件夹的上级文件夹</param>   
        /// <returns></returns>   
        private static bool ZipDirectory(string folderToZip, ZipOutputStream zipStream, string parentFolderName)
        {
            bool result = true;
            string[] folders, files;
            ZipEntry ent = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();


            ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/"));
            zipStream.PutNextEntry(ent);
            zipStream.Flush();

            files = Directory.GetFiles(folderToZip);
            foreach (string file in files)
            {
                try
                {

                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file)));
                    ent.DateTime = DateTime.Now;
                    ent.Size = fs.Length;

                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    ent.Crc = crc.Value;
                    zipStream.PutNextEntry(ent);
                    zipStream.Write(buffer, 0, buffer.Length);
                }
                catch
                {
                    //result = false;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                    if (ent != null)
                    {
                        ent = null;
                    }
                    GC.Collect();
                    GC.Collect(1);
                }
            }



            folders = Directory.GetDirectories(folderToZip);
            foreach (string folder in folders)
                if (!ZipDirectory(folder, zipStream, folderToZip))
                    return false;

            return result;
        }
        #endregion
    }
}
