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
        /// 
        /// 该方法与
        /// <see cref="ContainerBuilderExtensions.BatchInjection(ContainerBuilder)"/>
        /// 结合使用完成批量注入 特性依赖注入
        /// 
        /// 或单独使用该方法来完成特性依赖注入
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="automaticInjectionSelectors"></param>
        /// <returns></returns>
        public static ContainerBuilder UseAutofacastle(this ContainerBuilder containerBuilder, List<AutomaticInjectionSelector> automaticInjectionSelectors = null)
        {
            if (automaticInjectionSelectors != null && automaticInjectionSelectors.Any())
            { 
                AutofacastleCollection.AutomaticInjectionSelectors.AddRange(automaticInjectionSelectors);
            }

            RegisterCallBack(containerBuilder);

            return containerBuilder;
        }

        /// <summary>
        /// 该方法单独使用完成批量注入 特性依赖注入 切面拦截
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="automaticInjectionSelectors"></param>
        /// <param name="nonInterceptSelectors"></param>
        /// <param name="classInterceptSelectors"></param>
        /// <param name="IsConfigureIntercept"></param>
        /// <returns></returns>
        public static ContainerBuilder UseAutofacastle(this ContainerBuilder containerBuilder, List<AutomaticInjectionSelector> automaticInjectionSelectors = null, List<NonInterceptSelector> nonInterceptSelectors = null, List<ClassInterceptSelector> classInterceptSelectors = null, bool IsConfigureIntercept=false)
        {

            if (automaticInjectionSelectors != null && automaticInjectionSelectors.Any())
                AutofacastleCollection.AutomaticInjectionSelectors.AddRange(automaticInjectionSelectors);
            if(nonInterceptSelectors != null && nonInterceptSelectors.Any())
                AutofacastleCollection.NonInterceptSelectors.AddRange(nonInterceptSelectors);
            if(classInterceptSelectors!=null && classInterceptSelectors.Any())
                AutofacastleCollection.ClassInterceptSelectors.AddRange(classInterceptSelectors);


            ContainerBuilderExtensions.IsConfigureIntercept = IsConfigureIntercept;

            if (IsConfigureIntercept)//是否配置拦截
            {
                InterceptExtensions.CollectionInterceptPoint();//收集拦截点
                containerBuilder.InjectionInterceptPoints();//注入拦截点
                InterceptExtensions.CollectionInterceptedTypeInfo();//收集被拦截的类型以及信息
            }

            //批量注入 在收集拦截点之后 由于涉及到拦截配置
            containerBuilder.BatchInjection();

            RegisterCallBack(containerBuilder);//依赖注入回调

            return containerBuilder;
        }

        /// <summary>
        /// 该方法单独使用完成批量注入 特性依赖注入 切面拦截
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="assemblies">程序集</param>
        /// <param name="automaticInjectionSelectors"></param>
        /// <param name="nonInterceptSelectors"></param>
        /// <param name="classInterceptSelectors"></param>
        /// <param name="IsConfigureIntercept"></param>
        /// <returns></returns>
        public static ContainerBuilder UseAutofacastle(this ContainerBuilder containerBuilder,List<Assembly> assemblies,List<AutomaticInjectionSelector> automaticInjectionSelectors = null, List<NonInterceptSelector> nonInterceptSelectors = null,List<ClassInterceptSelector> classInterceptSelectors=null, bool IsConfigureIntercept = false)
        {

            if (automaticInjectionSelectors != null && automaticInjectionSelectors.Any())
                AutofacastleCollection.AutomaticInjectionSelectors.AddRange(automaticInjectionSelectors);
            if (nonInterceptSelectors != null && nonInterceptSelectors.Any())
                AutofacastleCollection.NonInterceptSelectors.AddRange(nonInterceptSelectors);
            if(classInterceptSelectors!=null && classInterceptSelectors.Any())
                AutofacastleCollection.ClassInterceptSelectors.AddRange(classInterceptSelectors);

            ContainerBuilderExtensions.IsConfigureIntercept = IsConfigureIntercept;

            if (IsConfigureIntercept)//是否配置拦截
            {
                InterceptExtensions.CollectionInterceptPoint(assemblies);//收集拦截点
                containerBuilder.InjectionInterceptPoints();//注入拦截点
                InterceptExtensions.CollectionInterceptedTypeInfo(assemblies);//收集被拦截的类型以及信息
            }

            //批量注入 在收集拦截点之后 由于涉及到拦截配置
            containerBuilder.BatchInjection(assemblies);

            RegisterCallBack(containerBuilder);//依赖注入回调

            return containerBuilder;
        }

        /// <summary>
        /// 该方法单独使用完成批量注入 特性依赖注入 切面拦截
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="types">类型集</param>
        /// <param name="automaticInjectionSelectors"></param>
        /// <param name="nonInterceptSelectors"></param>
        /// <param name="classInterceptSelectors"></param>
        /// <param name="IsConfigureIntercept"></param>
        /// <returns></returns>
        public static ContainerBuilder UseAutofacastle(this ContainerBuilder containerBuilder,List<Type> types,List<AutomaticInjectionSelector> automaticInjectionSelectors = null, List<NonInterceptSelector> nonInterceptSelectors = null, List<ClassInterceptSelector> classInterceptSelectors = null, bool IsConfigureIntercept = false)
        {

            if (automaticInjectionSelectors != null && automaticInjectionSelectors.Any())
                AutofacastleCollection.AutomaticInjectionSelectors.AddRange(automaticInjectionSelectors);
            if (nonInterceptSelectors != null && nonInterceptSelectors.Any())
                AutofacastleCollection.NonInterceptSelectors.AddRange(nonInterceptSelectors);
            if (classInterceptSelectors != null && classInterceptSelectors.Any())
                AutofacastleCollection.ClassInterceptSelectors.AddRange(classInterceptSelectors);

            ContainerBuilderExtensions.IsConfigureIntercept = IsConfigureIntercept;

            if (IsConfigureIntercept)//是否配置拦截
            {
                InterceptExtensions.CollectionInterceptPoint(types);//收集拦截点
                containerBuilder.InjectionInterceptPoints();//注入拦截点
                InterceptExtensions.CollectionInterceptedTypeInfo(types);//收集被拦截的类型以及信息
            }

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
