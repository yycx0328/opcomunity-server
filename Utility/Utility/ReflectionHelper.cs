// =========================================================
// Author	:   luyunhai
// Create Time  :   8/10/2015 11:05:00 PM
// =========================================================
// Copyright © USER-VFH583E7VU 2015 . All rights reserved.
// =========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.DynamicData;

/// <summary>
/// 反射辅助类
/// </summary>
public static class ReflectionHelper
{
    private static Dictionary<string, Func<object, object[], object>> FuncDictionary = new Dictionary<string, Func<object, object[], object>>();
    private static object[] EmptyObjects = new object[] { };
    //private static MethodInfo ConvertValueMethod = typeof(ReflectionHelper).GetMethod("ConvertValue", BindingFlags.Static | BindingFlags.NonPublic);
    private const string CONSTRUCTOR_NAME = ".ctor";

    #region Property(属性)
    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="property">The property.</param>
    /// <returns></returns>
    public static object GetProperty(object entity, PropertyInfo property)
    {
        //return property.GetValue(entity, null);
        if (entity == null || property == null)
            return null;

        string key = property.ReflectedType.FullName + "#" + property.Name + "-0-";
        Func<object, object[], object> func;
        if (!FuncDictionary.TryGetValue(key, out func))
        {
            //Func<object, object[], object> getAge = delegate(object entity, object args){ return ((User)entity).Age }
            if (property.CanRead)
            {
                ParameterExpression entityParameter = Expression.Parameter(typeof(object), "entity");
                ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
                Expression instanceExpr = Expression.Convert(entityParameter, property.ReflectedType);
                Expression propertyExpr = Expression.Property(instanceExpr, property);
                Expression body = Expression.Convert(propertyExpr, typeof(object));
                func = Expression.Lambda<Func<object, object[], object>>(body, entityParameter, argsParameter).Compile();
            }
            else
            {
                func = (object obj, object[] args) => null;
            }
            FuncDictionary[key] = func;
        }
        return func(entity, EmptyObjects);
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns></returns>
    public static object GetProperty(object entity, string propertyName)
    {
        if (entity == null || string.IsNullOrEmpty(propertyName))
            return null;

        Type entityType = entity.GetType();
        string key = entityType.FullName + "#" + propertyName + "-0-";
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(entity, EmptyObjects);
        else
            return GetProperty(entity, SearchPropertyInfo(entityType, propertyName));
    }

    /// <summary>
    /// 获取属性值，用于静态属性
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns></returns>
    public static object GetProperty(Type entityType, string propertyName)
    {
        if (entityType == null || string.IsNullOrEmpty(propertyName))
            return null;

        string key = entityType.FullName + "#" + propertyName + "-0-";
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(null, EmptyObjects);
        else
            return GetProperty(null, SearchPropertyInfo(entityType, propertyName));
    }

    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="property">The property.</param>
    /// <param name="value">The value.</param>
    public static void SetProperty(object entity, PropertyInfo property, object value)
    {
        //property.SetValue(entity, value, null);

        if (entity == null || property == null)
            return;

        string key = property.ReflectedType.FullName + "#" + property.Name + "-1-";
        Func<object, object[], object> func;
        if (!FuncDictionary.TryGetValue(key, out func))
        {
            if (property.CanWrite)
            {
                //Action<object, object[]> setAge = delegate(object entity, object[] args) { ((User)entity).Age = (int)ConvertValue(args[0], typeof(int)); };  
                ParameterExpression entityParameter = Expression.Parameter(typeof(object), "entity");
                ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
                Expression instanceExpr = Expression.Convert(entityParameter, property.ReflectedType);
                Expression iArgs = Expression.ArrayIndex(argsParameter, Expression.Constant(0));
                var instanceArgs = new Expression[1] { Expression.Convert(iArgs, property.PropertyType) };
                //Expression convertMethod = Expression.Call(ConvertValueMethod, new Expression[2] { iArgs, Expression.Constant(property.PropertyType) });
                //var instanceArgs = new Expression[1] { Expression.Convert(convertMethod, property.PropertyType) };
                MethodInfo setMethod = property.GetSetMethod();
                if (setMethod == null)
                    setMethod = property.GetSetMethod(true);
                Expression callMethod = Expression.Call(instanceExpr, setMethod, instanceArgs);
                Action<object, object[]> action = Expression.Lambda<Action<object, object[]>>(callMethod, entityParameter, argsParameter).Compile();
                func = (object obj, object[] args) => { action(obj, args); return null; };
            }
            else
            {
                func = (object obj, object[] args) => null;
            }
            FuncDictionary[key] = func;
            FuncDictionary[key + property.PropertyType.FullName] = func;
        }
        func(entity, new object[] { value });
    }



    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    public static void SetProperty(object entity, string propertyName, object value)
    {
        if (entity == null || string.IsNullOrEmpty(propertyName))
            return;

        Type entityType = entity.GetType();
        string key = entityType.FullName + "#" + propertyName + "-1-";
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            func(entity, new object[] { value });
        else
            SetProperty(entity, entityType.GetProperty(propertyName), value);
    }

