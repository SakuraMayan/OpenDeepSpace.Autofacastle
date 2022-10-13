using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDeepSpace.Autofacastle.AspectAttention
{
    /// <summary>
    /// 类拦截筛选器 满足筛选器条件的将执行类拦截
    /// </summary>
    public class ClassInterceptSelector : NamedTypeSelector
    {
        public ClassInterceptSelector(Func<Type, bool> predicate) : base(predicate)
        {
        }

        public ClassInterceptSelector(string name, Func<Type, bool> predicate) : base(name, predicate)
        {
        }
    }
}
