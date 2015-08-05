using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;


/**
 *   命名空间:   SCG.SINOStock.ViewModels.Process
 *   文件名:     Process_DuMoViewModel
 *   说明:       
 *   创建时间:   2014/2/21 15:43:00
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class Process_DuMoViewModel : ViewModelBase
    {
        private StockLotRule _stockLotRule = null;
        public Process_DuMoViewModel()
        {

            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                ReadComTest();
            }, ThreadOption.UIThread, true, p => p.Target == "Process_DuMoViewModel");
            _stockLotRule = new StockLotRule();
            _stockLotRule.GetStockLotEntityByLotNoCompleted += (s, e) =>
                {
                    if (e.Cancelled || e.Results == null)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        CurrentStockLot = e.Results as StockLot;

                    }
                };
            _stockLotRule.UpdateStockDetailStatusCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, 4, IsCheckAll);
                        GlassID = string.Empty;
                    }
                };
            ScanComExecute = (s, e) =>
            {
                GlassID = s.ToString();
                _stockLotRule.UpdateStockDetailStatusAsyns(GlassID, CurrentStockLot.ID, 4);//todo是否验证写死了
                //   AddStockDetailAsyns();
            };
        }

        #region 界面绑定字段
        public bool _isCheckAll;
        public bool IsCheckAll
        {
            get { return _isCheckAll; }
            set
            {
                _isCheckAll = value;
                this.RaisePropertyChanged("IsCheckAll");
            }
        }
        private string _lotNo;
        public string LotNo
        {
            get { return _lotNo; }
            set
            {
                _lotNo = value;
                this.RaisePropertyChanged("LotNo");
            }
        }
        private StockLot _currentStockLot;

        public StockLot CurrentStockLot
        {
            get { return _currentStockLot; }
            set
            {
                _currentStockLot = value;
                // _lineNumber = 1;
                this.RaisePropertyChanged("CurrentStockLot");
            }
        }

        private string _glassID;
        /// <summary>
        /// 扫描到的GlassID
        /// </summary>
        public string GlassID
        {
            get { return _glassID; }
            set
            {
                _glassID = value;
                this.RaisePropertyChanged("GlassID");
            }
        }
        #endregion


        private DelegateCommand<RoutedEventArgs> _cmdLotOperater;
        /// <summary>
        /// LotNo文本框回车绑定事件
        /// </summary>
        public DelegateCommand<RoutedEventArgs> CmdLotOperater
        {
            get
            {
                if (_cmdLotOperater == null)
                {
                    _cmdLotOperater = new DelegateCommand<RoutedEventArgs>(e =>
                    {
                        var key = e as KeyEventArgs;
                        if (key == null || key.Key == Key.Enter)//当key为空，或按键为回车键时
                        {
                            if (string.IsNullOrWhiteSpace(LotNo))
                            {
                                Common.MessageBox.Show("请填写LOT NO");
                                return;
                            }
                            _stockLotRule.GetStockLotEntityByLotNoAsyns(LotNo, 4, IsCheckAll);
                        }
                    });
                }
                return _cmdLotOperater;
            }
        }



        private DelegateCommand<RoutedEventArgs> _cmdDetailOperater;
        /// <summary>
        /// LotNo文本框回车绑定事件
        /// </summary>
        public DelegateCommand<RoutedEventArgs> CmdDetailOperater
        {
            get
            {
                if (_cmdDetailOperater == null)
                {
                    _cmdDetailOperater = new DelegateCommand<RoutedEventArgs>(e =>
                    {
                        var key = e as KeyEventArgs;
                        if (key == null || key.Key == Key.Enter)//当key为空，或按键为回车键时
                        {
                            if (!string.IsNullOrWhiteSpace(GlassID))
                            {
                                if (CurrentStockLot == null)
                                {
                                    Common.MessageBox.Show("请先输入LOTNO，确认型号及数量，再开始扫描");
                                    return;
                                }
                                //StockDetail entity = new StockDetail();
                                //entity.GlassID = GlassID;
                                //entity.Qty = 1;
                                //entity.StockLotID = CurrentStockLot.ID;
                                //  entity.AccountID=curren
                                _stockLotRule.UpdateStockDetailStatusAsyns(GlassID, CurrentStockLot.ID, 4);//todo是否验证写死了


                            }
                        }
                    });
                }
                return _cmdDetailOperater;
            }
        }
    }
}
