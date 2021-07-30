using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DependencyContainer
{
    private static readonly Dictionary<Type, object> _cachedDependencies = new Dictionary<Type, object>();

    public static T Get<T>() where T: class
    {
        _cachedDependencies.TryGetValue(typeof(T), out var value);

        return (T) value;
    }

    public static void Set<T>(T dependency)
        => _cachedDependencies.Add(typeof(T), dependency);
}
