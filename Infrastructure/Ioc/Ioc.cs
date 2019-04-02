using Microsoft.Practices.Unity;
using System;
using System.Reflection;
using Unity;

namespace Infrastructure
{
    public class Ioc
    {
        private static readonly UnityContainer _container;

        static Ioc()
        {
            _container = new UnityContainer();
        }

        public static void RegisterInheritedTypes(Assembly assembly, Type baseType)
        {
            _container.RegisterInheritedTypes(assembly, baseType);
        }

        public static void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _container.RegisterType<TInterface, TImplementation>();
        }

        public static T Get<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
