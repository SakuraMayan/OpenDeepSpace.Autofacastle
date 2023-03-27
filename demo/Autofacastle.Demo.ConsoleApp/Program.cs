// See https://aka.ms/new-console-template for more information
using Autofac;
using Autofacastle.Demo.ConsoleApp.Services;
using OpenDeepSpace.Autofacastle.DependencyInjection;
using OpenDeepSpace.Autofacastle.Extensions;

Console.WriteLine("Hello, World!");

ContainerBuilder containerBuilder = new ContainerBuilder();


//使用Autofacastle 并进行相关配置 
//自动注入筛选器 符合该筛选器的类将会自动注入依赖的类 从字段/属性上
containerBuilder.UseAutofacastle(automaticInjectionSelectors:new List<OpenDeepSpace.Autofacastle.DependencyInjection.AutomaticInjectionSelector>() { 
  
    //实现了ITransientService类中的 所有字段属性自动注入
    new OpenDeepSpace.Autofacastle.DependencyInjection.AutomaticInjectionSelector(t=>typeof(ITransientService).IsAssignableFrom(t))

},nonInterceptSelectors:new List<OpenDeepSpace.Autofacastle.AspectAttention.NonInterceptSelector>() { 

    //实现了ITransientService的类 都不被AOP所拦截
    new OpenDeepSpace.Autofacastle.AspectAttention.NonInterceptSelector(t=>typeof(ITransientService).IsAssignableFrom(t) && t!=typeof(TransientUpgradeService))

}, IsConfigureIntercept:true);

//外部注入的服务并添加拦截 注意需要在UseAutofacastle使用后AddIntercept
containerBuilder.RegisterType<ExternalService>().AsSelf().InstancePerDependency().AddIntercept(typeof(ExternalService));


//在不能使用特性注入的类中 需要获取容器中的类需要使用IocManager并初始化
IocManager.InitContainer(containerBuilder.Build());

//测试单接口多实现顺序
ITransientService transientServiceMore = IocManager.Resolve<ITransientService>();

transientServiceMore.BusinessOne();


//测试自动注入 以及 通过特性注入实例
ITransientService transientService= IocManager.Resolve<ITransientService>(ImplementationType: typeof(TransientService));

transientService.BusinessOne();

transientService= IocManager.Resolve<ITransientService>(ImplementationType: typeof(TransientUpgradeService));

transientService.BusinessTwo();

//测试Aop横切关注
//外部注入的类的切面拦截ExternalService
var externalService = IocManager.Resolve<ExternalService>();
externalService.BusinessOne();