    /// <summary>
    /// 设置属性值，用于静态属性
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    public static void SetProperty(Type entityType, string propertyName, object value)
    {
        if (entityType == null || string.IsNullOrEmpty(propertyName))
            return;

        string key = entityType.FullName + "#" + propertyName + "-1-";
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            func(null, new object[] { value });
        else
            SetProperty(null, entityType.GetProperty(propertyName), value);
    }
    #endregion

    #region CallMethod(调用方法)
    /// <summary>
    /// 调用方法
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="method">The method.</param>
    /// <param name="args">The args.</param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    public static object CallMethod(object entity, MethodInfo method, object[] args, Type[] genericTypes)
    {
        if (method == null)
            return null;
        if (args == null)
            args = EmptyObjects;
        //Fnction<object, object[], object> show = delegate(object entity, object[] args) { return ((User)entity).Show((string)ConvertValue(args, typeof(string))); };            

        Type[] argTypes = Type.GetTypeArray(args);

        string key = method.ReflectedType.FullName + "#" + method.Name + "-" + args.Length + "-" + string.Join("-", argTypes.Select(p => p.FullName).ToArray());
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(entity, args);

        //泛型方法，进行方法转换
        var parameterInfos = method.GetParameters();
        if (method.IsGenericMethodDefinition)
        {
            method = MakeGenericMethod(method, parameterInfos, argTypes, genericTypes);
            parameterInfos = method.GetParameters();
        }

        //return method.Invoke(entity, args);

        ParameterExpression entityParameter = Expression.Parameter(typeof(object), "entity");
        ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
        //参数            
        var instanceArgs = new Expression[args == null ? 0 : args.Length];
        for (int i = 0; i < instanceArgs.Length; i++)
        {
            var iArgs = Expression.ArrayIndex(argsParameter, Expression.Constant(i));
            instanceArgs[i] = Expression.Convert(iArgs, parameterInfos[i].ParameterType);
            //Expression convertMethod = Expression.Call(ConvertValueMethod, new Expression[2] { iArgs, Expression.Constant(parameterInfos[i].ParameterType) });
            //instanceArgs[i] = Expression.Convert(convertMethod, parameterInfos[i].ParameterType);
        }
        //是否静态方法
        Expression instanceExpr = null;
        if (!method.IsStatic)
            instanceExpr = Expression.Convert(entityParameter, method.ReflectedType);
        Expression callMethod = Expression.Call(instanceExpr, method, instanceArgs);
        //是否有返回值
        if (method.ReturnType == typeof(void))
        {
            Action<object, object[]> action = Expression.Lambda<Action<object, object[]>>(callMethod, entityParameter, argsParameter).Compile();
            func = (object obj, object[] gs) => { action(obj, gs); return null; };
        }
        else
        {
            func = Expression.Lambda<Func<object, object[], object>>(Expression.Convert(callMethod, typeof(object)), entityParameter, argsParameter).Compile();
        }
        FuncDictionary[key] = func;
        return func(entity, args);
    }

    /// <summary>
    /// 调用方法
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="method">The method.</param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object CallMethod(object entity, MethodInfo method, object[] args)
    {
        return CallMethod(entity, method, args, Type.EmptyTypes);
    }

