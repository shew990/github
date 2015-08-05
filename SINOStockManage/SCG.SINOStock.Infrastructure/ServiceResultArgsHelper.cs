using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.Infrastructure
 *   文件名:     ServiceResultArgsHelper
 *   说明:       
 *   创建时间:   2014/1/26 23:27:35
 *   作者:       liende
 */
namespace SCG.SINOStock.Infrastructure
{
    public class ResultArgs<T> : AsyncCompletedEventArgs
    {
        public ResultArgs(T result, Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
            Results = result;
        }

        public bool HasError
        {
            get { return Error != null; }
        }

        public T Results { get; private set; }
    }

    public class ResultsArgs<T> : AsyncCompletedEventArgs
    {
        public ResultsArgs(IEnumerable<T> results, Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
            Results = results;
        }

        public bool HasError
        {
            get { return Error != null; }
        }

        public IEnumerable<T> Results { get; private set; }
    }  
}
