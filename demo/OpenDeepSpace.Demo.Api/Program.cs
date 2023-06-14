using Autofac;
using Microsoft.AspNetCore.Mvc;
using OpenDeepSpace.Demo.Api.Filters;
using OpenDeepSpace.Demo.Services.BasicAttributeBatchInjection;
using OpenDeepSpace.Demo.Services.BasicInterfaceBatchInjection;
using OpenDeepSpace.Autofacastle;
using OpenDeepSpace.Autofacastle.DependencyInjection;
using OpenDeepSpace.Autofacastle.Extensions;
using OpenDeepSpace.Autofacastle.Reflection;
using OpenDeepSpace.NetCore.Autofacastle.Extensions;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddControllersAsServices();//AddControllersAsServices()��Controller��Ϊ���� ʹ�øþ仰������Controller��ʹ�����Եķ�ʽ�������ע�����
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Host.UseAutofacastle(automaticInjectionSelectors:new List<OpenDeepSpace.Autofacastle.DependencyInjection.AutomaticInjectionSelector>() { 

    new OpenDeepSpace.Autofacastle.DependencyInjection.AutomaticInjectionSelector(t=>t.BaseType==typeof(ControllerBase))
        
});//ʹ��Autofacastle*/

/*builder.Host.UseAutofacastle(classInterceptSelectors: new List<OpenDeepSpace.Autofacastle.AspectAttention.ClassInterceptSelector>()
{
    new OpenDeepSpace.Autofacastle.AspectAttention.ClassInterceptSelector(t=>t.GetInterfaces().Any(t=>t==typeof(ITransientServiceA)))
});*/

/*builder.Host.UseAutofacastle(automaticInjectionSelectors: new List<OpenDeepSpace.Autofacastle.DependencyInjection.AutomaticInjectionSelector>() {

    new OpenDeepSpace.Autofacastle.DependencyInjection.AutomaticInjectionSelector(t=>t.BaseType==typeof(ITransientServiceB))

});*/

builder.Host.UseAutofacastle();
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{

    //�ⲿ�ֶ�ע�����ʵ������������
    container.RegisterType(typeof(ExternalService)).AddIntercept(typeof(ExternalService));

});

//
/*builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{

    //�Լ��������
    var assemblies = AssemblyFinder.GetAllAssemblies().Where(assembly => !assembly.FullName.StartsWith("Microsoft") && !assembly.FullName.StartsWith("System"));
    container.UseAutofacastle(assemblies: assemblies.ToList(), IsConfigureIntercept: true);
    //�ⲿ�ֶ�ע�����ʵ������������
    container.RegisterType(typeof(ExternalService)).AddIntercept(typeof(ExternalService));

}).UseServiceProviderFactory(new AutofacastleServiceProviderFactory());*/

builder.Services.AddMvcCore(op => {

    //op.Filters.Add<IocManagerFilter>();
});

//δʹ�����Ի�ӿ�ע����࣬�Լ��ֶ�ע�������Ҫʹ�����ؿ��Բ������·�ʽ

var app = builder.Build();

//(app.Services as AutofacServiceProvider).LifetimeScope
//app.Services.GetAutofacRoot()
//ʹ��IocManager
IocManager.InitContainer(app.Services.GetAutofacRoot());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthorization();

app.MapControllers();

app.Run();