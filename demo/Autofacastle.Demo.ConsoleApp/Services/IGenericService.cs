using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{
    /// <summary>
    /// 泛型服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericService<T> where T : class
    {
        public Task<T> Get();
    }
}
