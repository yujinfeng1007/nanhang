using System.Collections.Generic;

namespace ZHXY.Domain
{
    public class ModuleButtonComparer : IEqualityComparer<SysButton>
    {
        public bool Equals(SysButton x, SysButton y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null)
                return false;
            return x.F_Id == y.F_Id;
        }

        public int GetHashCode(SysButton module)
        {
            var hashModuleName = module.F_FullName == null ? 0 : module.F_FullName.GetHashCode();
            var hashModuleCode = module.F_Id.GetHashCode();
            return hashModuleName ^ hashModuleCode;
        }
    }
}