    /// <summary>
    /// 调用方法
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="bindings"></param>
    /// <param name="args">The args.</param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    public static object CallMethod(object entity, string methodName, BindingFlags bindings, object[] args, Type[] genericTypes)
    {
        if (entity == null || string.IsNullOrEmpty(methodName))
            return null;

        Type entityType = entity.GetType();
        Type[] argTypes = Type.GetTypeArray(args);
        string key = entityType.FullName + "#" + methodName + "-" + bindings + "-" + args.Length + "-" + string.Join("-", argTypes.Select(p => p.FullName).ToArray());
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(entity, args);
        else
            return CallMethod(entity, FindBestMethod(entityType, methodName, bindings, argTypes, genericTypes), args, Type.EmptyTypes);
    }

    /// <summary>
    /// 调用方法
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="bindings"></param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object CallMethod(object entity, string methodName, BindingFlags bindings, object[] args)
    {
        return CallMethod(entity, methodName, bindings, args, Type.EmptyTypes);
    }

    /// <summary>
    /// 调用方法
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="args">The args.</param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    public static object CallMethod(object entity, string methodName, object[] args, Type[] genericTypes)
    {
        if (entity == null || string.IsNullOrEmpty(methodName))
            return null;

        Type entityType = entity.GetType();
        Type[] argTypes = Type.GetTypeArray(args);
        string key = entityType.FullName + "#" + methodName + "-" + args.Length + "-" + string.Join("-", argTypes.Select(p => p.FullName).ToArray());
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(entity, args);
        else
            return CallMethod(entity, FindBestMethod(entityType, methodName, argTypes, genericTypes), args, Type.EmptyTypes);
    }

    /// <summary>
    /// 调用方法
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object CallMethod(object entity, string methodName, object[] args)
    {
        return CallMethod(entity, methodName, args, Type.EmptyTypes);
    }

    /// <summary>
    /// 调用方法，用于静态方法或构造函数
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="bindings"></param>
    /// <param name="args">The args.</param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    public static object CallMethod(Type entityType, string methodName, BindingFlags bindings, object[] args, Type[] genericTypes)
    {
        if (entityType == null)
            return null;
        if (string.IsNullOrEmpty(methodName) || methodName == entityType.Name)
            methodName = CONSTRUCTOR_NAME;

        Type[] argTypes = Type.GetTypeArray(args);
        string key = entityType.FullName + "#" + methodName + "-" + bindings + "-" + args.Length + "-" + string.Join("-", argTypes.Select(p => p.FullName).ToArray());
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(null, args);
        else
            return CallMethod(null, FindBestMethod(entityType, methodName, bindings, argTypes, genericTypes), args, Type.EmptyTypes);
    }

    /// <summary>
    /// 调用方法，用于静态方法或构造函数
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="bindings"></param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object CallMethod(Type entityType, string methodName, BindingFlags bindings, object[] args)
    {
        return CallMethod(entityType, methodName, bindings, args, Type.EmptyTypes);
    }

    /// <summary>
    /// 调用方法，用于静态方法或构造函数
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="args">The args.</param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    public static object CallMethod(Type entityType, string methodName, object[] args, Type[] genericTypes)
    {
        if (entityType == null)
            return null;
        if (string.IsNullOrEmpty(methodName) || methodName == entityType.Name)
            methodName = CONSTRUCTOR_NAME;

        Type[] argTypes = Type.GetTypeArray(args);
        string key = entityType.FullName + "#" + methodName + "-" + args.Length + "-" + string.Join("-", argTypes.Select(p => p.FullName).ToArray());
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(null, args);
        else
            return CallMethod(null, FindBestMethod(entityType, methodName, argTypes, genericTypes), args, Type.EmptyTypes);
    }

    /// <summary>
    /// 调用方法，用于静态方法或构造函数
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="args">The args.</param>        
    /// <returns></returns>
    public static object CallMethod(Type entityType, string methodName, object[] args)
    {
        return CallMethod(entityType, methodName, args, Type.EmptyTypes);
    }
    #endregion

    #region Indexer(索引器)
    /// <summary>
    /// 获取索引器值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object GetIndexer(object entity, object[] args)
    {
        if (entity == null)
            return null;

        return CallMethod(entity, "get_Item", args, Type.EmptyTypes);
    }

    /// <summary>
    /// 设置索引器值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="value">The value.</param>
    /// <param name="args">The args.</param>
    public static void SetIndexer(object entity, object value, object[] args)
    {
        if (entity == null)
            return;

        object[] newArgs = new object[args.Length + 1];
        args.CopyTo(newArgs, 0);
        newArgs[newArgs.Length - 1] = value;
        CallMethod(entity, "set_Item", newArgs, Type.EmptyTypes);
    }
    #endregion

