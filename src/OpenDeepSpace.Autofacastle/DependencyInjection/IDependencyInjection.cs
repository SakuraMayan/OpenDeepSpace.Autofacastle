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



        ///考虑自定义Keyed Named
        /// <summary>
        /// 自定义Keyed
        /// </summary>
        object Keyed { get; set; }

        /// <summary>
        /// 自定义Named
        /// </summary>
        string Named { get; set; }
    }
}
