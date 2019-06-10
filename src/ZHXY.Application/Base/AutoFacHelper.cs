using System.Reflection;
using Autofac;
using ZHXY.Data;
using ZHXY.Domain;
using IContainer = Autofac.IContainer;

namespace ZHXY.Application
{

    public class AutoFacHelper
    {
        private static IContainer _container;
        static AutoFacHelper()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepositoryBase<>)).InstancePerDependency();
            builder.RegisterAssemblyTypes(Assembly.Load("ZHXY.Domain"), Assembly.Load("ZHXY.Repository")).Where(t => t.Name.EndsWith("Repository") && !t.IsAbstract).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(p => p.BaseType.Equals(typeof(AppService)) || p.Name.EndsWith("AppService") || p.Name.EndsWith("ManageApp")).AsSelf().InstancePerDependency();
            _container = builder.Build();
        }
        public static IContainer GetContainer() => _container;
        public static T GetFromFac<T>() => _container.Resolve<T>();
    }
}