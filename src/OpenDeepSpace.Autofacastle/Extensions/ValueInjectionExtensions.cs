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


        /// <summary>
        /// 解析值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static object ResolveValue(this ResolveRequestContext context, MemberInfo memberInfo)
        {
            return ResolveValue(context, null, memberInfo, null);
        }

        /// <summary>
        /// 解析值
        /// </summary>
        /// <param name="componentContext"></param>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        public static object ResolveValue(this IComponentContext componentContext, ParameterInfo parameterInfo)
        {
            return ResolveValue(null, componentContext, null, parameterInfo);
        }

        private static object ResolveValue(ResolveRequestContext context, IComponentContext componentContext, MemberInfo memberInfo, ParameterInfo parameterInfo)
        {

            object value = null;

            //通过ioc解析 IValueInjection实现来完成值的注入 兼容性考虑
            object valueInjection = null;
            if (context != null)
                context.TryResolve(typeof(IValueInjection), out valueInjection);
            if (componentContext != null)
                componentContext.TryResolve(typeof(IValueInjection), out valueInjection);

            if (valueInjection != null)//存在实现
               value = (valueInjection as IValueInjection).ResolveValue(context, componentContext, memberInfo, parameterInfo);

            return value;
        }
    }
}
