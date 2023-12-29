/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/12/28 15:57:32	
CLR: 4.0.30319.42000	
Description:


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

using OpenDeepSpace.Autofacastle.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Autofacastle.Extensions
{
    /// <summary>
    /// 实现服务顺序的拓展 用于单服务 多实现
    /// 提供获取指定顺序号的服务
    /// </summary>
    public static class ImplementServiceOrderExtensions
    {

        /// <summary>
        /// 获取最小顺序号的那个服务实现
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <returns></returns>
        public static TService GetMinOrderServiceImplementation<TService>(this IEnumerable<TService> services) where TService : IImplementServiceOrder
        {

            return services.OrderBy(t => t.ImplementServiceOrder).First();

        }

        /// <summary>
        /// 获取最大顺序号的那个服务实现
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <returns></returns>
        public static TService GetMaxOrderServiceImplementation<TService>(this IEnumerable<TService> services) where TService : IImplementServiceOrder
        {

            return services.OrderByDescending(t => t.ImplementServiceOrder).First();

        }

        /// <summary>
        /// 获取指定顺序号的那个服务实现
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="implementServiceOrder">实现服务的顺序号</param>
        /// <returns></returns>
        public static TService GetAppointOrderServiceImplementation<TService>(this IEnumerable<TService> services, int implementServiceOrder) where TService : IImplementServiceOrder
        {

            return services.First(t => t.ImplementServiceOrder == implementServiceOrder);

        }

    }
}
