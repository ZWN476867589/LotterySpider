using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LotterySpider.Business.UtilTools
{
    public static class EntityHelper
    {
        /// <summary>
        /// 获取对象的属性名称列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<string> getPropertiesNames<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return properties.Select(p => p.Name).ToList();
        }
        /// <summary>
        /// 根据属性名称获取对象列表中对应的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<object> getPropertyValueByList<T>(List<T> data, string propertyName)
        {
            List<object> valueList = new List<object>();
            if (!String.IsNullOrEmpty(propertyName) && data != null && data.Count > 0)
            {
                foreach (T obj in data)
                {
                    PropertyInfo[] infos = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropertyInfo info = infos.FirstOrDefault(p => p.Name == propertyName);
                    if (info != null)
                    {
                        try
                        {
                            valueList.Add(infos.FirstOrDefault(p => p.Name == propertyName).GetValue(obj, null));
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            return valueList;
        }
        /// <summary>
        /// 获取实体对象中属性名和值的对应关系
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>		
        public static Dictionary<string, object> getEntityPropertiesAndValue<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return properties.ToDictionary(p => p.Name, p => p.GetValue(obj, null));
        }
        /// <summary>
        /// 设置对象的每个属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertiseNameAndValue"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T setPropertiseValue<T>(Dictionary<string, object> propertiseNameAndValue, T obj)
        {
            if (obj == null)
            {
                return obj;
            }
            PropertyInfo[] infos = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (infos.Length <= 0)
            {
                return obj;
            }
            foreach (var keyAndValue in propertiseNameAndValue)
            {
                PropertyInfo info = infos.FirstOrDefault(p => p.Name == keyAndValue.Key);
                if (info != null)
                {
                    try
                    {
                        info.SetValue(obj, keyAndValue.Value, null);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception();
                    }
                }
            }
            return obj;
        }
    }
}
