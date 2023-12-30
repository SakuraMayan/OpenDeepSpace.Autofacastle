/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/12/30 11:14:38	
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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDeepSpace.Autofacastle.DependencyInjection
{
    /// <summary>
    /// 服务的实现
    /// </summary>
    public class ImplementationDescription : IImplementServiceOrder
    {

        public ServiceLifetime ServiceLifetime { get; set; }
        public Type ImplementationType { get; set; }
        public int ImplementServiceOrder { get; set; }


    }
}
