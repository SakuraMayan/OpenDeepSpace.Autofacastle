/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/12/28 15:53:26	
CLR: 4.0.30319.42000	
Description:
    单个服务多实现通用的 实现顺序接口 在Microsoft.DependencyInjection中采用IEnumerable<>接口注入 

=====================================Copyright Create Declare End==================================

=====================================Modify Records Explain Start==================================

Modifier:	
ModificationTime:
Description:


===================================================================================================

Modifier:
ModificationTime:
Description:


=====================================Modify Records Explain End====================================

===================================================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Autofacastle.DependencyInjection
{
    /// <summary>
    /// 实现该接口可以指定实现服务的顺序
    /// 针对单服务 多实现 
    /// 可以采用IEnumerable<TService>注入 然后结合<see cref="Extensions.ImplementServiceOrderExtensions"/>使用具体顺序号实现类
    /// </summary>
    public interface IImplementServiceOrder
    {

        /// <summary>
        /// 实现顺序 
        /// 顺序号越小先注入 顺序号越大后注入 因此后注入会优先获取到 
        /// !!!!!!!!注意:仅限采用本包的批量注入方式 
        ///        <see cref="Extensions.ServiceCollectionExtensions.BatchInjection(Microsoft.Extensions.DependencyInjection.IServiceCollection, IEnumerable{System.Reflection.Assembly})"/>
        ///        <see cref="Extensions.ServiceCollectionExtensions.BatchInjection(Microsoft.Extensions.DependencyInjection.IServiceCollection, IEnumerable{Type})"/>
        ///         且采用基于特性的注入<see cref="Attributes.TransientAttribute"/>|<see cref="Attributes.ScopedAttribute"/>|<see cref="Attributes.SingletonAttribute"/>方式生效     
        ///     外部独立使用即不使用本包的批量注入不保证这一点(顺序号越小先注入 顺序号越大后注入 因此后注入会优先获取到)
        ///     独立使用还是遵循<see cref="Microsoft.Extensions.DependencyInjection"/>的规则
        /// </summary>
        int ImplementServiceOrder { get; set; }

    }
}
