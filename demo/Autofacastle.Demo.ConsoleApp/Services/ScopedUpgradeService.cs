using OpenDeepSpace.Autofacastle.AspectAttention.Attributes;
using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{
    [Scoped]
    [AutomaticInjection]//类上使用自动注入特性
    [NonIntercept]//设置不被拦截特性
    public class ScopedUpgradeService : IScopedService
    {
        [AutomaticInjection(ImplementationType = typeof(TransientUpgradeService))]
        private ITransientService _service;//使用ImplementationType指定实现类 由于单接口多实现

        [AutomaticInjection(Keyed =typeof(TransientUpgradeService))]
        private ITransientService transientService;

        [AutomaticInjection(Named = nameof(TransientUpgradeService))]
        private ITransientService namedTransientService;

        private ExternalService _externalService;

        public async Task BusinessOne()
        {
            Console.WriteLine($"{nameof(ScopedService)}.{nameof(BusinessOne)}");
            await Task.CompletedTask;
        }
    }
}
