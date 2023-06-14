using Autofac;
using Microsoft.Extensions.Hosting;
using OpenDeepSpace.Autofacastle.AspectAttention;
using OpenDeepSpace.Autofacastle.DependencyInjection;
using OpenDeepSpace.Autofacastle.DependencyInjection.Middlewares;
using OpenDeepSpace.Autofacastle.Reflection;
using OpenDeepSpace.NetCore.Autofacastle.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenDeepSpace.NetCore.Autofacastle.Extensions
{
    /// <summary>
    /// Autofacastle拓展
    /// </summary>
    public static class AutofacastleExtensions
    {


        /// <summary>
        /// 
        /// 使用该方法之后 
        /// 批量注入 特性依赖自动注入 切面拦截[需要拦截的需要加入到DI容器中]全部配置完成
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="automaticInjectionSelectors"></param>
        /// <param name="nonInterceptSelectors"></param>
        /// <param name="classInterceptSelectors"></param>
        /// <returns></returns>
        public static IHostBuilder UseAutofacastle(this IHostBuilder hostBuilder,List<AutomaticInjectionSelector> automaticInjectionSelectors = null, List<NonInterceptSelector> nonInterceptSelectors = null, List<ClassInterceptSelector> classInterceptSelectors = null)
        {

            OpenDeepSpace.Autofacastle.Reflection.AssemblyFinder.AddExternalAssemblies(NetCore.Autofacastle.Reflection.AssemblyFinder.GetExcludeSystemNugetAllAssemblies());

            hostBuilder.UseServiceProviderFactory(new AutofacastleServiceProviderFactory(builder => { RegisterCallBack(builder); }, automaticInjectionSelectors, nonInterceptSelectors, classInterceptSelectors));

            return hostBuilder;
        }

        /// <summary>
        /// 注册自动注入中间件回调
        /// </summary>
        /// <param name="containerBuilder"></param>
        private static void RegisterCallBack(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterCallback(c =>
            {
                c.Registered += (sender, args) =>
                {

                    args.ComponentRegistration.PipelineBuilding += (s, pipeline) =>
                    {

                        pipeline.Use(new AutomaticInjectionMiddleware());
                    };
                };
            });
        }
    }
}
