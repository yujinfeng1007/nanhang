using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using System.Reflection;
using Autofac.Integration.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;
using System.Data.Entity;

namespace ZHXY.Web
{
    public class MvcApplication : HttpApplication
    { 
        protected void Application_Start()
        {
            SetDependencyResolver();
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new ViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // todo
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
            if (null != lastError)
            {
                Response.StatusCode = 200;
                if ((lastError.GetBaseException() is NoLoggedInException))
                {
                    Response.Write("<script>top.location.pathname = '/Login/Index';</script>");
                }
                else
                {
                    Response.Write(new {state="error",message=lastError.GetBaseException().Message }.ToCamelJson());
                }
                Server.ClearError();
                Response.End();
            }
           
        }


        private static void SetDependencyResolver()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ZhxyDbContext>().As<DbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepositoryBase<>)).InstancePerRequest();

            // 注册仓储层
            builder.RegisterAssemblyTypes(Assembly.Load("ZHXY.Domain")).Where(t => t.Name.EndsWith("Repository") && !t.IsAbstract).AsImplementedInterfaces().InstancePerRequest();

            // 注册单元工作
            builder.RegisterType(typeof(UnitWork)).As(typeof(IUnitWork)).InstancePerRequest();

            // 注册app层
            builder.RegisterAssemblyTypes(Assembly.Load("ZHXY.Application")).Where(p => p.BaseType.Equals(typeof(AppService))&&!p.IsAbstract).AsSelf().InstancePerRequest();

            // 注册控制器
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // 启用视图模型自动注入
            builder.RegisterSource(new ViewRegistrationSource());

            // 注册所有的Attribute
            builder.RegisterFilterProvider();

            // 设置容器
            var container = builder.Build();

            //AutoFacHelper.SetContainer(container);

            // 设置 Mvc 依赖解析 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}