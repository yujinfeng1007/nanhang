using System.Collections.Generic;

namespace ZHXY.Domain
{
    public interface IItemDetailRepository : IRepositoryBase<SysDicItem>
    {
        List<SysDicItem> GetItemList(string enCode);

        List<SysDicItem> GetItemByEnCode(string enCode);
        
    }
}