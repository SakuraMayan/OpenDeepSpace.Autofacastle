using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{
    /// <summary>
    /// 程序启动时的一个自动初始化服务 预加载
    /// </summary>
    [Singleton(AutoActivate =true)]
    public class SingletonAutoActivedService : ISingletonService
    {

        public SingletonAutoActivedService()
        {
            Console.WriteLine($"{nameof(SingletonAutoActivedService)} AutoActived");
        }

        public void BusinessOne()
        {
            Console.WriteLine($"{nameof(SingletonAutoActivedService)}.{nameof(BusinessOne)}");
        }
    }
}
