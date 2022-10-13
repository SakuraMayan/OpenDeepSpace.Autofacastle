using OpenDeepSpace.Autofacastle.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{

    public class SingletonService : ISingletonService, ISingleton
    {
        public void BusinessOne()
        {
            Console.WriteLine($"{nameof(SingletonService)}.{nameof(BusinessOne)}");
        }
    }
}
