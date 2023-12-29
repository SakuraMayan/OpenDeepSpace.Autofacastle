using Autofac;
using OpenDeepSpace.Autofacastle.AspectAttention;
using OpenDeepSpace.Autofacastle.DependencyInjection;
using OpenDeepSpace.Autofacastle.DependencyInjection.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Autofacastle.Extensions
{
    /// <summary>
    /// Autofacastle拓展
    /// </summary>
    public static class AutofacastleExtensions
    {
        /// <summary>
        /// 批量注入和特性依赖注入方法
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="assemblies">程序集</param>
        /// <param name="automaticInjectionSelectors">自动注入筛选器</param>
        /// <returns></returns>
        public static ContainerBuilder UseAutofacastle(this ContainerBuilder containerBuilder,IEnumerable<Assembly> assemblies,List<AutomaticInjectionSelector> automaticInjectionSelectors = null)
        {
            if(assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            return containerBuilder.UseAutofacastle(assemblies.SelectMany(t => t.GetTypes()), automaticInjectionSelectors);
        }

        /// <summary>
        /// 批量注入和特性依赖注入方法
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="types">类型集合</param>
        /// <param name="automaticInjectionSelectors">自动注入筛选器</param>
        /// <returns></returns>
        public static ContainerBuilder UseAutofacastle(this ContainerBuilder containerBuilder, IEnumerable<Type> types, List<AutomaticInjectionSelector> automaticInjectionSelectors = null)
        {
            if(types == null)
                throw new ArgumentNullException(nameof(types));

            //批量注入
            containerBuilder.BatchInjection(types);

            if (automaticInjectionSelectors != null && automaticInjectionSelectors.Any())
            {
                AutofacastleCollection.AutomaticInjectionSelectors.AddRange(automaticInjectionSelectors);
            }

            RegisterCallBack(containerBuilder);

            return containerBuilder;
        }

        /// <summary>
        /// 
        /// 完成批量注入 
        /// 特性依赖注入 切面拦截
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="assemblies">使用autofacastle的程序集</param>
        /// <param name="automaticInjectionSelectors"></param>
        /// <param name="nonInterceptSelectors"></param>
        /// <param name="classInterceptSelectors"></param>
        /// <returns></returns>
        public static ContainerBuilder UseAutofacastle(this ContainerBuilder containerBuilder,IEnumerable<Assembly> assemblies, List<AutomaticInjectionSelector> automaticInjectionSelectors = null, List<NonInterceptSelector> nonInterceptSelectors = null,List<ClassInterceptSelector> classInterceptSelectors=null)
        {
            if(assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            return containerBuilder.UseAutofacastle(assemblies.SelectMany(t=>t.GetTypes()),
                automaticInjectionSelectors, nonInterceptSelectors, classInterceptSelectors);
        }


        /// <summary>
        /// 
        /// 完成批量注入 
        /// 特性依赖注入 切面拦截
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="types">使用autofacastle的类型集合</param>
        /// <param name="automaticInjectionSelectors"></param>
        /// <param name="nonInterceptSelectors"></param>
        /// <param name="classInterceptSelectors"></param>
        /// <returns></returns>
        public static ContainerBuilder UseAutofacastle(this ContainerBuilder containerBuilder, IEnumerable<Type> types,List<AutomaticInjectionSelector> automaticInjectionSelectors = null, List<NonInterceptSelector> nonInterceptSelectors = null, List<ClassInterceptSelector> classInterceptSelectors = null)
        {

            if(types == null)
                throw new ArgumentNullException(nameof(types));

            if (automaticInjectionSelectors != null && automaticInjectionSelectors.Any())
                AutofacastleCollection.AutomaticInjectionSelectors.AddRange(automaticInjectionSelectors);
            if (nonInterceptSelectors != null && nonInterceptSelectors.Any())
                AutofacastleCollection.NonInterceptSelectors.AddRange(nonInterceptSelectors);
            if (classInterceptSelectors != null && classInterceptSelectors.Any())
                AutofacastleCollection.ClassInterceptSelectors.AddRange(classInterceptSelectors);


            InterceptExtensions.CollectionInterceptPoint(types);//收集拦截点
            containerBuilder.InjectionInterceptPoints();//注入拦截点

            InterceptExtensions.CollectionInterceptedTypeInfo(types);//收集被拦截的类型以及信息
           



            //批量注入 在收集拦截点之后 由于涉及到拦截配置
            containerBuilder.BatchInjection(types);

            RegisterCallBack(containerBuilder);//依赖注入回调

            return containerBuilder;
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
