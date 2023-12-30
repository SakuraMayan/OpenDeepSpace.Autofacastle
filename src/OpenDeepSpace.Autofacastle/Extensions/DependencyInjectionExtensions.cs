using Microsoft.Extensions.DependencyInjection;
using OpenDeepSpace.Autofacastle.AspectAttention;
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
    /// 类型拓展
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// 过滤掉依赖注入接口
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Type[] FilterDependencyInjectionInterfaces(this Type[] types)
        {

            types = types.Where(t => t != typeof(ITransient) && t != typeof(IScoped) &&
            t != typeof(ISingleton) && t!=typeof(INonIntercept) && t!=typeof(IClassIntercept) 
            && t!=typeof(IImplementServiceOrder) && t != typeof(IServiceLifetime)).ToArray();

            return types;
        }

        /// <summary>
        /// 是否有依赖注入特性和接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsHasDependencyInjectionAttributeOrInterface(this Type type)
        {
            return
                type.GetCustomAttributes().Select(t => t.GetType()).
                    Any(t => t == typeof(TransientAttribute) ||
                           t == typeof(ScopedAttribute) ||
                           t == typeof(SingletonAttribute)
                        )
                    ||

                type.GetInterfaces().Any(t => t == typeof(ITransient)
                          || t == typeof(IScoped)
                          || t == typeof(ISingleton)
                    );


        }

        public static bool IsHasDependencyInjectionInterface(this Type type)
        {
            return type.GetInterfaces().Any(t => t == typeof(ITransient)
                                      || t == typeof(IScoped)
                                      || t == typeof(ISingleton)
                                );
        }

        public static bool IsHasDependencyInjectionAttribute(this Type type)
        {
            return type.GetCustomAttributes().Select(t => t.GetType()).
                                Any(t => t == typeof(TransientAttribute) ||
                                       t == typeof(ScopedAttribute) ||
                                       t == typeof(SingletonAttribute)
                                    );
        }


        /// <summary>
        /// 收集批量注入
        /// </summary>
        /// <param name="types"></param>
        /// <returns>第一个为服务列表 第二个为替换服务的列表</returns>
        public static (Dictionary<Type, List<ImplementationDescription>>, Dictionary<Type, List<ImplementationDescription>>) CollectionBatchInjection(this IEnumerable<Type> types)
        {
            //筛选出标有依赖注入特性和依赖接口注入的类
            types = types.Where(t => t.IsClass && !t.IsAbstract && t.IsHasDependencyInjectionAttributeOrInterface()).ToList().Distinct();

            ///反向筛选出服务
            ///服务对应的实现集合字典 K:service V:<see cref="ImplementationDescription"/>
            Dictionary<Type, List<ImplementationDescription>> serviceDic = new Dictionary<Type, List<ImplementationDescription>>();
            Dictionary<Type, List<ImplementationDescription>> replaceServiceDic = new Dictionary<Type, List<ImplementationDescription>>();
            foreach (var type in types)
            {
                //优先从接口依赖判断起 因此使用接口依赖的生命周期在前 在按照先写先注入 后写后注入 的方式依次完成注入
                if (type.IsHasDependencyInjectionInterface())//存在依赖注入接口
                {

                    var serviceTypes = type.GetInterfaces().FilterDependencyInjectionInterfaces();


                    //如果通过接口依赖注入 实现了多个依赖注入接口 都需要注入
                    var diInterfaces = type.GetInterfaces().Where(t => t.GetInterfaces().Contains(typeof(IServiceLifetime)));
                    foreach (var diInterface in diInterfaces)
                    {
                        if (diInterface == typeof(ITransient))
                            SetServiceDic(serviceDic, type, 0, serviceTypes, ServiceLifetime.Transient,null);
                        if (diInterface == typeof(IScoped))
                            SetServiceDic(serviceDic, type, 0, serviceTypes, ServiceLifetime.Scoped, null);
                        if (diInterface == typeof(ISingleton))
                            SetServiceDic(serviceDic, type, 0, serviceTypes, ServiceLifetime.Singleton, null);
                    }

                }

                if (type.IsHasDependencyInjectionAttribute())//存在依赖注入特性
                {


                    var diAttrs = type.GetCustomAttributes().Where(t => (typeof(DependencyInjectionAttribute)).IsAssignableFrom(t.GetType()));

                    foreach (var diAttr in diAttrs)
                    {

                        if (diAttr is TransientAttribute transientAttr)
                        {
                            SetServiceDic(serviceDic, type, transientAttr, ServiceLifetime.Transient);
                            SetReplaceServiceDic(replaceServiceDic, type, transientAttr, ServiceLifetime.Transient);

                        }

                        if (diAttr is ScopedAttribute scopedAttr)
                        {

                            SetServiceDic(serviceDic, type, scopedAttr, ServiceLifetime.Scoped);
                            SetReplaceServiceDic(replaceServiceDic, type, scopedAttr, ServiceLifetime.Scoped);


                        }

                        if (diAttr is SingletonAttribute singletonAttr)
                        {
                            SetServiceDic(serviceDic, type, singletonAttr, ServiceLifetime.Singleton);
                            SetReplaceServiceDic(replaceServiceDic, type, singletonAttr, ServiceLifetime.Singleton);

                            //是否预加载 且不能是动态泛型
                            if (singletonAttr != null && singletonAttr.AutoActivate && !type.IsGenericTypeDefinition)
                                AutofacastleCollection.AutoActives.Add(type);
                        }
                    }



                }

            }

            return (serviceDic, replaceServiceDic);
        }

        /// <summary>
        /// 设置ServiceDic字典
        /// </summary>
        /// <param name="serviceDic"></param>
        /// <param name="type"></param>
        /// <param name="order"></param>
        /// <param name="serviceTypes"></param>
        /// <param name="serviceLifetime"></param>
        private static void SetServiceDic(Dictionary<Type, List<ImplementationDescription>> serviceDic, Type type, int order, Type[] serviceTypes, ServiceLifetime serviceLifetime,DependencyInjectionAttribute diAttr)
        {

            var newImplementationTypeOrder = new ImplementationDescription()
            {
                ImplementationType = type,
                ImplementServiceOrder = order,
                ServiceLifetime = serviceLifetime,
                DependencyInjectionAttribute = diAttr,
                AutoActivate = diAttr != null ? diAttr.GetType().GetInterfaces().Contains(typeof(IAutoActivate)) ? (diAttr as IAutoActivate).AutoActivate : false:false
            };

            //添加自身
            serviceDic[type] = new List<ImplementationDescription>() { newImplementationTypeOrder };

            foreach (var serviceType in serviceTypes)
            {
                if (serviceDic.ContainsKey(serviceType))
                {
                    //如果存在相同序号 且实现类型也相同 且生命周期也相同就不添加
                    bool existSame = serviceDic[serviceType].Exists(t => t.ImplementationType == newImplementationTypeOrder.ImplementationType && t.ImplementServiceOrder == newImplementationTypeOrder.ImplementServiceOrder && t.ServiceLifetime == newImplementationTypeOrder.ServiceLifetime);

                    if (!existSame)
                        serviceDic[serviceType].Add(newImplementationTypeOrder);

                    //排序
                    serviceDic[serviceType] = serviceDic[serviceType].OrderBy(t => t.ImplementServiceOrder).ToList();
                }
                else
                {
                    serviceDic[serviceType] = new List<ImplementationDescription>() { newImplementationTypeOrder };
                }

            }
        }

        private static void SetServiceDic(Dictionary<Type, List<ImplementationDescription>> serviceDic, Type type, DependencyInjectionAttribute diAttr, ServiceLifetime serviceLifetime)
        {

            if (diAttr.AsServices == null || (diAttr != null && !diAttr.AsServices.Any()))
            {//AsServices为空 或者 AsServices没有任何元素 

                //获取type实现的接口并过滤掉依赖注入接口
                diAttr.AsServices = type.GetInterfaces().FilterDependencyInjectionInterfaces();


            }

            //如果指定了Keyed或Named尝试添加
            type.TryAddExistSameKeyedOrNamed(diAttr);

            SetServiceDic(serviceDic, type, diAttr.ImplementServiceOrder, diAttr.AsServices, serviceLifetime,diAttr);

        }

        private static void SetReplaceServiceDic(Dictionary<Type, List<ImplementationDescription>> replaceServiceDic, Type type, DependencyInjectionAttribute diAttr, ServiceLifetime serviceLifetime)
        {


            //替换服务不为空并且存在值
            if (diAttr.ReplaceServices != null && diAttr.ReplaceServices.Any())
                SetServiceDic(replaceServiceDic, type, diAttr.ImplementServiceOrder, diAttr.ReplaceServices, serviceLifetime, diAttr);

        }

        /// <summary>
        /// 尝试添加相同的Keyed 如果Keyed存在 抛出异常 否则添加
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dependencyInjectionAttribute"></param>
        public static void TryAddExistSameKeyed(this Type type,DependencyInjectionAttribute dependencyInjectionAttribute)
        {
            //如果Keyed不为空
            if (dependencyInjectionAttribute != null && dependencyInjectionAttribute.Keyed != null)
            {
                if (AutofacastleCollection.KeyedImplementationTypes.ContainsKey(dependencyInjectionAttribute.Keyed))
                {
                    //不是同一类型
                    if (AutofacastleCollection.KeyedImplementationTypes[dependencyInjectionAttribute.Keyed] != type)
                        throw new Exception($"已经存在一个相同的Keyed:{dependencyInjectionAttribute.Keyed}在{AutofacastleCollection.KeyedImplementationTypes[dependencyInjectionAttribute.Keyed]}类上");
                    else
                        return;//是同一类型
                }
                AutofacastleCollection.KeyedImplementationTypes[dependencyInjectionAttribute.Keyed] = type;
            }
        }

        /// <summary>
        /// 尝试添加相同的Named 如果Named存在抛出异常 否则添加
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dependencyInjectionAttribute"></param>
        public static void TryAddExistSameNamed(this Type type,DependencyInjectionAttribute dependencyInjectionAttribute)
        {
            //如果Named不为空
            if (dependencyInjectionAttribute != null && !string.IsNullOrWhiteSpace(dependencyInjectionAttribute.Named))
            {
                if (AutofacastleCollection.NamedImplementationTypes.ContainsKey(dependencyInjectionAttribute.Named))
                {
                    //如果不是同一个类型
                    if (AutofacastleCollection.NamedImplementationTypes[dependencyInjectionAttribute.Named] != type)
                        throw new Exception($"已经存在一个相同的Named:{dependencyInjectionAttribute.Named}在{AutofacastleCollection.NamedImplementationTypes[dependencyInjectionAttribute.Named]}类上");
                    else
                        return;//是同一类型直接返回
                }
                AutofacastleCollection.NamedImplementationTypes[dependencyInjectionAttribute.Named] = type;
            }
        }

        /// <summary>
        /// 尝试添加相同的Keyed或Named
        /// </summary>
        /// <param name="dependencyInjectionAttribute"></param>
        public static void TryAddExistSameKeyedOrNamed(this Type type,DependencyInjectionAttribute dependencyInjectionAttribute)
        {
            TryAddExistSameKeyed(type, dependencyInjectionAttribute);
            TryAddExistSameNamed(type, dependencyInjectionAttribute);
        }
    }
}
