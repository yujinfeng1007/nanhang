using System.Collections.Generic;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ShortMsgStayRepository : Data.Repository<ShortMsgStay>, IShortMsgStayRepository
    {
        private readonly IOrganizeRepository orgservice = new OrganizeRepository();
        private readonly IRepositoryBase<ShortMsgModelEntity> shortservice = new Data.Repository<ShortMsgModelEntity>();



        public void InsertAndDelete(List<ShortMsg> shortmsg, List<ShortMsgPoolEntity> shortmsgpool,
            List<ShortMsgStay> shortmsgstay)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                db.BatchInsert(shortmsg);
                db.BatchInsert(shortmsgpool);
                db.Delete(shortmsgstay);
                db.Commit();
            }
        }

        public string GetMsg(SysMsgModel entity)
        {
            var model = shortservice.Find(entity.MsgModelId);
            var F_Module = model.F_Module;
            if (!string.IsNullOrEmpty(entity.F_AccNum)) F_Module = F_Module.Replace("F_AccNum", entity.F_AccNum);

            if (entity.F_Deposit != null) F_Module = F_Module.Replace("F_Deposit", entity.F_Deposit.ToString());

            if (!string.IsNullOrEmpty(entity.F_ExamNum)) F_Module = F_Module.Replace("F_ExamNum", entity.F_ExamNum);

            if (!string.IsNullOrEmpty(entity.F_StuName)) F_Module = F_Module.Replace("F_StuName", entity.F_StuName);

            if (entity.F_Total != null) F_Module = F_Module.Replace("F_Total", entity.F_Total.ToString());

            if (!string.IsNullOrEmpty(entity.F_Divis_ID))
                F_Module = F_Module.Replace("F_Divis_ID", orgservice.Find(entity.F_Divis_ID).F_FullName);

            if (!string.IsNullOrEmpty(entity.F_WebsitePhone))
                F_Module = F_Module.Replace("F_WebsitePhone", entity.F_WebsitePhone);

            if (!string.IsNullOrEmpty(entity.F_Website)) F_Module = F_Module.Replace("F_Website", entity.F_Website);

            if (!string.IsNullOrEmpty(entity.F_Exam_Address))
                F_Module = F_Module.Replace("F_Exam_Address", entity.F_Exam_Address);

            if (entity.F_ExamDTM != null) F_Module = F_Module.Replace("F_ExamDTM", entity.F_ExamDTM.ToString());

            if (!string.IsNullOrEmpty(entity.F_ExamTitle))
                F_Module = F_Module.Replace("F_ExamTitle", entity.F_ExamTitle);

            if (!string.IsNullOrEmpty(entity.F_StudentNum))
                F_Module = F_Module.Replace("F_StudentNum", entity.F_StudentNum);

            if (!string.IsNullOrEmpty(entity.F_InitNum)) F_Module = F_Module.Replace("F_InitNum", entity.F_InitNum);

            if (entity.F_SundryFees != null)
                F_Module = F_Module.Replace("F_SundryFees", entity.F_SundryFees.ToString());
            return F_Module;
        }
    }
}