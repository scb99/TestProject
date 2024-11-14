﻿using System.Reflection;

namespace DBExplorerBlazor3TestProject;

public static class HelperMethods
{
    public static async Task InvokeAsync(this Type t, string methodNmae, object obj, params object[] parameters)
    {
        var temp = t.GetMethod(methodNmae, BindingFlags.NonPublic | BindingFlags.Instance)!;
        await temp.InvokeAsync(obj, parameters);
    }

    public static MethodInfo GetMethodInfo(this Type t, string methodName)
        => t.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)!;

    public static T GetPrivatePropertyValue<T>(this object obj, string propertyName)
    {
        return (T)(obj.GetType()
                      .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)?
                      .GetValue(obj) ?? default(T)!);
    }

    public static void SetPrivatePropertyValue<T>(this object obj, string propertyName, T value)
    {
        obj.GetType()
           .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)?
           .SetValue(obj, value);
    }

    public static T GetPublicPropertyValue<T>(this object obj, string propertyName)
    {
        return (T)(obj.GetType()
                      .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)?
                      .GetValue(obj) ?? default(T)!);
    }

    public static void SetPublicPropertyValue<T>(this object obj, string propertyName, T value)
    {
        obj.GetType()
           .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)?
           .SetValue(obj, value);
    }

    //public static object Call(this object o, string methodName, params object[] args)
    //{
    //    var mi = o.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

    //    if (mi != null)
    //    {
    //        return mi.Invoke(o, args);
    //    }

    //    return null;
    //}

    //public static async Task<T> InvokeAsync<T>(this MethodInfo methodInfo, object obj, params object[] parameters)
    //{
    //    dynamic awaitable = methodInfo.Invoke(obj, parameters);
    //    await awaitable;
    //    return (T)awaitable.GetAwaiter().GetResult();
    //}

    public static async Task InvokeAsync(this MethodInfo methodInfo, object obj, params object[] parameters)
    {
        dynamic awaitable = methodInfo.Invoke(obj, parameters)!;
        await awaitable;
    }


    //public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
    //{
    //    var task = (Task)@this.Invoke(obj, parameters);
    //    await task.ConfigureAwait(false);
    //    var resultProperty = task.GetType().GetProperty("Result");
    //    return resultProperty.GetValue(task);
    //}
}