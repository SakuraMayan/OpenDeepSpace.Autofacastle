using Autofac;
using Autofac.Builder;
using Autofac.Core;
using OpenDeepSpace.Autofacastle.AspectAttention.InterceptorPoint;
using OpenDeepSpace.Autofacastle.DependencyInjection;
using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Autofacastle.Extensions
{
    /// <summary>
    /// ConatinerBuilder拓展
    /// </summary>
    public static class ContainerBuilderExtensions
    {

        /// <summary>
        /// 批量注入
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="assemblies">程序集</param>
        /// <returns></returns>
        public static ContainerBuilder BatchInjection(this ContainerBuilder containerBuilder, IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));


            var types = assemblies.SelectMany(t=>t.GetTypes());

            BatchInjectionInternal(containerBuilder, types);

            return containerBuilder;
        }

        /// <summary>
        /// 批量注入
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="types">类型集合</param>
        /// <returns></returns>
        public static ContainerBuilder BatchInjection(this ContainerBuilder containerBuilder, IEnumerable<Type> types)
        {

            if (types == null)
                throw new ArgumentNullException(nameof(types));

            BatchInjectionInternal(containerBuilder, types);

            return containerBuilder;
        }

     

        /// <summary>
        /// 注入拦截点 
        /// 注入拦截前之前先调用收集拦截点
        /// <see cref="InterceptExtensions.CollectionInterceptPoint"/>
        /// 或
        /// <see cref="InterceptExtensions.CollectionInterceptPoint(List{Assembly})"/>
        /// 或
        /// <see cref="InterceptExtensions.CollectionInterceptPoint(List{Type})"/>
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public static ContainerBuilder InjectionInterceptPoints(this ContainerBuilder containerBuilder)
        {
            //把收集到的拦截点注入到容器
            foreach (var interceptPointInfo in InterceptPointCollection.interceptPointInfos)
            {
                containerBuilder.RegisterType(interceptPointInfo.InterceptPointType).AsSelf().InstancePerDependency();
            }

            return containerBuilder;
        }

        private static void BatchInjectionInternal(ContainerBuilder containerBuilder, IEnumerable<Type> types)
        {
            //内部自动加入
            types = types.Union(typeof(ContainerBuilderExtensions).Assembly.GetTypes());

            //收集拦截点
            var result = types.CollectionBatchInjection();

            Dictionary<Type, List<ImplementationDescription>> serviceDic = result.Item1;


            //实现向ContainerBuilder中注入
            foreach (var service in serviceDic.Keys)
            {

                foreach (var implementation in serviceDic[service])
                {
                    //修复类型
                    var fixService = service.FixTypeReference();
                    var fixImplementation = implementation.ImplementationType.FixTypeReference();


                    //判断是否动态泛型
                    if (fixImplementation.IsGenericTypeDefinition)//动态泛型
                    {
                        if (fixImplementation.ContainsGenericParameters)//包含泛型参数 实现的为开放泛型才注入:xxx<> 实现的非开放泛型不能注入:xxx
                        {
                            //注册自身
                            var registrationBuilder = containerBuilder.RegisterGeneric(fixService).AsSelf();
                            registrationBuilder = AsServiceForGenericType(fixImplementation, implementation.DependencyInjectionAttribute, registrationBuilder, fixService);

                            switch (implementation.ServiceLifetime)
                            {
                                case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton:
                                    registrationBuilder.SingleInstance();
                                    break;
                                case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped:
                                    registrationBuilder.InstancePerLifetimeScope();
                                    break;
                                case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient:
                                    registrationBuilder.InstancePerDependency();
                                    break;
                                default:
                                    break;
                            }
                            
                            
                            //配置拦截
                            registrationBuilder.AddIntercept(fixImplementation, false);
                        }


                    }
                    else
                    {
                        //注册自身
                        var registrationBuilder = containerBuilder.RegisterType(fixImplementation).AsSelf();
                        registrationBuilder = AsService(fixImplementation, implementation.DependencyInjectionAttribute, registrationBuilder, fixService);

                        //设置生命周期
                        switch (implementation.ServiceLifetime)
                        {
                            case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton:
                                registrationBuilder.SingleInstance();
                                if (implementation.AutoActivate)//预加载
                                    registrationBuilder.AutoActivate();
                                break;
                            case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped:
                                registrationBuilder.InstancePerLifetimeScope();
                                break;
                            case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient:
                                registrationBuilder.InstancePerDependency();
                                break;
                            default:
                                break;
                        }

                        //配置拦截
                        registrationBuilder.AddIntercept(fixImplementation, false);
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dependencyInjectionAttribute"></param>
        /// <param name="registrationBuilder"></param>
        /// <param name="service"></param>
        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> AsService(Type type, DependencyInjectionAttribute dependencyInjectionAttribute, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder, Type service)
        {

            //作为服务
            registrationBuilder.As(service);

            registrationBuilder.Keyed(type, service);
            if (!string.IsNullOrWhiteSpace(type.FullName))
                registrationBuilder.Named(type.FullName, service);

            if (dependencyInjectionAttribute != null)
            { 
            
                //如果存在Keyed
                if (dependencyInjectionAttribute.Keyed != null)
                    registrationBuilder.Keyed(dependencyInjectionAttribute.Keyed, service);
                //如果Named存在
                if (!string.IsNullOrWhiteSpace(dependencyInjectionAttribute.Named))
                    registrationBuilder.Named(dependencyInjectionAttribute.Named, service);
            }

            return registrationBuilder;
        }

        /// <summary>
        /// 泛型类的作为服务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dependencyInjectionAttr"></param>
        /// <param name="registrationBuilder"></param>
        /// <param name="service"></param>
        private static IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> AsServiceForGenericType(Type type, DependencyInjectionAttribute dependencyInjectionAttr, IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> registrationBuilder, Type service)
        {
            
            //如果直接获取的出来Ixxx<> FullName为空 
            //针对泛型 FullName为空导致为非泛型 补充完整FullName 才能正确批量注入动态泛型
            //例如(typeof(Ixxx<>),typeof(xxx()))
            service = service.FixTypeReference();
               
            //作为服务
            registrationBuilder.As(service);

            registrationBuilder.Keyed(type, service);
            if (!string.IsNullOrWhiteSpace(type.FullName))
                registrationBuilder.Named(type.FullName, service);

            if (dependencyInjectionAttr != null)
            { 
                //如果存在Keyed
                if (dependencyInjectionAttr.Keyed != null)
                    registrationBuilder.Keyed(dependencyInjectionAttr.Keyed, service);
                //如果Named存在
                if (!string.IsNullOrWhiteSpace(dependencyInjectionAttr.Named))
                    registrationBuilder.Named(dependencyInjectionAttr.Named, service);
            }

            return registrationBuilder;
        }
    }
}
