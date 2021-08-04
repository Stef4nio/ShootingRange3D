using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that realizes a Service Locator pattern, helping others, to fill their dependencies
/// </summary>
public static class DependencyContainer
{
    private static readonly Dictionary<Type, object> _cachedDependencies = new Dictionary<Type, object>();

    /// <summary>
    /// Get a dependency of type T
    /// </summary>
    /// <typeparam name="T">Desired dependency type</typeparam>
    /// <returns>Reference to requested dependency</returns>
    public static T Get<T>() where T: class
    {
        _cachedDependencies.TryGetValue(typeof(T), out var value);

        return (T) value;
    }

    /// <summary>
    /// Allows to save a dependency to a container for later usage
    /// </summary>
    /// <param name="dependency">A dependency, you want to save</param>
    /// <typeparam name="T">Type of dependency to be saved</typeparam>
    public static void Set<T>(T dependency)
        => _cachedDependencies.Add(typeof(T), dependency);
}
