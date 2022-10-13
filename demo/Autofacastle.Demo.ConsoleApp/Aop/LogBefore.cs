

using OpenDeepSpace.Autofacastle.AspectAttention;
using OpenDeepSpace.Autofacastle.AspectAttention.Interceptor.Attributes;

namespace Autofacastle.Demo.ConsoleApp.Aop
{
    /// <summary>
    /// 方法调用前记录日志
    /// </summary>
    public class LogBefore : MethodBeforeAbstractInterceptAttribute
    {
        public override Task Before(InterceptContext interceptorContext)
        {
            Console.WriteLine($"方法前执行{interceptorContext.TargetMethod}");
            return Task.CompletedTask;
        }
    }
}
