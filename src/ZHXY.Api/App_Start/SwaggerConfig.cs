using System.Web.Http;
using WebActivatorEx;
using ZHXY.Api;
using Swashbuckle.Application;
using System.Linq;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace ZHXY.Api
{
    /// <summary>
    /// Swagger配置
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "ZHXY.Api");
                        c.IncludeXmlComments(GetXmlCommentsPath());
                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("智慧校园开发接口");
                    });
        }

        private static string GetXmlCommentsPath()
        {
            return string.Format("{0}/bin/ZHXY.Api.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}