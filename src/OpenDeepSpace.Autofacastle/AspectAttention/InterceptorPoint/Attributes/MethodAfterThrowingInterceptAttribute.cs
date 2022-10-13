using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Autofacastle.AspectAttention.InterceptorPoint.Attributes
{
    /// <summary>
    /// 方法执行后抛出异常的拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodAfterThrowingInterceptAttribute : Attribute
    {
       
        /// <summary>
        /// 指定拦截的错误类型
        /// </summary>
        public Type ExceptionType { get; set; }

        /// <summary>
        /// 抛出返回异常参数
        /// </summary>
        public string Throwing { get; set; }
    }
}
