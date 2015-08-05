using SCG.SINOStock.WCFService;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     RuleBase
 *   说明:       
 *   创建时间:   2014/1/24 17:40:25
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class RuleBase : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Rule关键属性
        private Boolean _isBusy = true;
        public Boolean IsBusy
        {
            get { return _isBusy; }
            protected set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }
        private Boolean _hasChange;
        public Boolean HasChanges
        {
            get { return _hasChange; }
            protected set
            {
                if (_hasChange != value)
                {
                    _hasChange = value;
                    OnPropertyChanged("HasChanges");
                }
            }
        }
        public bool AllowMultipleErrors { get; set; }
        #endregion
        protected Exception _lastError;
        private ISINOStockServiceProxy _proxy;
        protected ISINOStockServiceProxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = new SINOStockServiceProxy();
                    _proxy.PropertyChanged += (s, e) =>
                    {
                        switch (e.PropertyName)
                        {
                            case "ActiveCallCount":
                                IsBusy = (Proxy.ActiveCallCount == 0);
                                break;
                        }
                    };
                }
                return _proxy;
            }
        }
        public Account CurrentAccount
        {
             get
            {
                return Common.ServiceDataLocator.GetInstance<Account>();
            }
           
        }

    }
}
