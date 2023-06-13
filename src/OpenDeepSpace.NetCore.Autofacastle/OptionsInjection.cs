/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/6/13 18:34:37	
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

using Microsoft.Extensions.Options;
using OpenDeepSpace.Autofacastle;
using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenDeepSpace.NetCore.Autofacastle
{
    /// <summary>
    /// 
    /// </summary>
    [Singleton(AutoActivate = true)]
    public class OptionsInjection : IOptionsInjection
    {
        public object ResolveOptions(object serviceInstance,Type serviceType, Type implementationType)
        {
            //解析出的实例不为空
            if (serviceInstance != null && implementationType != null)
            {
                //对选项的处理IOptions<xxx>
                //implementationType是IOptions<> 并且 serviceType不是IOptions<>
                if (implementationType.IsGenericType && implementationType.GetGenericTypeDefinition() == typeof(IOptions<>) && (!serviceType.IsGenericType || (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() != typeof(IOptions<>))))
                {
                    //获取真实的值
                    //获取返回值中的属性某个属性信息
                    PropertyInfo? propertyInfo = serviceInstance.GetType().GetProperty("Value");
                    //获取出IOptions<xxxOption>的value值 即xxxOption
                    serviceInstance = propertyInfo?.GetValue(serviceInstance);
                }
            }

            return serviceInstance;
        }
    }
}