    #region Field(字段)
    /// <summary>
    /// 获取字段值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="field">The field.</param>
    /// <returns></returns>
    public static object GetField(object entity, FieldInfo field)
    {
        //return field.GetValue(entity);
        if (field == null)
            return null;

        string key = field.ReflectedType.FullName + "#" + field.Name + "-0-";
        Func<object, object[], object> func;
        if (!FuncDictionary.TryGetValue(key, out func))
        {
            //Func<object, object[], object> getAge = delegate(object entity, object args){ return ((User)entity).Age }
            ParameterExpression entityParameter = Expression.Parameter(typeof(object), "entity");
            ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
            Expression instanceExpr = Expression.Convert(entityParameter, field.ReflectedType);
            Expression fieldExpr = Expression.Field(instanceExpr, field);
            Expression body = Expression.Convert(fieldExpr, typeof(object));
            func = Expression.Lambda<Func<object, object[], object>>(body, entityParameter, argsParameter).Compile();
            FuncDictionary[key] = func;
        }
        return func(entity, EmptyObjects);
    }

    /// <summary>
    /// 获取字段值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns></returns>
    public static object GetField(object entity, string fieldName)
    {
        if (entity == null || string.IsNullOrEmpty(fieldName))
            return null;

        Type entityType = entity.GetType();
        string key = entityType.FullName + "#" + fieldName + "-0-";
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(null, EmptyObjects);
        else
            return GetField(entity, entityType.GetField(fieldName));
    }

    /// <summary>
    /// 获取字段值，用于静态字段
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns></returns>
    public static object GetField(Type entityType, string fieldName)
    {
        if (entityType == null || string.IsNullOrEmpty(fieldName))
            return null;

        string key = entityType.FullName + "#" + fieldName + "-0-";
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(null, EmptyObjects);
        else
            return GetField(null, entityType.GetField(fieldName));
    }

    /// <summary>
    /// 设置字段值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="field">The field.</param>
    /// <param name="value">The value.</param>
    public static void SetField(object entity, FieldInfo field, object value)
    {
        //field.SetValue(entity, value);
        if (field == null)
            return;

        string key = field.ReflectedType.FullName + "#" + field.Name + "-1-";
        Func<object, object[], object> func;
        if (!FuncDictionary.TryGetValue(key, out func))
        {
            //Action<object, object[]> setAge = delegate(object entity, object args) { ((User)entity).Age = (int)ConvertValue(args[0], typeof(int)); };  
            ParameterExpression entityParameter = Expression.Parameter(typeof(object), "entity");
            ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
            Expression instanceExpr = Expression.Convert(entityParameter, field.ReflectedType);
            var iArgs = Expression.ArrayIndex(argsParameter, Expression.Constant(0));
            var instanceArgs = new Expression[2] { instanceExpr, Expression.Convert(iArgs, field.FieldType) };
            //Expression convertMethod = Expression.Call(ConvertValueMethod, new Expression[2] { iArgs, Expression.Constant(field.FieldType) });
            //var instanceArgs = new Expression[2] { instanceExpr, Expression.Convert(convertMethod, field.FieldType) };
            Expression callMethod = Expression.Call(field.GetField_SetMI(), instanceArgs);
            Action<object, object[]> action = Expression.Lambda<Action<object, object[]>>(callMethod, entityParameter, argsParameter).Compile();
            func = (object obj, object[] args) => { action(obj, args); return null; };
            FuncDictionary[key] = func;
            FuncDictionary[key + field.FieldType.FullName] = func;
        }
        func(entity, new object[] { value });
    }

    /// <summary>
    /// 设置字段值
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static void SetField(object entity, string fieldName, object value)
    {
        if (entity == null || string.IsNullOrEmpty(fieldName))
            return;

        Type entityType = entity.GetType();
        string key = entityType.FullName + "#" + fieldName + "-1-";
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            func(null, new object[] { value });
        else
            SetField(entity, entityType.GetField(fieldName), value);
    }

