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
    /// 
    /// </summary>
    public interface IImplementServiceOrder
    {

        /// <summary>
        /// 实现服务的顺序
        /// </summary>
        int ImplementServiceOrder { get; set; }

    }
}
