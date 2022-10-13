using OpenDeepSpace.Autofacastle.DependencyInjection;
using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{
    /// <summary>
    /// TransientService服务
    /// </summary>
    public class TransientService : ITransientService, ITransient
    {
        /// <summary>
        /// 由于设置了拦截筛选器将会自动注入无需使用AutomaticInjection特性
        /// </summary>
        public IGenericService<GenericEntity> genericService { get; set; }

        /// <summary>
        /// 由于设置了自动注入特性 这个也会当作注入 为了不注入需要使用NonAutomaticInjection
        /// </summary>
        [NonAutomaticInjection]
        public int i;

        public void BusinessOne()
        {
            genericService.Get();//调用GenericService

            Console.WriteLine($"{nameof(TransientService)}.{nameof(BusinessOne)}");
        }

        public async Task BusinessThree()
        {
            Console.WriteLine($"{nameof(TransientService)}.{nameof(BusinessThree)}");
            await Task.CompletedTask;
        }

        public async Task<bool> BusinessTwo()
        {
            Console.WriteLine($"{nameof(TransientService)}.{nameof(BusinessTwo)}");
            return await Task.FromResult(true);
        }
    }
}
