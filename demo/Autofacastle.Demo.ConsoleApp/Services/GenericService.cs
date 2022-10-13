using OpenDeepSpace.Autofacastle.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{
    /// <summary>
    /// 瞬时注入泛型服务
    /// </summary>
    public class GenericService : IGenericService<GenericEntity>,ITransient
    {
        public async Task<GenericEntity> Get()
        {
            GenericEntity entity = new GenericEntity();
            entity.Name = "泛型实体";

            Console.WriteLine($"{nameof(GenericService)}.{nameof(Get)}");

            return await Task.FromResult(entity);
        }
    }

    /// <summary>
    /// 泛型实体
    /// </summary>
    public class GenericEntity
    { 
        public string Name { get; set; }
    }
}
