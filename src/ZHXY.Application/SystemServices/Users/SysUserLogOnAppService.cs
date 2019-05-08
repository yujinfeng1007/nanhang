
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 用户登录管理
    /// </summary>
    public class SysUserLogOnAppService : AppService
    {
        private IRepositoryBase<UserLogin> Repository { get; }

        public SysUserLogOnAppService() => Repository = new Repository<UserLogin>();

        public SysUserLogOnAppService(IRepositoryBase<UserLogin> repos) => Repository = repos;

        public UserLogin GetForm(string keyValue) => Repository.Find(keyValue);

        public void UpdateForm(UserLogin userLogOnEntity) => Repository.Update(userLogOnEntity);

        public void RevisePassword(string userPassword, string keyValue)
        {
            var userLogOnEntity = new UserLogin
            {
                F_Id = keyValue,
                F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower()
            };
            userLogOnEntity.F_UserPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(userPassword, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();

            var parameters = "sysid=" + userLogOnEntity.F_Id + "&amp;appid=" + Configs.GetValue("appid") + "&amp;password=" + userLogOnEntity.F_UserPassword + "";
            WebHelper.SendRequest(Configs.GetValue("CallUrlUpt"), parameters, true, "application/json");
            Repository.Update(userLogOnEntity);
        }
    }
}