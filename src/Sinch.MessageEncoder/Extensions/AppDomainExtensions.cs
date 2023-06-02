using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Extensions;

internal static class AppDomainExtensions
{
    public static IEnumerable<Type> GetSubclassesOf<TType>(this AppDomain @this) where TType : class
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(TType)) && type.IsAbstract is false);
    }

    public static TCollection GetSubclassesOf<TType, TCollection>(this AppDomain @this, Func<IEnumerable<Type>, TCollection> factory) 
        where TType : class
        where TCollection : class, ICollection
    {
        var types = @this.GetSubclassesOf<TType>();
        var result = factory(types);
        return result;
    }
}