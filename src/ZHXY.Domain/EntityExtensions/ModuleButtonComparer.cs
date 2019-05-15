using System.Collections.Generic;

namespace ZHXY.Domain
{
    public class ModuleButtonComparer : IEqualityComparer<Button>
    {
        public bool Equals(Button x, Button y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null)
                return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(Button module)
        {
            var hashModuleName = module.Name == null ? 0 : module.Name.GetHashCode();
            var hashModuleCode = module.Id.GetHashCode();
            return hashModuleName ^ hashModuleCode;
        }
    }
}