using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Touha.Infrastructure.Utils.Xml
{
    public class XmlUtil
    {
        #region 泛型方式
        #region 通过XmlSerializer进行序列化，反序列化
        /// <summary>
        /// 反序列化（xml---->obj)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlObj"></param>
        /// <returns></returns>
        public static T DeserializeViaXmlSerializer<T>(string xmlObj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xmlObj))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// 序列化（obj--->xml)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeViaXmlSerializer<T>(T obj)
        {
            using (StringWriter writer = new StringWriter())
            {
                new XmlSerializer(obj.GetType()).Serialize((TextWriter)writer, obj);
                return writer.ToString();
            }
        }
        #endregion

        #region 通过DataContractSerializer进行转换
        /// <summary>
        /// 反序列化xml-->obj
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeViaDataContractSerializer<T>(string xml)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(xml)))
                {
                    DataContractSerializer xmldes = new DataContractSerializer(typeof(T));
                    object obj = Activator.CreateInstance(typeof(T));
                    return (T)xmldes.ReadObject(ms);
                }
            }
            catch (Exception)
            {

                return default(T);
            }
        }

        /// <summary>
        /// 反序列化stream-->obj
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeViaDataContractSerializer<T>(Stream stream)
        {
            try
            {
                DataContractSerializer xmldes = new DataContractSerializer(typeof(T));
                object obj = Activator.CreateInstance(typeof(T));
                return (T)xmldes.ReadObject(stream);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 序列化obj-->xml
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializerViaDataContractSerializer<T>(T obj)
        {
            DataContractSerializer xmlser = new DataContractSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            try
            {
                xmlser.WriteObject(ms, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string str = sr.ReadToEnd();

            sr.Dispose();
            ms.Dispose();

            return str;
        }
        #endregion
        #endregion

        #region 非泛型
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion
        #endregion
    }
}
