/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/12/30 11:02:30	
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
    /// KeyedNamed约束
    /// </summary>
    public interface IKeyedNamed
    {
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
