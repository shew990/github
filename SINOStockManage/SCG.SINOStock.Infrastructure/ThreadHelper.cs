using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

/**
 *   命名空间:   SCG.SINOStock.Infrastructure
 *   文件名:     ThreadHelper
 *   说明:       
 *   创建时间:   2014/1/1 19:36:24
 *   作者:       liende
 */
namespace SCG.SINOStock.Infrastructure
{
    public class ThreadHelper
    {
        public static void BeginInvokeOnUIThread(Action action)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
