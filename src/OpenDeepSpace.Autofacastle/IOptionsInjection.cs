/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/6/13 18:31:41	
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

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDeepSpace.Autofacastle
{
    /// <summary>
    /// IOptions注入拓展 为了netcore的兼容性
    /// </summary>
    public interface IOptionsInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceInstance">已经解析出的IOptions<>泛型实例</param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        object ResolveOptions(object serviceInstance,Type serviceType, Type implementationType);
    }
}
