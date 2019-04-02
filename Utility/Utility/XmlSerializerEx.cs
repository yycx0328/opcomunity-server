// =========================================================
// Author	:   luyunhai
// Create Time  :   8/10/2015 10:51:24 PM
// =========================================================
// Copyright © USER-VFH583E7VU 2015 . All rights reserved.
// =========================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

/// <summary>
/// SerializeHelper
/// </summary>
public static partial class XmlSerializerEx
{
    /// <summary>
    /// 参见：http://msdn.microsoft.com/zh-cn/library/3003scdt(VS.80).aspx
    /// </summary>
    private static Dictionary<string, Type> SimpleTypes
    {
        get
        {
            if (_SimpleTypes == null)
            {
                //the first letter is lowercase, can use XmlSerializer directly
                _SimpleTypes = new Dictionary<string, Type>();
                _SimpleTypes["string"] = typeof(string);
                _SimpleTypes["dateTime"] = typeof(DateTime);
                _SimpleTypes["boolean"] = typeof(bool);
                _SimpleTypes["unsignedByte"] = typeof(byte);
                _SimpleTypes["char"] = typeof(char);
                _SimpleTypes["decimal"] = typeof(decimal);
                _SimpleTypes["double"] = typeof(double);
                _SimpleTypes["guid"] = typeof(Guid);
                _SimpleTypes["short"] = typeof(short);
                _SimpleTypes["int"] = typeof(int);
                _SimpleTypes["long"] = typeof(long);
                _SimpleTypes["byte"] = typeof(sbyte);
                _SimpleTypes["float"] = typeof(float);
                _SimpleTypes["unsignedShort"] = typeof(UInt16);
                _SimpleTypes["unsignedInt"] = typeof(UInt32);
                _SimpleTypes["unsignedLong"] = typeof(UInt64);
                _SimpleTypes["base64Binary"] = typeof(byte[]);
                _SimpleTypes["QName"] = typeof(XmlQualifiedName);
                _SimpleTypes["time"] = typeof(DateTime);
            }
            return _SimpleTypes;
        }
    }
    private static Dictionary<string, Type> _SimpleTypes = null;

    /// <summary>
    /// 缓存XmlSerializer
    /// </summary>
    private static Dictionary<Type, XmlSerializer> XmlSerializerDictionary = new Dictionary<Type, XmlSerializer>();

    /// <summary>
    /// Use XmlSerializer serialize an object
    /// </summary>
    /// <param name="source"></param>
    /// <param name="writeObjectType"></param>
    /// <returns></returns>
    public static XmlNode XmlSerialize(object source, bool writeObjectType)
    {
        XmlDocument document = new XmlDocument();

        Type sourceType = (source == null ? typeof(object) : source.GetType());
        using (MemoryStream ms = new MemoryStream())
        {
            XmlTextWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);
            writer.WriteStartElement("XmlSerialize");
            writer.WriteAttributeString("xmlns", "xsi", "http://www.w3.org/2000/xmlns/", "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns", "xsd", "http://www.w3.org/2000/xmlns/", "http://www.w3.org/2001/XMLSchema");

            XmlWriterBaseSerialize(writer, source, sourceType, writeObjectType);

            writer.WriteEndElement();
            writer.Flush();

            ms.Seek(0, System.IO.SeekOrigin.Begin);
            document.Load(ms);
        }

        XmlNode result = document.DocumentElement.ChildNodes[0];

        if (writeObjectType && !IsSimpleType(sourceType) && result.Attributes["objectType"] == null)
            XmlHelper.SetAttribute(result, "objectType", GetTypeName(sourceType));

