using Sinch.MessageEncoder.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sinch.MessageEncoder.Extensions;

internal static class AppDomainExtensions
{
    private static List<Type> _types = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .ToList();

    public static IEnumerable<Type> GetSubclassesOf<TType>(this AppDomain @this) where TType : class
    {
        return _types.Where(type => type.IsSubclassOf(typeof(TType)) && type.IsAbstract is false);
    }

    public static TCollection GetSubclassesOf<TType, TCollection>(this AppDomain @this, Func<IEnumerable<Type>, TCollection> factory)
        where TType : class
        where TCollection : class, ICollection
    {
        var types = @this.GetSubclassesOf<TType>();
        var result = factory(types);
        return result;
    }

    public static Dictionary<Type, Type> GetSubclassesOfOpenGeneric(this AppDomain @this, Type openGenericType)
    {
        var _related = _types.Select(type => new { type, baseType = type.BaseType })
            .Where(pair =>
            {
                return !pair.type.IsAbstract && !pair.type.IsInterface &&
                       pair.baseType is { IsGenericType: true } &&
                       pair.baseType.GetGenericTypeDefinition() == openGenericType;
            })
            .Select(pair => pair.type)
            .ToDictionary(x => typeof(Message<,>).MakeGenericType(x.BaseType.GenericTypeArguments[0], x.BaseType.GenericTypeArguments[1]), t => t);

        return _related;
    }
}