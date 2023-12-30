using Autofac;
using Microsoft.Extensions.DependencyInjection;
using OpenDeepSpace.Autofacastle.DependencyInjection;
using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenDeepSpace.Autofacastle.AspectAttention.InterceptorPoint;
using OpenDeepSpace.Autofacastle.Extensions;
using OpenDeepSpace.Autofacastle;
using Autofac.Extensions.DependencyInjection;

namespace OpenDeepSpace.Autofacastle.Extensions
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// 批量注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies">程序集</param>
        /// <returns></returns>
        public static IServiceCollection BatchInjection(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));



            return BatchInjection(services, assemblies.SelectMany(t => t.GetTypes()));
        }


        /// <summary>
        /// 批量注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IServiceCollection BatchInjection(this IServiceCollection services, IEnumerable<Type> types)
        {

            return BatchInjectionInternal(services, types);
        }

        /// <summary>
        /// 批量注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private static IServiceCollection BatchInjectionInternal(this IServiceCollection services, IEnumerable<Type> types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            
            //加入自身的程序集
            types =types.Union(typeof(ServiceCollectionExtensions).Assembly.GetTypes());

            var result = types.CollectionBatchInjection();

            Dictionary<Type, List<ImplementationDescription>> serviceDic = result.Item1;
            Dictionary<Type, List<ImplementationDescription>> replaceServiceDic = result.Item2;

            //实现向ServiceCollection中注入
            foreach (var service in serviceDic.Keys)
            {

                foreach (var implementation in serviceDic[service])
                {
                    //修复类型
                    var fixService = service.FixTypeReference();
                    var fixImplementation = implementation.ImplementationType.FixTypeReference();

                    //存在注入自身的情况
                    services.Add(new ServiceDescriptor(fixService, fixImplementation, implementation.ServiceLifetime));


                }


            }

            //执行替换服务操作
            foreach (var service in replaceServiceDic.Keys)
            {
                foreach (var implementation in replaceServiceDic[service])
                {
                    //修复类型
                    var fixService = service.FixTypeReference();
                    var fixImplementation = implementation.ImplementationType.FixTypeReference();

                    services.ReplaceService(fixService, fixImplementation, implementation.ServiceLifetime);

                }
            }

            return services;
        }

        

      
        /// <summary>
        /// 替换服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IServiceCollection ReplaceService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            ServiceDescriptor descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);
            services.Replace(descriptor);
            return services;
        }



        /// <summary>
        /// 注入拦截点 
        /// 注入拦截前之前先调用收集拦截点
        /// <see cref="InterceptExtensions.CollectionInterceptPoint"/>
        /// 或
        /// <see cref="InterceptExtensions.CollectionInterceptPoint(IEnumerable{Assembly})"/>
        /// 或
        /// <see cref="InterceptExtensions.CollectionInterceptPoint(IEnumerable{Type})"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection InjectionInterceptPoints(this IServiceCollection services)
        {
            //把收集到的拦截点注入到容器
            foreach (var interceptPointInfo in InterceptPointCollection.interceptPointInfos)
            {
                services.AddTransient(interceptPointInfo.InterceptPointType);
            }

            return services;
        }

       
    }
}