    /// <summary>
    /// 设置字段值，用于静态字段
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static void SetField(Type entityType, string fieldName, object value)
    {
        if (entityType == null || string.IsNullOrEmpty(fieldName))
            return;

        string key = entityType.FullName + "#" + fieldName + "-1-";
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            func(null, new object[] { value });
        else
            SetField(null, entityType.GetField(fieldName), value);
    }

    /// <summary>
    /// 构造一个访问字段的方法
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    private static DynamicMethod GetField_SetMI(this FieldInfo field)
    {
        var type = field.DeclaringType;
        //DynamicMethod方便我们Emit IL 
        DynamicMethod dm = new DynamicMethod(string.Empty, typeof(void), new Type[] { type, field.FieldType }, type);
        //ILGenerator
        ILGenerator il = dm.GetILGenerator();
        //在计算栈上载入this 
        il.Emit(OpCodes.Ldarg_0);
        //在计算栈上载入要赋的值 
        il.Emit(OpCodes.Ldarg_1);
        //设置字段属性 
        il.Emit(OpCodes.Stfld, field);
        //方法结束 
        il.Emit(OpCodes.Ret);
        return dm;
    }
    #endregion

    #region InvokeMember(调用成员)
    /// <summary>
    /// 调用成员
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="member">The member.</param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object InvokeMember(object entity, MemberInfo member, params object[] args)
    {
        if (member == null)
            return null;
        if (args == null)
            args = new object[] { };

        switch (member.MemberType)
        {
            case MemberTypes.Property:
                var pi = member as PropertyInfo;
                var indexPatameters = pi.GetIndexParameters();
                if (indexPatameters.Length == 0)
                {
                    if (args.Length == 0)
                        return GetProperty(entity, pi);
                    SetProperty(entity, pi, args[0]);
                    return null;
                }
                else
                {
                    if (indexPatameters.Length == args.Length)
                        return GetIndexer(entity, args);
                    CallMethod(entity, "set_Item", args, Type.EmptyTypes);
                    return null;
                }
            case MemberTypes.Method:
                return CallMethod(entity, member as MethodInfo, args, Type.EmptyTypes);
            case MemberTypes.Constructor:
                return (member as ConstructorInfo).Invoke(args);
            case MemberTypes.Field:
                if (args.Length == 0)
                    return GetField(entity, member as FieldInfo);
                else
                {
                    SetField(entity, member as FieldInfo, args[0]);
                    return null;
                }
            case MemberTypes.Event:
                var ei = member as EventInfo;
                return CallMethod(entity, ei.GetRaiseMethod(), args, Type.EmptyTypes);
            default:
                return null;
        }
    }

    /// <summary>
    /// 调用成员
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="entityType"></param>
    /// <param name="memberName"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static object InvokeMember(object entity, Type entityType, string memberName, object[] args)
    {
        if (entity == null && entityType == null)
            return null;

        if (args == null)
            args = EmptyObjects;
        if (entityType == null)
            entityType = entity.GetType();
        if (string.IsNullOrEmpty(memberName) || memberName == entityType.Name)
            memberName = CONSTRUCTOR_NAME;

        Type[] argTypes = Type.GetTypeArray(args);
        string key = entityType.FullName + "#" + memberName + "-" + "-" + args.Length + "-" + string.Join("-", argTypes.Select(p => p.FullName).ToArray());
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(entity, args);

        MemberInfo[] memberList = entityType.GetMember(memberName);
        return InvokeBestMember(memberList, entity, memberName, args, argTypes);
    }

    /// <summary>
    /// 调用成员
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="entityType"></param>
    /// <param name="memberName"></param>
    /// <param name="bindings"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static object InvokeMember(object entity, Type entityType, string memberName, BindingFlags bindings, params object[] args)
    {
        if (entity == null && entityType == null)
            return null;

        if (args == null)
            args = EmptyObjects;
        if (entityType == null)
            entityType = entity.GetType();
        if (string.IsNullOrEmpty(memberName) || memberName == entityType.Name)
            memberName = CONSTRUCTOR_NAME;

        Type[] argTypes = Type.GetTypeArray(args);
        string key = entityType.FullName + "#" + memberName + "-" + bindings + "-" + args.Length + "-" + string.Join("-", argTypes.Select(p => p.FullName).ToArray());
        Func<object, object[], object> func;
        if (FuncDictionary.TryGetValue(key, out func))
            return func(entity, args);

        MemberInfo[] memberList = entityType.GetMember(memberName, bindings);
        return InvokeBestMember(memberList, entity, memberName, args, argTypes);
    }

