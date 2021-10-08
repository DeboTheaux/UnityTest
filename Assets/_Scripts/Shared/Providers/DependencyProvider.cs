using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public static class DependencyProvider
{
    private static readonly Dictionary<Type, object> dependencies = new Dictionary<Type, object>();

    public static void RegisterDependency<T>(T dependency)
    {
        var type = typeof(T);
        Assert.IsFalse(dependencies.ContainsKey(type), $"Dependency {type} is already registered");
        dependencies.Add(type, dependency);
    }

    public static T GetDependency<T>()
    {
        var type = typeof(T);
        if (!dependencies.TryGetValue(type, out var dependency))
            throw new Exception($"Dependency {type} not found");
        return (T)dependency;
    }
}
