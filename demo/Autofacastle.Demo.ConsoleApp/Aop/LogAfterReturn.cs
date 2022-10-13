using OpenDeepSpace.Autofacastle.AspectAttention;
using OpenDeepSpace.Autofacastle.AspectAttention.Interceptor.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Aop
{
    public class LogAfterReturn : MethodAfterReturnAbstractInterceptAttribute
    {
        public override async Task AfterReturn(InterceptContext interceptContext, object result)
        {
            Console.WriteLine($"方法正常执行后:{interceptContext.TargetMethod},{result}");

            await Task.CompletedTask;
        }
    }
}
