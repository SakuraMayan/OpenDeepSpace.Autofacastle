/*=================================================================================================

=====================================Copyright Create Declare Start================================

Copyright © 2023 by OpenDeepSpace. All rights reserved.
Author: OpenDeepSpace	
CreateTime: 2023/6/13 18:02:12	
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

using Autofac.Core.Resolving.Pipeline;
using Autofac;
using OpenDeepSpace.Autofacastle.DependencyInjection.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenDeepSpace.Autofacastle.Extensions
{
    /// <summary>
    /// 值注入拓展 是否需要注入值的判断拓展
    /// </summary>
    public static class ValueInjectionExtensions
    {
        /// <summary>
        /// 是否需要自动注入值
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static bool IsNeedValueInjection(this MemberInfo memberInfo)
        {
            return IsNeedValueInjection(memberInfo, null);
        }

        /// <summary>
        /// 是否需要值自动注入
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        public static bool IsNeedValueInjection(this ParameterInfo parameterInfo)
        {
            return IsNeedValueInjection(null, parameterInfo);
        }

       


        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        private static bool IsNeedValueInjection(MemberInfo memberInfo, ParameterInfo parameterInfo)
        {
            ValueInjectionAttribute valueAttr = null;
            if (memberInfo != null)
                valueAttr = memberInfo.GetCustomAttribute<ValueInjectionAttribute>();
            if (parameterInfo != null)
                valueAttr = parameterInfo.GetCustomAttribute<ValueInjectionAttribute>();
            if (valueAttr == null)//值特性为空
                return false;
            return true;
        }
    }
}
