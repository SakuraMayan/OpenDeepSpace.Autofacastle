﻿using OpenDeepSpace.Autofacastle.AspectAttention.Interceptor.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Autofacastle.AspectAttention.Interceptor.Attributes
{
    /// <summary>
    /// 方法前拦截特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public abstract class MethodBeforeAbstractInterceptAttribute : MethodInterceptBaseAttribute
    {

        /// <summary>
        /// 方法执行前拦截
        /// </summary>
        /// <param name="interceptContext">拦截上下文</param>
        public abstract Task Before(InterceptContext interceptContext);
    }
}
