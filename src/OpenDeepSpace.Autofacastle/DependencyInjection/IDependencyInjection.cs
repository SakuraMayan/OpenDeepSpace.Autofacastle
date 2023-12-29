using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Autofacastle.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDependencyInjection
    {
        /// <summary>
        /// 注册作为某类型的服务
        /// </summary>
        Type[] AsServices { get; set; }


        /// <summary>
        /// 替换服务
        /// 比如 ServiceA 实现了 IServiceA
        /// 现在有一个ServiceAUp 实现了 IServiceA 并且我们想用ServiceAUp的实现替换掉之前的ServiceA的实现
        /// 这时候可以把ReplaceServices=new []{typeof(IServiceA)}接口
        /// </summary>
        Type[] ReplaceServices { get; set; }

        ///考虑自定义Keyed Named
        /// <summary>
        /// 自定义Keyed
        /// </summary>
        object Keyed { get; set; }

        /// <summary>
        /// 自定义Named
        /// </summary>
        string Named { get; set; }

        /// <summary>
        /// 排序 对于相同类型的依据序号优先加载 序号越小越先加载 即序号大的将覆盖序号小的
        /// </summary>
        int ImplementServiceOrder { get; set; }
    }
}
