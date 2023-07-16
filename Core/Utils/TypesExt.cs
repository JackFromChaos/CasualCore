using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class TypesExt 
{
    private static List<Type> allTypes;
    static Dictionary<Type, List<Type>> derivedClasses = new Dictionary<Type, List<Type>>();
    static Dictionary<Type, string> longNames = new Dictionary<Type, string>();

    public static List<Type> AllTypes
    {
        get
        {
            if (allTypes == null)
            {
                allTypes = new List<Type>();
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes();
                    allTypes.AddRange(types);
                }
            }
            return allTypes;
        }
    }

    public static List<Type> GetDerivedClasses<T>()
    {
        return GetDerivedClasses(typeof(T));
    }

    public static List<Type> GetDerivedClasses(Type baseType)
    {
        if (derivedClasses == null)
        {
            derivedClasses = new Dictionary<Type, List<Type>>();
        }
        List<Type> ret;
        if (derivedClasses.TryGetValue(baseType, out ret))
        {
            return ret;
        }


        var types = AllTypes;
        List<Type> derived = new List<Type>();
        derivedClasses[baseType] = derived;
        foreach (Type type in types)
        {
            if (baseType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
            {
                if (!type.IsGenericType)
                {
                    derived.Add(type);
                }

            }



        }
        return derived;
    }



    public static string GetTypeName(this object value)
    {
        if (value == null) return "null";
        return value.GetType().Name;
    }

    

    private static string getClassName(this Type type)
    {
        var args = type.GenericTypeArguments;
        if (type.IsGenericType && args != null && args.Length > 0)
        {

            StringBuilder b = new StringBuilder();
            b.Append('<');
            b.Append(args[0].getClassName());
            for (int i = 1; i < args.Length; i++)
            {
                b.Append($",{args[i].getClassName()}");
            }
            b.Append('>');
            string typeName = type.Name;
            typeName = typeName.Replace("`" + args.Length, b.ToString());
            return typeName;
        }
        else
        {
            return type.Name;
        }
    }

    public static string GetClassName(this Type type)
    {
        string ret;
        if (!longNames.TryGetValue(type, out ret))
        {
            ret = type.getClassName();
            longNames[type] = ret;
        }
        return ret;

    }

}
