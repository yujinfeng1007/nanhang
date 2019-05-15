using System.Collections.Generic;

namespace ZHXY.Domain
{
    public class ModuleComparer : IEqualityComparer<Menu>
    {
        public bool Equals(Menu x, Menu y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x is null || y is null)
                return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(Menu module)
        {
            var hashModuleName = module.Name == null ? 0 : module.Name.GetHashCode();

            var hashModuleCode = module.Id.GetHashCode();

            return hashModuleName ^ hashModuleCode;
        }
    }
}