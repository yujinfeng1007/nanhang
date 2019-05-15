using System.Collections.Generic;

namespace ZHXY.Domain
{
    public interface IModuleButtonRepository : IRepositoryBase<Button>
    {
        void SubmitCloneButton(List<Button> entitys);
    }
}