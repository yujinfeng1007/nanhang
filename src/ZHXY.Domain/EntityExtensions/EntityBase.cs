using System;
using ZHXY.Common;

namespace ZHXY.Domain
{
    [Serializable]
    public abstract class EntityBase : IEntity
    {
        public void Create(bool flag = true)
        {
            if (!(this is ICreationAudited entity)) return;
            entity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
            if (flag)
            {
                var LoginInfo = Operator.GetCurrent();
                if (LoginInfo != null)
                {
                    entity.F_CreatorUserId = LoginInfo.Id;
                    if (entity.F_DepartmentId.IsEmpty())
                        entity.F_DepartmentId = LoginInfo.Organ;
                }
            }

            entity.F_CreatorTime = DateTime.Now;
            entity.F_DeleteMark = false;
        }

        public void Modify(string keyValue, bool flag = true)
        {
            var entity = this as IModificationAudited;
            entity.F_Id = keyValue;
            if (flag)
            {
                var LoginInfo = Operator.GetCurrent();
                if (LoginInfo != null)
                {
                    entity.F_LastModifyUserId = LoginInfo.Id;
                }
            }
            entity.F_LastModifyTime = DateTime.Now;
        }

        public void Remove(bool flag = true)
        {
            var entity = this as IDeleteAudited;
            if (flag)
            {
                var LoginInfo = Operator.GetCurrent();
                if (LoginInfo != null)
                {
                    entity.F_DeleteUserId = LoginInfo.Id;
                }
            }
            entity.F_DeleteTime = DateTime.Now;
            entity.F_DeleteMark = true;
        }
    }
}