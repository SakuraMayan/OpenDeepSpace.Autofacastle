using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{
    /// <summary>
    /// 可以指定Keyed Named
    /// </summary>
    [Transient(AsServices = new[] { typeof(ITransientService) }, Keyed = typeof(TransientUpgradeService), Named = nameof(TransientUpgradeService))]
    public class TransientUpgradeService : ITransientService, IDisposable
    {

        public void BusinessOne()
        {
            Console.WriteLine($"{nameof(TransientUpgradeService)}.{nameof(BusinessOne)}");
        }

        public async Task BusinessThree()
        {
            Console.WriteLine($"{nameof(TransientUpgradeService)}.{nameof(BusinessThree)}");
            await Task.CompletedTask;
        }

        public async Task<bool> BusinessTwo()
        {
            Console.WriteLine($"{nameof(TransientUpgradeService)}.{nameof(BusinessTwo)}");
            return await Task.FromResult(true);
        }

        public void Dispose()
        {
            
        }
    }
}
