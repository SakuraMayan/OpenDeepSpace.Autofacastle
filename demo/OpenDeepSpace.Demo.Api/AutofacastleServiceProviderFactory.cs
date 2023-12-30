using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using OpenDeepSpace.Autofacastle;
using OpenDeepSpace.Autofacastle.AspectAttention;
using OpenDeepSpace.Autofacastle.DependencyInjection;
using OpenDeepSpace.Autofacastle.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Demo.Api
{
    public class AutofacastleServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly Action<ContainerBuilder>? _configurationAction;
        private readonly IEnumerable<Type> types;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies">需要使用Autofacastle的程序集</param>
        /// <param name="configurationAction"></param>
        /// <param name="automaticInjectionSelectors"></param>
        /// <param name="nonInterceptSelectors"></param>
        /// <param name="classInterceptSelectors"></param>
        public AutofacastleServiceProviderFactory(IEnumerable<Assembly> assemblies,Action<ContainerBuilder>? configurationAction = null, List<AutomaticInjectionSelector>? automaticInjectionSelectors=null,List<NonInterceptSelector>? nonInterceptSelectors=null,List<ClassInterceptSelector>? classInterceptSelectors=null):this(assemblies.SelectMany(t => t.GetTypes()), configurationAction, automaticInjectionSelectors, nonInterceptSelectors, classInterceptSelectors)
        {
            if(assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types">需要使用Autofacastle的类型集</param>
        /// <param name="configurationAction"></param>
        /// <param name="automaticInjectionSelectors"></param>
        /// <param name="nonInterceptSelectors"></param>
        /// <param name="classInterceptSelectors"></param>
        public AutofacastleServiceProviderFactory(IEnumerable<Type> types, Action<ContainerBuilder>? configurationAction = null, List<AutomaticInjectionSelector>? automaticInjectionSelectors = null, List<NonInterceptSelector>? nonInterceptSelectors = null, List<ClassInterceptSelector>? classInterceptSelectors = null)
        {
            this.types = types;

            _configurationAction = configurationAction ?? (builder => { });
            if (automaticInjectionSelectors != null && automaticInjectionSelectors.Any())
            {
                AutofacastleCollection.AutomaticInjectionSelectors.AddRange(automaticInjectionSelectors);
            }
            if (nonInterceptSelectors != null && nonInterceptSelectors.Any())
            {
                AutofacastleCollection.NonInterceptSelectors.AddRange(nonInterceptSelectors);
            }
            if (classInterceptSelectors != null && classInterceptSelectors.Any())
            {
                AutofacastleCollection.ClassInterceptSelectors.AddRange(classInterceptSelectors);
            }
        }


        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            //批量注册
            services.BatchInjection(types);

            //服务替换

            //收集拦截点
            InterceptExtensions.CollectionInterceptPoint(types);
            //注入拦截点
            services.InjectionInterceptPoints();
            //收集被拦截的类集
            InterceptExtensions.CollectionInterceptedTypeInfo(types);
            //尝试注入没有被注入的被拦截类
            //services.TryInjectionInterceptedType();//这里有点问题 应该是所有被拦截的类都需要注入的容器中而不是自己指定
 
            builder.Populate(services);

            if (_configurationAction == null)
                throw new InvalidOperationException($"{nameof(_configurationAction)}为空");

            _configurationAction(builder);

            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            if(containerBuilder==null)
                throw new ArgumentNullException(nameof(containerBuilder));
            return new AutofacServiceProvider(containerBuilder.Build());
        }
    }
}
