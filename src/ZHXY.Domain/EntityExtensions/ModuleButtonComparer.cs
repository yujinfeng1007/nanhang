using System.Collections.Generic;

namespace ZHXY.Domain
{
    public class ModuleButtonComparer : IEqualityComparer<Function>
    {
        public bool Equals(Function x, Function y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null)
                return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(Function module)
        {
            var hashModuleName = module.Name == null ? 0 : module.Name.GetHashCode();
            var hashModuleCode = module.Id.GetHashCode();
            return hashModuleName ^ hashModuleCode;
        }
    }
}