﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Autofacastle.DependencyInjection.Attributes
{
    /// <summary>
    /// 依赖注入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyInjectionAttribute : Attribute, IAsServices,IReplaceServices,IKeyedNamed,IImplementServiceOrder
    {
        public Type[] AsServices { get ; set ; }
        public Type[] ReplaceServices { get ; set ; }
        public object Keyed { get ; set ; }
        public string Named { get; set; }
        public int ImplementServiceOrder { get ; set ; }
    }
}
