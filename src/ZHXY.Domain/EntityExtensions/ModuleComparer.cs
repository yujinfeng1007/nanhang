using System.Collections.Generic;

namespace ZHXY.Domain
{
    public class ModuleComparer : IEqualityComparer<SysModule>
    {
        public bool Equals(SysModule x, SysModule y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x is null || y is null)
                return false;
            return x.F_Id == y.F_Id;
        }

        public int GetHashCode(SysModule module)
        {
            var hashModuleName = module.F_FullName == null ? 0 : module.F_FullName.GetHashCode();

            var hashModuleCode = module.F_Id.GetHashCode();

            return hashModuleName ^ hashModuleCode;
        }
    }
}