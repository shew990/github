using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.WCFService
 *   文件名:     ISINOStockServiceProxy
 *   说明:       
 *   创建时间:   2014/1/2 0:08:16
 *   作者:       liende
 */
namespace SCG.SINOStock.WCFService
{
    public interface ISINOStockServiceProxy : ISINOStockService, INotifyPropertyChanged
    {
        int ActiveCallCount { get; }
    }
}
