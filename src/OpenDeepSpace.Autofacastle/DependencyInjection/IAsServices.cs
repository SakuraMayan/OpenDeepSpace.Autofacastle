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
    public interface IAsServices
    {
        /// <summary>
        /// 注册作为某类型的服务
        /// </summary>
        Type[] AsServices { get; set; }


       

      
    }
}
