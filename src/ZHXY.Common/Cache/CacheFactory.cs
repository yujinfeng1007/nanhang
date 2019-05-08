namespace ZHXY.Common
{
    public class CacheFactory
    {
        private static ICache cache;

        /// <summary>
        ///     本地缓存
        /// </summary>
        /// <returns>  </returns>
        public static ICache Cache()
        {
            if (cache != null)  return cache;
            cache = new Cache();
            return cache;
        }
    }
}