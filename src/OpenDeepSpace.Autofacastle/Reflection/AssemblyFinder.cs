using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Autofacastle.Reflection
{
    /// <summary>
    /// 程序集查找者
    /// </summary>
    public static class AssemblyFinder
    {
        //所有程序集
        private static readonly string AllAssemblies = $"{nameof(AllAssemblies)}";
        //排除系统和Nuget的程序集
        private static readonly string ExcludeSystemNugetAllAssemblies = $"{nameof(ExcludeSystemNugetAllAssemblies)}";
        //系统和Nuget的程序集
        private static readonly string SystemNugetAllAssmeblies = $"{nameof(SystemNugetAllAssmeblies)}";

        //缓存
        private static readonly ConcurrentDictionary<string, List<Assembly>> assembliesCache = new ConcurrentDictionary<string, List<Assembly>>();

        //排除已知以这些开头的程序集
        private static readonly string[] ExcludeAssemblyStartWiths = new string[] { "System",
                "Microsoft"};
        

        /// <summary>
        /// 获取项目所有程序集
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetAllAssemblies()
        {

            if (assembliesCache.ContainsKey(AllAssemblies))
                return assembliesCache[AllAssemblies];            
            assembliesCache[AllAssemblies] = CollectionAssembly();

            return assembliesCache[AllAssemblies];
        }


        /// <summary>
        /// 收集程序集
        /// </summary>
        private static List<Assembly> CollectionAssembly()
        {
            //单项目 非分层
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            //分层项目
            var layerAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();

            //union
            var list = domainAssemblies.Union(layerAssemblies).Where(t => ExcludeAssemblyStartWiths.All(t1 => !t.FullName.StartsWith(t1))).ToList();

            return list;
        }

        /// <summary>
        /// 添加外部的程序集
        /// </summary>
        /// <param name="assemblies"></param>
        public static void AddExternalAssemblies(List<Assembly> assemblies)
        {
            if (assemblies == null)
                return;

            if (!assembliesCache.ContainsKey(AllAssemblies) || assembliesCache[AllAssemblies] == null)
                GetAllAssemblies();
            
            assembliesCache[AllAssemblies].Union(assemblies);
            
        }

    }
}
