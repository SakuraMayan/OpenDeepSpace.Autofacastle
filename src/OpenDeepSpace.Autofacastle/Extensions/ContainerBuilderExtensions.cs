﻿using Autofac;
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
            types = types = types.Union(typeof(ContainerBuilderExtensions).Assembly.GetTypes());

            //排序 暂时这样调整 实际上应该按照实现类 来进行分组排序的
            types = types.Where(t => t.IsClass && !t.IsAbstract).OrderBy(t => t is IImplementServiceOrder?(t as IImplementServiceOrder).ImplementServiceOrder : t.GetCustomAttribute<TransientAttribute>() != null ? (t.GetCustomAttribute<TransientAttribute>() as IImplementServiceOrder).ImplementServiceOrder :
            t.GetCustomAttribute<ScopedAttribute>() != null ? (t.GetCustomAttribute<ScopedAttribute>() as IImplementServiceOrder).ImplementServiceOrder
            : t.GetCustomAttribute<SingletonAttribute>() != null ? (t.GetCustomAttribute<SingletonAttribute>() as IImplementServiceOrder).ImplementServiceOrder
            : 0);

            foreach (var type in types)
            {
                //瞬时
                if (type.IsAssignableTo<ITransient>() || type.GetCustomAttribute<TransientAttribute>() != null)
                {

                    var transientAttr = type.GetCustomAttribute<TransientAttribute>();

                    //如果指定了Keyed或Named尝试添加
                    type.TryAddExistSameKeyedOrNamed(transientAttr);

                    //判断是否动态泛型
                    if (type.IsGenericTypeDefinition)//动态泛型
                    {
                        if (type.ContainsGenericParameters)//包含泛型参数 实现的为开放泛型才注入:xxx<> 实现的非开放泛型不能注入:xxx
                        {
                            //注册自身
                            var registrationBuilder = containerBuilder.RegisterGeneric(type).AsSelf();
                            if (transientAttr != null && transientAttr.AsServices != null && transientAttr.AsServices.Any())
                            {//指定服务注入

                                foreach (var service in transientAttr.AsServices)
                                {

                                    registrationBuilder = AsServiceForGenericType(type, transientAttr, registrationBuilder, service);
                                    //设置生命周期
                                    registrationBuilder.InstancePerDependency();

                                  
                                }
                                
                            }
                            else
                            {//未指定服务注入 查找实现的相关接口注入 
                                foreach (var service in type.GetInterfaces().FilterDependencyInjectionInterfaces())
                                {
                                    registrationBuilder = AsServiceForGenericType(type, transientAttr, registrationBuilder, service);
                                    //设置生命周期
                                    registrationBuilder.InstancePerDependency();

                                   
                                }
                            }
                            //配置拦截
                            registrationBuilder.AddIntercept(type,false);
                        }


                    }
                    else
                    {
                        //注册自身
                        var registrationBuilder = containerBuilder.RegisterType(type).AsSelf();

                        if (transientAttr != null && transientAttr.AsServices != null && transientAttr.AsServices.Any())
                        {//指定服务注入
                            foreach (var service in transientAttr.AsServices)
                            {
                                registrationBuilder = AsService(type, transientAttr, registrationBuilder, service);

                                //设置生命周期
                                registrationBuilder.InstancePerDependency();

                            }
                            
                        }
                        else
                        {//未指定服务注入 查找实现的相关接口注入 
                            foreach (var service in type.GetInterfaces().FilterDependencyInjectionInterfaces())
                            {
                                registrationBuilder = AsService(type, transientAttr, registrationBuilder, service);
                                //设置生命周期
                                registrationBuilder.InstancePerDependency();

                            }
                        }
                        //配置拦截
                        registrationBuilder.AddIntercept(type,false);
                    }

                }

                //一次请求
                if (type.IsAssignableTo<IScoped>() || type.GetCustomAttribute<ScopedAttribute>() != null)
                {
                    var scopedAttr = type.GetCustomAttribute<ScopedAttribute>();
                    //如果指定了Keyed或Named尝试添加
                    type.TryAddExistSameKeyedOrNamed(scopedAttr);
                    //判断是否动态泛型
                    if (type.IsGenericTypeDefinition)//动态泛型
                    {
                        if (type.ContainsGenericParameters)//包含泛型参数 实现的为开放泛型才注入:xxx<> 实现的非开放泛型不能注入:xxx
                        {
                            //注册自身
                            var registrationBuilder = containerBuilder.RegisterGeneric(type).AsSelf();
                            if (scopedAttr != null && scopedAttr.AsServices != null && scopedAttr.AsServices.Any())
                            {//指定服务注入
                                foreach (var service in scopedAttr.AsServices)
                                {
                                    registrationBuilder = AsServiceForGenericType(type, scopedAttr, registrationBuilder, service);

                                    //设置生命周期
                                    registrationBuilder.InstancePerLifetimeScope();

                                   
                                }
                               
                            }
                            else
                            {//未指定服务注入 查找实现的相关接口注入 
                                foreach (var service in type.GetInterfaces().FilterDependencyInjectionInterfaces())
                                {
                                    registrationBuilder = AsServiceForGenericType(type, scopedAttr, registrationBuilder, service);
                                    //设置生命周期
                                    registrationBuilder.InstancePerLifetimeScope();

                                   
                                }
                                
                            }
                            //配置拦截
                            registrationBuilder.AddIntercept(type, false);
                        }


                    }
                    else
                    {
                        //注册自身
                        var registrationBuilder = containerBuilder.RegisterType(type).AsSelf();

                        if (scopedAttr != null && scopedAttr.AsServices != null && scopedAttr.AsServices.Any())
                        {//指定服务注入
                            foreach (var service in scopedAttr.AsServices)
                            {
                                registrationBuilder = AsService(type, scopedAttr, registrationBuilder, service);

                                //设置生命周期
                                registrationBuilder.InstancePerLifetimeScope();

                            }
                            
                        }
                        else
                        {//未指定服务注入 查找实现的相关接口注入 
                            foreach (var service in type.GetInterfaces().FilterDependencyInjectionInterfaces())
                            {
                                registrationBuilder = AsService(type, scopedAttr, registrationBuilder, service);
                                //设置生命周期
                                registrationBuilder.InstancePerLifetimeScope();

                            }
                        }
                        //配置拦截
                        registrationBuilder.AddIntercept(type, false);
                    }
                }

                //单例
                if (type.IsAssignableTo<ISingleton>() || type.GetCustomAttribute<SingletonAttribute>() != null)
                {
                    var singletonAttr = type.GetCustomAttribute<SingletonAttribute>();
                    //如果指定了Keyed或Named尝试添加
                    type.TryAddExistSameKeyedOrNamed(singletonAttr);
                    //判断是否动态泛型
                    if (type.IsGenericTypeDefinition)//动态泛型
                    {
                        if (type.ContainsGenericParameters)//包含泛型参数 实现的为开放泛型才注入:xxx<> 实现的非开放泛型不能注入:xxx
                        {
                            //注册自身
                            var registrationBuilder = containerBuilder.RegisterGeneric(type).AsSelf();
                            if (singletonAttr != null && singletonAttr.AsServices != null && singletonAttr.AsServices.Any())
                            {//指定服务注入
                                foreach (var service in singletonAttr.AsServices)
                                {
                                    registrationBuilder = AsServiceForGenericType(type, singletonAttr, registrationBuilder, service);

                                    //设置生命周期
                                    registrationBuilder.SingleInstance();

                                }
                               
                            }
                            else
                            {//未指定服务注入 查找实现的相关接口注入 
                                foreach (var service in type.GetInterfaces().FilterDependencyInjectionInterfaces())
                                {
                                    registrationBuilder = AsServiceForGenericType(type, singletonAttr, registrationBuilder, service);
                                    //设置生命周期
                                    registrationBuilder.SingleInstance();

                                    
                                }
                                
                            }
                            //配置拦截
                            registrationBuilder.AddIntercept(type, false);
                        }


                    }
                    else
                    {
                        //注册自身
                        var registrationBuilder = containerBuilder.RegisterType(type).AsSelf();

                        if (singletonAttr != null && singletonAttr.AsServices != null && singletonAttr.AsServices.Any())
                        {//指定服务注入
                            foreach (var service in singletonAttr.AsServices)
                            {
                                registrationBuilder = AsService(type, singletonAttr, registrationBuilder, service);

                                if (singletonAttr.AutoActivate)
                                    registrationBuilder.AutoActivate();//预加载

                                //设置生命周期
                                registrationBuilder.SingleInstance();

                            }
                         
                        }
                        else
                        {//未指定服务注入 查找实现的相关接口注入 
                            foreach (var service in type.GetInterfaces().FilterDependencyInjectionInterfaces())
                            {
                                registrationBuilder = AsService(type, singletonAttr, registrationBuilder, service);
                                if (singletonAttr != null && singletonAttr.AutoActivate)
                                    registrationBuilder.AutoActivate();//预加载
                                //设置生命周期
                                registrationBuilder.SingleInstance();

                            }
                            
                        }
                        //配置拦截
                        registrationBuilder.AddIntercept(type, false);
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