    /// <summary>
    /// 调用成员
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="memberName">Name of the member.</param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object InvokeMember(object entity, string memberName, object[] args)
    {
        if (entity == null)
            return null;

        return InvokeMember(entity, entity.GetType(), memberName, args);
    }

    /// <summary>
    /// 调用成员
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="memberName">Name of the member.</param>        
    /// <returns></returns>
    public static object InvokeMember(object entity, string memberName)
    {
        if (entity == null)
            return null;

        return InvokeMember(entity, entity.GetType(), memberName, EmptyObjects);
    }

    /// <summary>
    /// 调用成员
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="memberName">Name of the member.</param>
    /// <param name="bindings"></param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object InvokeMember(object entity, string memberName, BindingFlags bindings, params object[] args)
    {
        if (entity == null)
            return null;

        return InvokeMember(entity, entity.GetType(), memberName, bindings, args);
    }

    /// <summary>
    /// 调用成员，用于静态成员
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="memberName">Name of the member.</param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object InvokeMember(Type entityType, string memberName, object[] args)
    {
        if (entityType == null)
            return null;

        return InvokeMember(null, entityType, memberName, args);
    }

    /// <summary>
    /// 调用成员，用于静态成员
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="memberName">Name of the member.</param>        
    /// <returns></returns>
    public static object InvokeMember(Type entityType, string memberName)
    {
        if (entityType == null)
            return null;

        return InvokeMember(null, entityType, memberName, EmptyObjects);
    }

    /// <summary>
    /// 调用成员，用于静态成员
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="memberName">Name of the member.</param>
    /// <param name="bindings"></param>
    /// <param name="args">The args.</param>
    /// <returns></returns>
    public static object InvokeMember(Type entityType, string memberName, BindingFlags bindings, params object[] args)
    {
        if (entityType == null)
            return null;

        return InvokeMember(null, entityType, memberName, bindings, args);
    }

    /// <summary>
    /// InvokeTheBestMember
    /// </summary>
    /// <param name="memberList"></param>
    /// <param name="entity"></param>
    /// <param name="memberName"></param>
    /// <param name="args"></param>
    /// <param name="argTypes"></param>
    /// <returns></returns>
    private static object InvokeBestMember(MemberInfo[] memberList, object entity, string memberName, object[] args, Type[] argTypes)
    {
        if (memberList.Length == 0)
        {
            throw new NullReferenceException("Can't find member by " + memberName);
        }
        else if (memberList.Length == 1)
        {
            return InvokeMember(entity, memberList[0], args);
        }
        else if (memberList[0].MemberType == MemberTypes.Method || memberList[0].MemberType == MemberTypes.Constructor)
        {
            //匹配最合适的函数
            MethodBase method = FindBestMethod(memberList, argTypes, Type.EmptyTypes);
            //调用最佳函数
            return InvokeMember(entity, method, args);
        }
        else
        {
            throw new NotSupportedException("Not support InvokeMember for " + memberName);
        }
    }
    #endregion

    #region MatchMethod
    /// <summary>
    /// 匹配最合适的函数
    /// </summary>
    /// <param name="memberList"></param>
    /// <param name="argTypes"></param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    private static MethodBase FindBestMethod(MemberInfo[] memberList, Type[] argTypes, Type[] genericTypes)
    {
        if (memberList == null || memberList.Length == 0)
            return null;
        MethodBase[] methodList = memberList.Select(p => p as MethodBase).Where(o => o != null).ToArray();
        return FindBestMethod(methodList, argTypes, genericTypes);
    }

