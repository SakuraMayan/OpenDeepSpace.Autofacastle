using Autofacastle.Demo.ConsoleApp.Aop;
using OpenDeepSpace.Autofacastle.AspectAttention.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Services
{
    /// <summary>
    /// 表示外部的一个服务
    /// </summary>
    [ClassIntercept]//标注为类拦截
    public class ExternalService : IDisposable
    {

        /// <summary>
        /// 虚方法将会被拦截
        /// </summary>
        [LogBefore]
        [Log2Before(GroupName ="第二个日志记录")]
        public virtual void BusinessOne()
        {
            Console.WriteLine($"{nameof(ExternalService)}.{nameof(BusinessOne)}");
        }


        public void Dispose()
        {
            
        }
    }
}