        return result;
    }

    /// <summary>
    /// Use XmlSerializer serialize an object
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="source"></param>
    /// <param name="autoWriteObjectType"></param>
    public static void XmlWriterSerialize(XmlTextWriter writer, object source, bool autoWriteObjectType)
    {
        XmlWriterSerialize(writer, source, null, autoWriteObjectType);
    }

    #region Serialize Functions
    /// <summary>
    /// XmlWriterSerialize
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="source"></param>
    /// <param name="sourceType"></param>
    /// <param name="autoWriteObjectType"></param>
    private static void XmlWriterSerialize(XmlTextWriter writer, object source, Type sourceType, bool autoWriteObjectType)
    {
        if (sourceType == null)
            sourceType = (source == null ? typeof(object) : source.GetType());

        //IsArray
        if (sourceType.IsArray)
        {
            Array array = (Array)source;
            if (array.Rank == 1 && IsSimpleType(sourceType))
                XmlWriterBaseSerialize(writer, source, sourceType, autoWriteObjectType);
            else
                XmlSerializeArray(writer, sourceType, array, sourceType.GetElementType(), autoWriteObjectType);
            return;
        }
        //SimpleType
        if (source == null || IsSimpleType(sourceType))
        {
            XmlWriterBaseSerialize(writer, source, sourceType, autoWriteObjectType);
            return;
        }

        //Type is a known type
        Type typeObject = source as Type;
        if (typeObject != null)
        {
            writer.WriteStartElement("Type");
            writer.WriteString(GetTypeName(source as Type));
            writer.WriteEndElement();
            return;
        }

        //NameObjectCollectionBase || NameValueCollection
        NameObjectCollectionBase nameObject = source as NameObjectCollectionBase;
        if (nameObject != null)
        {
            NameValueCollection nameValue = nameObject as NameValueCollection;
            if (nameValue != null)
            {
                var array = nameValue.Cast<string>().Select(p => new KeyValuePair<string, string>(p, nameValue[p])).ToArray();
                XmlSerializeArray<KeyValuePair<string, string>>(writer, sourceType, array, autoWriteObjectType);
            }
            else
            {
                System.Reflection.MethodInfo baseGet = sourceType.GetMethod("BaseGet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new Type[] { typeof(string) }, null);
                var array = nameObject.Cast<string>().Select(p => new KeyValuePair<string, object>(p, ReflectionHelper.CallMethod(nameObject, baseGet, new object[] { p }))).ToArray();
                XmlSerializeArray<KeyValuePair<string, object>>(writer, sourceType, array, autoWriteObjectType);
            }
            return;
        }

        //IDictionary || IDictionary`2
        IDictionary dictionaryObject = source as IDictionary;
        if (dictionaryObject != null)
        {
            if (sourceType.IsGenericType)
                XmlSerializeIEnumerableT(writer, source, sourceType, autoWriteObjectType);
            else
                XmlSerializeArray<KeyValuePair<object, object>>(writer, sourceType, dictionaryObject.Cast<DictionaryEntry>().Select(p => new KeyValuePair<object, object>(p.Key, p.Value)).ToArray(), autoWriteObjectType);
            return;
        }

        //IList || IList`1
        IList listObject = source as IList;
        if (listObject != null)
        {
            if (sourceType.IsGenericType)
                XmlSerializeIEnumerableT(writer, source, sourceType, autoWriteObjectType);
            else
                XmlSerializeArray<object>(writer, sourceType, listObject.Cast<object>().ToArray(), autoWriteObjectType);
            return;
        }

        //ICollection || ICollection`1
        ICollection collectionObject = source as ICollection;
        if (collectionObject != null)
        {
            if (sourceType.IsGenericType)
                XmlSerializeIEnumerableT(writer, source, sourceType, autoWriteObjectType);
            else
            {
                object[] array = new object[collectionObject.Count];
                collectionObject.CopyTo(array, 0);
                XmlSerializeArray<object>(writer, sourceType, array, autoWriteObjectType);
            }
            return;
        }

        //DataRow
        DataRow row = source as DataRow;
        if (row != null)
        {
            KeyValuePair<string, object>[] array = new KeyValuePair<string, object>[row.Table.Columns.Count];
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                array[i] = new KeyValuePair<string, object>(row.Table.Columns[i].ColumnName, row[i]);
            }
            XmlSerializeArray<KeyValuePair<string, object>>(writer, sourceType, array, autoWriteObjectType);
            return;
        }

        //序列化属性和字段
        EntityXmlSerialize(writer, source, sourceType, autoWriteObjectType);
    }

    private static void XmlWriterBaseSerialize(XmlTextWriter writer, object source, Type sourceType, bool autoWriteObjectType)
    {
        if (sourceType == null)
            sourceType = (source == null ? typeof(object) : source.GetType());
        XmlSerializer serializer = GetXmlSerializer(sourceType);
        if (serializer == null)
        {
            XmlWriterSerialize(writer, source, autoWriteObjectType);
        }
        else
        {
            writer.Flush();
            long position = writer.BaseStream.Position;
            try
            {
                serializer.Serialize(writer, source);
            }
            catch
            {
                writer.Flush();
                writer.BaseStream.Position = position + 1;
                int ch = writer.BaseStream.ReadByte();
                if (ch == Convert.ToInt32('<'))
                {
                    writer.WriteEndElement();
                    writer.Flush();
                }
                writer.BaseStream.SetLength(position + 1);

                XmlWriterSerialize(writer, source, autoWriteObjectType);
            }
        }
    }

    /// <summary>
    /// 序列化实体的公有属性和字段
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="source"></param>
    /// <param name="sourceType"></param>
    /// <param name="autoWriteObjectType"></param>
    private static void EntityXmlSerialize(XmlTextWriter writer, object source, Type sourceType, bool autoWriteObjectType)
    {
        writer.WriteStartElement(GetCleanName(sourceType.Name));
        if (autoWriteObjectType)
            writer.WriteAttributeString("objectType", GetTypeName(sourceType));

        object value;
        //Properties
        foreach (var pi in sourceType.GetProperties().OrderBy(p => p.Name))
        {
            value = ReflectionHelper.GetProperty(source, pi);
            EntityMemberXmlSerialize(writer, autoWriteObjectType, value, pi.Name);
        }
        //Fields
        foreach (var fi in sourceType.GetFields().OrderBy(p => p.Name))
        {
            value = ReflectionHelper.GetField(source, fi);
            EntityMemberXmlSerialize(writer, autoWriteObjectType, value, fi.Name);
        }

        writer.WriteEndElement();
    }

    private static void EntityMemberXmlSerialize(XmlTextWriter writer, bool autoWriteObjectType, object memberValue, string memberName)
    {
        if (memberValue != null)
        {
            if (memberValue.GetType().IsPublic)
            {
                XmlNode node = XmlHelper.ReplaceTag(XmlSerialize(memberValue, autoWriteObjectType), memberName);
                writer.WriteNode(node.CreateNavigator(), false);
            }
            else
            {
                writer.WriteStartElement(memberName);
                writer.WriteString(memberValue.ToString());
                writer.WriteEndElement();
            }
        }
    }

    private static void XmlSerializeIEnumerableT(XmlTextWriter writer, object source, Type sourceType, bool autoWriteObjectType)
    {
        Array array = ReflectionHelper.CallMethod(typeof(Enumerable), "ToArray", new object[] { source }) as Array;

        Type genericType = sourceType.FindInterfaces((m, o) => m.IsGenericType && m.GetGenericTypeDefinition() == typeof(IEnumerable<>), null).FirstOrDefault();
        Type elementType = genericType.GetGenericArguments()[0];
        XmlSerializeArray(writer, sourceType, array, elementType, autoWriteObjectType);
    }

    /// <summary>
    /// 序列化数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="writer"></param>
    /// <param name="sourceType"></param>
    /// <param name="array"></param>
    /// <param name="autoWriteObjectType"></param>
    private static void XmlSerializeArray<T>(XmlTextWriter writer, Type sourceType, T[] array, bool autoWriteObjectType)
    {
        XmlSerializeArray(writer, sourceType, array, typeof(T), autoWriteObjectType);
    }

    /// <summary>
    /// 序列化数组
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="sourceType"></param>
    /// <param name="array"></param>
    /// <param name="elementType"></param>
    /// <param name="autoWriteObjectType"></param>
    private static void XmlSerializeArray(XmlTextWriter writer, Type sourceType, Array array, Type elementType, bool autoWriteObjectType)
    {
        string cleanTypeName = GetCleanName(sourceType.Name);
        writer.WriteStartElement(cleanTypeName);
        if (autoWriteObjectType && !ContainsSimpleTypeEx(cleanTypeName, sourceType))
            writer.WriteAttributeString("objectType", GetTypeName(sourceType));

        if (IsSimpleType(elementType) && array.Rank == 1)
        {
            XmlWriterBaseSerialize(writer, array, sourceType, autoWriteObjectType);
        }
        else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
        {
            Type keyType = Type.GetType(Arithmetic.GetInlineItem(elementType.FullName, 1, '[', ']'));
            Type valueType = Type.GetType(Arithmetic.GetInlineItem(elementType.FullName, 2, '[', ']'));
            System.Reflection.PropertyInfo keyProperty = elementType.GetProperty("Key");
            System.Reflection.PropertyInfo valueProperty = elementType.GetProperty("Value");
            bool keyIsSimpleType = SimpleTypes.ContainsValue(keyType);
            bool valueIsSimpleType = SimpleTypes.ContainsValue(valueType);
            System.Xml.Serialization.XmlSerializer keySerializer = keyIsSimpleType ? GetXmlSerializer(keyType) : null;
            System.Xml.Serialization.XmlSerializer valueSerializer = valueIsSimpleType ? GetXmlSerializer(valueType) : null;

            object keyObj, valueObj;
            foreach (object item in array)
            {
                keyObj = ReflectionHelper.GetProperty(item, keyProperty);
                valueObj = ReflectionHelper.GetProperty(item, valueProperty);
                writer.WriteStartElement("Item");
                if (keyIsSimpleType)
                    writer.WriteAttributeString("key", new XAttribute("key", keyObj).Value);
                if (valueIsSimpleType)
                    writer.WriteAttributeString("value", new XAttribute("value", valueObj).Value);
                if (!keyIsSimpleType)
                    writer.WriteNode(XmlSerialize(keyObj, autoWriteObjectType).CreateNavigator(), false);
                if (!valueIsSimpleType)
                    writer.WriteNode(XmlSerialize(valueObj, autoWriteObjectType).CreateNavigator(), false);
                writer.WriteEndElement();

            }
        }
        else
        {
            foreach (object item in array)
            {
                writer.WriteNode(XmlSerialize(item, autoWriteObjectType).CreateNavigator(), false);
            }
        }
        writer.WriteEndElement();
    }
    #endregion

    #region utils
    /// <summary>
    /// 获取系统可以识别的类型名称
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static string GetTypeName(Type type)
    {
        return TypeHelper.GetShortTypeName(type);
    }

    /// <summary>
    /// 将类型名转换为可以作为XmlNode节点名称的类型名称
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string GetCleanName(string name)
    {
        if (name.EndsWith("[]"))
            name = "ArrayOf" + name.Substring(0, name.Length - 2);
        StringBuilder sb = new StringBuilder();
        foreach (char ch in name)
        {
            if (char.IsControl(ch) || char.IsPunctuation(ch) || char.IsSymbol(ch))
                sb.Append('_');
            else
                sb.Append(ch);
        }
        return sb.ToString();
        //return name.Replace('`', '_').Replace('[', '_').Replace(']', '_').Replace(',', '_');
    }

    private static bool IsSimpleType(Type type)
    {
        if (type.IsGenericType)
            return false;
        else if (type.IsArray)
            return SimpleTypes.ContainsValue(TypeHelper.GetArrayItemType(type));
        else
            return SimpleTypes.ContainsValue(type);
    }

    private static bool ContainsSimpleType(string name, Type type)
    {
        Type simpleType;
        if (!SimpleTypes.TryGetValue(name, out simpleType))
            return false;
        return (simpleType == type);
    }

    private static bool ContainsSimpleTypeEx(string name, Type type)
    {
        if (type.FullName == "System.RuntimeType" && type.BaseType != null)
            type = type.BaseType;
        const string ArrayOf = "ArrayOf";
        if (type.IsArray && name.StartsWith(ArrayOf))
        {
            //数组
            Type elmType = TypeHelper.GetArrayItemType(type);
            string elmName = name.Substring(ArrayOf.Length);
            bool isKnownType = ContainsSimpleType(elmName, elmType);
            if (!isKnownType)
            {
                elmName = elmName[0].ToString().ToLower() + elmName.Substring(1);
                isKnownType = ContainsSimpleType(elmName, elmType);
            }
            return isKnownType;
        }
        else if (type.IsGenericType)
        {
            return false;
        }
        else
        {
            return ContainsSimpleType(name, type);
        }
    }

    private static XmlSerializer GetXmlSerializer(Type type)
    {
        XmlSerializer serializer;
        if (XmlSerializerDictionary.TryGetValue(type, out serializer))
            return serializer;

        try
        {
            serializer = new XmlSerializer(type);
        }
        catch
        {
            serializer = null;
        }
        XmlSerializerDictionary[type] = serializer;
        return serializer;
    }
    #endregion
}
