using System.Collections.Generic;

namespace ZHXY.Domain
{
    public interface IModuleButtonRepository : IRepositoryBase<SysButton>
    {
        void SubmitCloneButton(List<SysButton> entitys);
    }
}