/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/6/13 18:14:48	
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
using Autofac.Core.Resolving.Pipeline;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenDeepSpace.Autofacastle
{
    /// <summary>
    /// 为了兼容netcore中值注入 通过采用接口的方式 向外暴露 
    /// 实现类采用 单例启动注入的方式
    /// </summary>
    public interface IValueInjection
    {
        object ResolveValue(ResolveRequestContext context, IComponentContext componentContext, MemberInfo memberInfo, ParameterInfo parameterInfo);

    }
}
