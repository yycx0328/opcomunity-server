using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Web.UI;
using System.Runtime.Serialization;
namespace Utility
{
    /// <summary>
    /// SerializeHelper
    /// </summary>
    public static class SerializeHelper
    {
        private static System.Xml.Serialization.XmlSerializerNamespaces EmptyXmlSerializerNamespaces
        {
            get
            {
                return Singleton<System.Xml.Serialization.XmlSerializerNamespaces>.GetInstance(
                    delegate()
                    {
                        System.Xml.Serialization.XmlSerializerNamespaces namespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
                        namespaces.Add(string.Empty, string.Empty);
                        return namespaces;
                    });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static byte[] ToBinary<T>(this T o) where T : class, new()
        {
            byte[] bytes = null;
            DataContractSerializer dc = new DataContractSerializer(typeof(T));


            using (MemoryStream ms = new MemoryStream())
            {
                //formatter.Serialize(ms, value);
                dc.WriteObject(ms, o);
                ms.Seek(0, 0);
                bytes = ms.ToArray();
            }

            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="input"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static TResult FromBinary<TResult>(this TResult input, byte[] bits) where TResult : class, new()
        {
            TResult result = default(TResult);
            DataContractSerializer dc = new DataContractSerializer(typeof(TResult));
            //IFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bits))
            {
                result = (TResult)dc.ReadObject(ms);
            }

            return result;
        }

        #region BinarySerializer
        /// <summary>
        /// Use BinaryFormatter serialize an object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static byte[] BinarySerialize(object source)
        {
            if (source == null)
                return new byte[0] { };

            using (MemoryStream ms = new MemoryStream())
            {
                Singleton<BinaryFormatter>.GetInstance().Serialize(ms, source);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Use BinaryFormatter serialize an object
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="source">The source.</param>
        public static void BinarySerialize(Stream stream, object source)
        {
            bool isNull = (source == null);
            stream.WriteByte(Convert.ToByte(isNull));
            Singleton<BinaryFormatter>.GetInstance().Serialize(stream, source);
        }

        /// <summary>
        /// Use BinaryFormatter deserialize an array of byte
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static object BinaryDeserialize(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return null;

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return Singleton<BinaryFormatter>.GetInstance().Deserialize(ms);
            }
        }

        /// <summary>
        /// Use BinaryFormatter deserialize a stream
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static object BinaryDeserialize(Stream stream)
        {
            bool isNull = Convert.ToBoolean(stream.ReadByte());
            if (isNull)
                return null;
            else
                return Singleton<BinaryFormatter>.GetInstance().Deserialize(stream);
        }
        #endregion

        #region StringSerializer
        /// <summary>
        /// Use ObjectStateFormatter serialize an object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string StringSerialize(object source)
        {
            return Singleton<ObjectStateFormatter>.GetInstance().Serialize(source);
        }

        /// <summary>
        /// Use ObjectStateFormatter serialize an object
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="source">The source.</param>
        public static void StringSerialize(Stream stream, object source)
        {
            Singleton<ObjectStateFormatter>.GetInstance().Serialize(stream, source);
        }

        /// <summary>
        /// Use ObjectStateFormatter deserialize a string
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static object StringDeserialize(string value)
        {
            return Singleton<ObjectStateFormatter>.GetInstance().Deserialize(value);
        }

        /// <summary>
        /// Use ObjectStateFormatter deserialize a stream
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static object StringDeserialize(Stream stream)
        {
            return Singleton<ObjectStateFormatter>.GetInstance().Deserialize(stream);
        }
        #endregion

        #region XmlSerialize
        /// <summary>
        /// Use XmlSerializer serialize an object
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static XmlDocument XmlSerialize<T>(T source)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            using(XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8))
            {                
                if (source != null && typeof(T) != source.GetType())
                    new XmlSerializer(source.GetType()).Serialize(writer, source, EmptyXmlSerializerNamespaces);
                else
                    Singleton<XmlSerializerT<T>>.GetInstance().Serialize(writer, source, EmptyXmlSerializerNamespaces);

                ms.Position = 0;
                doc.Load(ms);                
                return doc;
            }
        }

        /// <summary>
        /// Use XmlSerializer serialize an object
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="source"></param>
        public static void XmlSerialize<T>(Stream stream, object source)
        {
            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {
                if (source != null && typeof(T) != source.GetType())
                    new XmlSerializer(source.GetType()).Serialize(writer, source, EmptyXmlSerializerNamespaces);
                else
                    Singleton<XmlSerializerT<T>>.GetInstance().Serialize(writer, source, EmptyXmlSerializerNamespaces);
            }
        }

        /// <summary>
        /// Use XmlSerializer serialize an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer"></param>
        /// <param name="source"></param>
        public static void XmlSerialize<T>(XmlWriter writer, object source)
        {
            if (source != null && typeof(T) != source.GetType())
                new XmlSerializer(source.GetType()).Serialize(writer, source, EmptyXmlSerializerNamespaces);
            else
                Singleton<XmlSerializerT<T>>.GetInstance().Serialize(writer, source, EmptyXmlSerializerNamespaces);
        }

        /// <summary>
        /// Use XmlSerializer deserialize a string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return default(T);

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(xml);
            using (System.IO.MemoryStream ms = new MemoryStream(buffer))
            using (XmlTextReader reader = new XmlTextReader(ms))
            {
                return (T)Singleton<XmlSerializerT<T>>.GetInstance().Deserialize(reader);
            }
        }

        /// <summary>
        /// Use XmlSerializer deserialize a stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(Stream stream)
        {
            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                return (T)Singleton<XmlSerializerT<T>>.GetInstance().Deserialize(stream);
            }
        }

