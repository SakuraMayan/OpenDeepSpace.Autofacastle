﻿/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/12/30 11:13:03	
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
    /// 用于Sington启动自动加载
    /// </summary>
    public interface IAutoActivate
    {
        /// <summary>
        /// 自动加载预加载
        /// </summary>
        bool AutoActivate { get; set; }
    }
}