    /// <summary>
    /// 匹配最合适的函数
    /// </summary>
    /// <param name="methodList"></param>
    /// <param name="argTypes"></param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    private static MethodBase FindBestMethod(MethodBase[] methodList, Type[] argTypes, Type[] genericTypes)
    {
        if (methodList == null || methodList.Length == 0)
            return null;
        List<ParameterInfo[]> parametersList = methodList.Select(p => p.GetParameters()).ToList();
        methodList = methodList.Where((p, i) => parametersList[i].Length == argTypes.Length).ToArray();
        if (methodList.Length == 1)
            return methodList[0];
        else if (methodList.Length == 0)
            throw new NullReferenceException("Can't find method by " + methodList[0].Name);
        parametersList = parametersList.Where(p => p.Length == argTypes.Length).ToList();
        //传入参数类型            
        //泛型方法转换
        for (int i = 0; i < methodList.Length; i++)
        {
            if (methodList[i].IsGenericMethodDefinition && methodList[i] is MethodInfo)
                methodList[i] = MakeGenericMethod(methodList[i] as MethodInfo, parametersList[i], argTypes, genericTypes);
        }
        //函数参数类型
        List<Type[]> typeList = methodList.Select(p => p.GetParameters().Select(o => o.ParameterType).ToArray()).ToList();
        methodList = methodList.Where((p, i) => typeList[i].Length == argTypes.Length).ToArray();
        typeList = typeList.Where(p => p.Length == argTypes.Length).ToList();
        if (methodList.Length == 0)
            throw new NullReferenceException("Can't find method by " + methodList[0].Name);
        //参数匹配最合适的函数
        int bestMi = 0;
        for (int i = 1; i < methodList.Length; i++)
        {
            if (!MatchTheFirstOne(typeList[bestMi], typeList[i], argTypes))
                bestMi = i;
        }
        return methodList[bestMi];
    }

    /// <summary>
    /// 匹配最合适的函数
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="argTypes">The arg types.</param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    public static MethodInfo FindBestMethod(Type entityType, string methodName, Type[] argTypes, Type[] genericTypes)
    {
        if (entityType == null)
            return null;
        if (string.IsNullOrEmpty(methodName) || methodName == entityType.Name)
            methodName = CONSTRUCTOR_NAME;

        MemberInfo[] memberList = entityType.GetMember(methodName);
        MethodInfo method = FindBestMethod(memberList, argTypes, genericTypes) as MethodInfo;
        if (method == null)
            throw new NullReferenceException("Can't find method by " + methodName);
        else
            return MakeGenericMethod(method, argTypes, genericTypes);
    }

    /// <summary>
    /// 匹配最合适的函数
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="argTypes">The arg types.</param>
    /// <returns></returns>
    public static MethodInfo FindBestMethod(Type entityType, string methodName, Type[] argTypes)
    {
        return FindBestMethod(entityType, methodName, argTypes, Type.EmptyTypes);
    }

    /// <summary>
    /// 匹配最合适的函数
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="methodName">Name of the method.</param>        
    /// <returns></returns>
    public static MethodInfo FindBestMethod(Type entityType, string methodName)
    {
        return FindBestMethod(entityType, methodName, Type.EmptyTypes, Type.EmptyTypes);
    }

    /// <summary>
    /// 匹配最合适的函数
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="bindings"></param>
    /// <param name="argTypes">The arg types.</param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    public static MethodInfo FindBestMethod(Type entityType, string methodName, BindingFlags bindings, Type[] argTypes, Type[] genericTypes)
    {
        if (entityType == null)
            return null;
        if (string.IsNullOrEmpty(methodName) || methodName == entityType.Name)
            methodName = CONSTRUCTOR_NAME;

        MemberInfo[] memberList = entityType.GetMember(methodName, bindings);
        MethodInfo method = FindBestMethod(memberList, argTypes, genericTypes) as MethodInfo;
        if (method == null)
            throw new NullReferenceException("Can't find method by " + methodName);
        else
            return MakeGenericMethod(method, argTypes, genericTypes);
    }

    /// <summary>
    /// 匹配最合适的函数
    /// </summary>
    /// <param name="entityType">Type of the entity.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="bindings"></param>
    /// <param name="argTypes">The arg types.</param>
    /// <returns></returns>
    public static MethodInfo FindBestMethod(Type entityType, string methodName, BindingFlags bindings, Type[] argTypes)
    {
        return FindBestMethod(entityType, methodName, bindings, argTypes, Type.EmptyTypes);
    }

