using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectionUtilities
{
    public static IEnumerable<PropertyInfo> Properties(this object x)
    {
        return x.GetType().GetProperties();
    }

    public static IEnumerable<PropertyInfo> Properties(this object x, Func<PropertyInfo, bool> predicate)
    {
        return x.GetType().GetProperties().Where(predicate);
    }
    public static PropertyInfo Property(this object x, object name)
    {
        return x.GetType().GetProperty(name.ToString());
    }

    public static PropertyInfo Property(this object x, Func<PropertyInfo, bool> predicate)
    {
        return x.GetType().GetProperties().SingleOrDefault(predicate);
    }

    public static MethodInfo Method(this object x, object name)
    {
        return x.GetType().GetMethod(name.ToString());
    }

    public static MethodInfo Method(this object x, Func<MethodInfo, bool> predicate)
    {
        return x.Methods(predicate).SingleOrDefault();
    }

    public static object PropertyValue(this object x, object name)
    {
        return x.GetType().GetProperty(name.ToString()).GetValue(x);
    }

    public static object PropertyValue(this object x, Func<PropertyInfo, bool> predicate)
    {
        PropertyInfo prop = x.GetType().GetProperties().SingleOrDefault(predicate);
        if(prop == null) return null;
        return prop.GetValue(x);
    }

    public static object MethodValue(this object x, object name)
    {
        return MethodValue(x, name, null);
    }

    public static object MethodValue(this object x, object name, object[] arguments)
    {
        return x.GetType().GetMethod(name.ToString()).Invoke(x, arguments);
    }

    public static object MethodValue(this object x, Func<MethodInfo, bool> predicate)
    {
        return MethodValue(x, predicate, null);
    }

    public static object MethodValue(this object x, Func<MethodInfo, bool> predicate, object[] arguments)
    {
        MethodInfo meth = x.GetType().GetMethods().SingleOrDefault(predicate);
        if(meth == null) return null;
        return meth.Invoke(x, arguments);
    }

    public static IEnumerable<MethodInfo> Methods(this object x)
    {
        return x.GetType().GetMethods();
    }

    public static IEnumerable<MethodInfo> Methods(this object x, Func<MethodInfo, bool> predicate)
    {
        return x.GetType().GetMethods().Where(predicate);
    }
}
