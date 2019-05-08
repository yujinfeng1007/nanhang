using System;
namespace ZHXY.Domain
{
    public interface ICreationAudited
    {
        string F_Id { get; set; }
        string F_CreatorUserId { get; set; }
        DateTime? F_CreatorTime { get; set; }

        string F_DepartmentId { get; set; }
        bool? F_DeleteMark { get; set; }
    }
}