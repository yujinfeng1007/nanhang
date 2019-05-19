
//using System;

//namespace ZHXY.Domain
//{
//    [Serializable]
//    public abstract class Entity : EntityBase, ICreationAudited, IDeleteAudited, IModificationAudited
//    {
//        /// <summary>
//        /// ID
//        /// </summary>

//        public string F_Id { get; set; }

//        /// <summary>
//        /// 序号
//        /// </summary>

//        public int? F_SortCode { get; set; }

//        /// <summary>
//        /// 所属部门
//        /// </summary>

//        public string F_DepartmentId { get; set; }

//        /// <summary>
//        /// 删除标记
//        /// </summary>

//        public bool? F_DeleteMark { get; set; }

//        /// <summary>
//        /// 启用标记
//        /// </summary>

//        public bool? F_EnabledMark { get; set; }

//        /// <summary>
//        /// 创建时间
//        /// </summary>

//        public DateTime? F_CreatorTime { get; set; }

//        /// <summary>
//        /// 创建者
//        /// </summary>

//        public string F_CreatorUserId { get; set; }

//        /// <summary>
//        /// 修改时间
//        /// </summary>

//        public DateTime? F_LastModifyTime { get; set; }

//        /// <summary>
//        /// 修改者
//        /// </summary>

//        public string F_LastModifyUserId { get; set; }

//        /// <summary>
//        /// 删除时间
//        /// </summary>

//        public DateTime? F_DeleteTime { get; set; }

//        /// <summary>
//        /// 删除者
//        /// </summary>

//        public string F_DeleteUserId { get; set; }

//        /// <summary>
//        /// 备注
//        /// </summary>

//        public string F_Memo { get; set; }
//    }
//}