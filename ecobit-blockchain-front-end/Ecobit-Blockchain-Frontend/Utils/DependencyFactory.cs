using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Unity;

namespace Ecobit_Blockchain_Frontend.Utils
{
    public static class DependencyFactory
    {
        private static readonly IUnityContainer Container = InitUnityContainer();

        /// <summary>
        /// Resolves the type parameter T to an instance of the appropriate type.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        public static T Resolve<T>()
        {
            T ret = default(T);
 
            if (Container.IsRegistered(typeof(T)))
            {
                ret = Container.Resolve<T>();
            }
 
            return ret;
        }

        private static IUnityContainer InitUnityContainer()
        {
            var container = new UnityContainer();
            
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            if (section != null)
            {
                section.Configure(container);
            }
            return container;
        }
    }
}