using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace LotterySpider.Business
{
    public class JsonHelper
    {
        #region Json字符串序列化及反序列化
        /// <summary>
        /// 将json数据反序列化为Dictionary
        /// </summary>
        /// <param name="jsonData">json字符串</param>
        /// <returns>数据字典</returns>
        public static Dictionary<string, object> JsonToDictionary(string jsonData)
        {
            if (String.IsNullOrEmpty(jsonData))
            {
                return null;
            }
            //实例化JavaScriptSerializer类的新实例
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 将数据字典格式化为字符串
        /// </summary>
        /// <param name="jsonData">数据字典</param>
        /// <returns>json字符串</returns>
        public static string DictionaryToJson(Dictionary<string, object> jsonData)
        {
            if (jsonData == null)
            {
                return null;
            }
            if (jsonData.Count() == 0)
            {
                return null;
            }
            try
            {
                //实例化JavaScriptSerializer类的新实例
                JavaScriptSerializer jss = new JavaScriptSerializer();
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                StringBuilder sb = new StringBuilder();
                jss.Serialize(jsonData, sb);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region json添加,更改键值对
        /// <summary>
        /// json添加,更改键值对
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="jsonKey"></param>
        /// <param name="jsonValue"></param>
        /// <returns></returns>
        public static string JsonAddValue(string jsonString, string jsonKey, string jsonValue)
        {
            // 存在非标准格式
            Dictionary<string, object> tempList = JsonToDictionary(jsonString);
            // 存在jsonString为null的新增情况
            if (tempList == null)
            {
                tempList = new Dictionary<string, object>();
            }
            if (tempList.ContainsKey(jsonKey))
            {
                if (jsonValue != null)
                {
                    tempList[jsonKey] = jsonValue;
                }
                else
                {
                    tempList.Remove(jsonKey);
                }
            }
            else
            {
                tempList.Add(jsonKey, jsonValue);
            }
            return DictionaryToJson(tempList);
        }
        #endregion
        #region 获取json字符串中单个属性的值
        /// <summary>
        /// 获取json字符串中单个属性的值
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetJsonValue(string jsonString, string key)
        {
            object result = null;
            if (String.IsNullOrEmpty(jsonString))
            {
                return result;
            }
            Dictionary<string, object> tempList = JsonToDictionary(jsonString);
            if (tempList.Keys.Contains(key))
            {
                result = tempList[key];
            }
            return result;
        }
        /// <summary>
        /// 获取json字符串中单个属性的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dicJson">集合</param>
        /// <returns></returns>
        public static object GetJsonValue(string key, Dictionary<string, object> dicJson)
        {
            if (dicJson == null)
            {
                return null;
            }
            if (dicJson.Count == 0)
            {
                return null;
            }
            if (!dicJson.ContainsKey(key))
            {
                return null;
            }
            if (String.IsNullOrEmpty(key))
            {
                return null;
            }
            return dicJson[key];
        }
        /// <summary>
        /// 获取json字符串中单个属性的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dicJson">集合</param>
        /// <returns></returns>
        public static object GetJsonValue(object key, Dictionary<string, object> dicJson)
        {
            if (key == null)
            {
                return null;
            }
            return GetJsonValue(key.ToString(), dicJson);
        }
        #endregion
    }
}