    /// <summary>
    /// 如果是泛型方法则进行转换
    /// </summary>
    /// <param name="method"></param>
    /// <param name="argParameters"></param>
    /// <param name="argTypes"></param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    private static MethodInfo MakeGenericMethod(MethodInfo method, ParameterInfo[] argParameters, Type[] argTypes, Type[] genericTypes)
    {
        if (!method.IsGenericMethodDefinition)
            return method;

        if (argParameters.Length != argTypes.Length)
            return method;

        var dictionary = method.GetGenericArguments().ToDictionary(p => p);
        for (int i = 0; i < argParameters.Length; i++)
        {
            Type parameterType = argParameters[i].ParameterType;
            if (parameterType.IsGenericType && argTypes[i].IsGenericType)
            {
                Type baseType = parameterType.GetGenericTypeDefinition();
                Type genericType = TypeHelper.GetInheritBaseType(argTypes[i], baseType, true);
                if (genericType != null)
                {
                    Type[] types1 = parameterType.GetGenericArguments();
                    Type[] types2 = genericType.GetGenericArguments();
                    if (types1.Length == types2.Length)
                    {
                        for (int j = 0; j < types1.Length; j++)
                        {
                            if (dictionary.ContainsKey(types1[j]))
                                dictionary[types1[j]] = types2[j];
                        }
                    }
                }
            }
        }
        if (genericTypes == null)
            genericTypes = Type.EmptyTypes;
        List<Type> typeArgs = new List<Type>(genericTypes);
        typeArgs.AddRange(dictionary.Values.Where(p => !p.IsGenericParameter));
        method = method.MakeGenericMethod(typeArgs.ToArray());

        return method;
    }

    /// <summary>
    /// 如果是泛型方法则进行转换
    /// </summary>
    /// <param name="method"></param>        
    /// <param name="argTypes"></param>
    /// <param name="genericTypes"></param>
    /// <returns></returns>
    private static MethodInfo MakeGenericMethod(MethodInfo method, Type[] argTypes, Type[] genericTypes)
    {
        if (method.IsGenericMethodDefinition)
            return MakeGenericMethod(method, method.GetParameters(), argTypes, genericTypes);
        else
            return method;
    }

    /// <summary>
    /// 如果types1最匹配argTypes则返回true，否则返回false
    /// </summary>
    /// <param name="types1"></param>
    /// <param name="types2"></param>
    /// <param name="argTypes"></param>
    /// <returns></returns>
    private static bool MatchTheFirstOne(Type[] types1, Type[] types2, Type[] argTypes)
    {
        if (types1.Length == types2.Length && types1.Length == argTypes.Length)
        {
            for (int i = 0; i < argTypes.Length; i++)
            {
                if (types1[i] != types2[i])
                {
                    if (types1[i] == argTypes[i])
                        return true;
                    else if (types2[i] == argTypes[i])
                        return false;
                    else if (TypeHelper.IsInheritBase(argTypes[i], types1[i], true))
                        return true;
                    else
                        return false;
                }
            }
        }

        if (types1.Length == argTypes.Length)
            return true;
        else if (types2.Length == argTypes.Length)
            return false;
        else
            return true;
    }
    #endregion

    #region Utils
    /// <summary>
    /// 查找PropertyInfo
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static PropertyInfo SearchPropertyInfo(Type entityType, string propertyName)
    {
        if (entityType == null || string.IsNullOrEmpty(propertyName))
            return null;

        PropertyInfo property = null;
        if (MetaModel.Default != null)
        {
            MetaTable metaTable = MetaModel.Default.GetTable(entityType);
            if (metaTable != null)
            {
                MetaColumn metaColumn = metaTable.GetColumn(propertyName);
                if (metaColumn != null)
                    property = metaColumn.EntityTypeProperty;
                else
                    property = entityType.GetProperty(propertyName);
            }
            else
            {
                property = entityType.GetProperty(propertyName);
            }
        }
        else
        {
            property = entityType.GetProperty(propertyName);
        }
        return property;
    }

    /// <summary>
    /// ChangeValueType
    /// </summary>
    /// <param name="value"></param>
    /// <param name="conversionType"></param>
    /// <returns></returns>
    public static object ChangeValueType(object value, Type conversionType)
    {
        if (value == DBNull.Value || value == null)
            return null;

        if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            //NullableType
            var converter = new System.ComponentModel.NullableConverter(conversionType);
            return converter.ConvertFrom(value);
        }
        else
        {
            //NonNullableType
            IConvertible icv = value as IConvertible;
            if (icv != null)
                return Convert.ChangeType(value, conversionType);
            else
                return value;
        }
    }
    #endregion
}

