/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/12/30 11:24:36	
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

namespace OpenDeepSpace.Autofacastle.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReplaceServices
    {
        /// <summary>
        /// 替换服务
        /// 比如 ServiceA 实现了 IServiceA
        /// 现在有一个ServiceAUp 实现了 IServiceA 并且我们想用ServiceAUp的实现替换掉之前的ServiceA的实现
        /// 这时候可以把ReplaceServices=new []{typeof(IServiceA)}接口
        /// </summary>
        Type[] ReplaceServices { get; set; }
    }
}
