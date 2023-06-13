/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/6/13 18:18:41	
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

using Autofac;
using OpenDeepSpace.Autofacastle;
using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using OpenDeepSpace.NetCore.Autofacastle.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenDeepSpace.NetCore.Autofacastle
{
    /// <summary>
    /// 值注入采用单例启动加载的方式
    /// </summary>
    [Singleton(AutoActivate =true)]
    public class ValueInjection : IValueInjection
    {
        public object ResolveValue(IComponentContext componentContext, ParameterInfo parameterInfo)
        {
            return componentContext.ResolveValue(parameterInfo);
        }
    }
}
