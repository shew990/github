using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.Common
 *   文件名:     ServiceDataLocator
 *   说明:       
 *   创建时间:   2014/1/27 22:57:09
 *   作者:       liende
 */
namespace SCG.SINOStock.Common
{
    public class ServiceDataLocator
    {
        private static readonly Dictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static void Register<T>(T obj) where T : class
        {
            if (Instances.ContainsKey(typeof(T)))
            {
                Instances.Remove(typeof(T));
            }
            Instances.Add(typeof(T), obj);
        }
        public static T GetInstance<T>() where T : class
        {
            if (Instances.ContainsKey(typeof(T)))
            {
                return (T)Instances[typeof(T)];
            }
            return null;
        }
        public static void Clear()
        {
            Instances.Clear();
        }
        public static void Remove<T>(T obj) where T : class
        {
            if (Instances.ContainsKey(obj.GetType()))
            {
                Instances.Remove(obj.GetType());
            }
        }
    }
}
