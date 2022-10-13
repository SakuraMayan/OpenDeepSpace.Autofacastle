using OpenDeepSpace.Autofacastle.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ScopedService : IScopedService, IScoped
    {
        public async Task BusinessOne()
        {
            Console.WriteLine($"{nameof(ScopedService)}.{nameof(BusinessOne)}");
            await Task.CompletedTask;
        }
    }
}