        /// <summary>
        /// Use XmlSerializer deserialize a XmlReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(XmlReader reader)
        {
            return (T)Singleton<XmlSerializerT<T>>.GetInstance().Deserialize(reader);
        }
        #endregion

        #region JsonSerialize
        /// <summary>
        /// Use DataContractJsonSerializer serialize an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string JsonSerialize<T>(T source)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                Singleton<JsonSerializerT<T>>.GetInstance().Serializer.WriteObject(ms, source);
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Use DataContractJsonSerializer serialize an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="source"></param>
        public static void JsonSerialize<T>(Stream stream, T source)
        {
            Singleton<JsonSerializerT<T>>.GetInstance().Serializer.WriteObject(stream, source);
        }

        /// <summary>
        /// Use DataContractJsonSerializer serialize an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer"></param>
        /// <param name="source"></param>
        public static void JsonSerialize<T>(XmlWriter writer, T source)
        {
            Singleton<JsonSerializerT<T>>.GetInstance().Serializer.WriteObject(writer, source);
        }

        /// <summary>
        /// Use DataContractJsonSerializer deserialize a stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(Stream stream)
        {
            return (T)Singleton<JsonSerializerT<T>>.GetInstance().Serializer.ReadObject(stream);
        }

        /// <summary>
        /// Use DataContractJsonSerializer deserialize a XmlReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(XmlReader reader)
        {
            return (T)Singleton<JsonSerializerT<T>>.GetInstance().Serializer.ReadObject(reader);
        }
        #endregion
    }

    /// <summary>
    /// XmlSerializerT
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlSerializerT<T> : XmlSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializerT&lt;T&gt;"/> class.
        /// </summary>
        public XmlSerializerT()
            : base(typeof(T))
        {            
        }
    }

    /// <summary>
    /// JsonSerializerT
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonSerializerT<T> 
    {
        /// <summary>
        /// JsonSerializerT
        /// </summary>
        public System.Runtime.Serialization.Json.DataContractJsonSerializer Serializer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializerT&lt;T&gt;"/> class.
        /// </summary>
        public JsonSerializerT()
        {
            Serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
        }
    }
}
