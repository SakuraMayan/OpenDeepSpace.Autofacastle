using OpenDeepSpace.Autofacastle.AspectAttention;
using OpenDeepSpace.Autofacastle.AspectAttention.Interceptor.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autofacastle.Demo.ConsoleApp.Aop
{
    /// <summary>
    /// 方法执行出现异常的拦截
    /// </summary>
    public class LogThrow : MethodAfterThrowingAbstractInterceptAttribute
    {
        public override async Task AfterThrowing(InterceptContext interceptContext, Exception exception)
        {
            Console.WriteLine($"方法执行异常:{interceptContext.TargetMethod},异常:{exception.Message}");

            await Task.CompletedTask;
        }
    }
}
