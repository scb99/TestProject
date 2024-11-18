using System.Reflection;

namespace ExtensionMethods;

public static class HelperMethods
{
    public static async Task InvokeAsync(this Type t, string methodName, object obj, params object[] parameters) 
        => await t.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)!.InvokeAsync(obj, parameters);

    public static async Task InvokeAsync(this MethodInfo methodInfo, object obj, params object[] parameters)
    {
        dynamic awaitable = methodInfo.Invoke(obj, parameters)!;
        await awaitable;
    }

    public static void Invoke(this Type t, string methodName, object obj, params object[] paramaters) 
        => t.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(obj, paramaters);

    public static Delegate GetDelegate(this Type t, string methodName, object obj)
        => t.GetMethodInfo(methodName)!.CreateDelegate(typeof(Action), obj);

    public static MethodInfo GetMethodInfo(this Type t, string methodName)
        => t.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)!;

    public static T GetPublicPropertyValue<T>(this object obj, string propertyName) 
        => (T)(obj.GetType()
                  .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)?
                  .GetValue(obj) ?? default(T)!);

    public static T GetPrivatePropertyValue<T>(this object obj, string propertyName) 
        => (T)(obj.GetType()
                  .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)?
                  .GetValue(obj) ?? default(T)!);

    public static T GetPrivateMemberValue<T>(this object obj, string memberName) 
        => (T)(obj.GetType()
                  .GetField(memberName, BindingFlags.Instance | BindingFlags.NonPublic)?
                  .GetValue(obj) ?? default(T)!);

    public static T GetPrivateDictionaryValue<T>(this object obj, string propertyName, string key) 
        => (T)(obj.GetType()
                  .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)?
                  .GetValue(obj)?
                  .GetType()
                  .GetProperty("Item")?
                  .GetValue(obj, new object[] { key }) ?? default(T)!);

    public static void SetPublicPropertyValue<T>(this object obj, string propertyName, T value) 
        => obj.GetType()
              .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)?
           .   SetValue(obj, value);

    public static void SetPrivatePropertyValue<T>(this object obj, string propertyName, T value) 
        => obj.GetType()
              .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)?
              .SetValue(obj, value);

    public static void SetPrivateMemberValue<T>(this object obj, string memberName, T value) 
        => obj.GetType()
              .GetField(memberName, BindingFlags.Instance | BindingFlags.NonPublic)?
              .SetValue(obj, value);
}