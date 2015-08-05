using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ViewModels.QualityInfo
 *   文件名:     QualityInfoDetailViewModel
 *   说明:       
 *   创建时间:   2014/3/4 15:19:44
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class QualityInfoDetailViewModel : ViewModelBase
    {
        private QualityInfoRule _rule = null;
        public QualityInfoDetailViewModel()
        {
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                if (InfoTypeCollection == null )
                {
                    InfoTypeCollection = new ObservableCollection<string>();
                    InfoTypeCollection.Add("入库");
                    InfoTypeCollection.Add("减薄");
                    InfoTypeCollection.Add("抛光");
                    InfoTypeCollection.Add("镀膜");
                }
                CurrentQualityInfo = param.Entity as QualityInfo;
                if (CurrentQualityInfo == null)
                {
                    CurrentQualityInfo = new QualityInfo();
                }

            }, ThreadOption.UIThread, true, p => p.Target == "QualityInfoDetailViewModel");
            _rule = new QualityInfoRule();
            _rule.AddQualityInfoCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    Common.MessageBox.Show("添加成功");
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdViewName = CmdViewName.QualityInfoMainView,
                        Target = "Sell",
                    });
                }
            };
            _rule.ModifyQualityInfoCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    Common.MessageBox.Show("修改成功");
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdViewName = CmdViewName.QualityInfoMainView,
                        Target = "Sell",
                    });
                }
            };
        }
        #region 界面绑定属性
        private QualityInfo _currentQualityInfo;
        public QualityInfo CurrentQualityInfo
        {
            get { return _currentQualityInfo; }
            set
            {
                _currentQualityInfo = value;
                this.RaisePropertyChanged("CurrentQualityInfo");
            }
        }
        private ObservableCollection<string> _infoTypeCollection;
        public ObservableCollection<string> InfoTypeCollection
        {
            get { return _infoTypeCollection; }
            set
            {
                _infoTypeCollection = value;
                this.RaisePropertyChanged("InfoTypeCollection");
            }
        }
        private DelegateCommand _cmdPageLoad;
        public DelegateCommand CmdPageLoad
        {
            get
            {
                if (_cmdPageLoad == null)
                {
                    _cmdPageLoad = new DelegateCommand(() =>
                    {
                        // GetAccountsAsyns();

                    });
                }
                return _cmdPageLoad;
            }
        }
        private DelegateCommand _cmdGotoList;
        public DelegateCommand CmdGotoList
        {
            get
            {
                if (_cmdGotoList == null)
                {
                    _cmdGotoList = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.QualityInfoMainView,
                            Target = "Sell",
                        });
                        //   GetAccountsAsyns();
                    });
                }
                return _cmdGotoList;
            }
            set { _cmdGotoList = value; }
        }

        private DelegateCommand _cmdSave;
        public DelegateCommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new DelegateCommand(() =>
                    {
                        //var tmp = RuleFunctionList;
                        if (CurrentQualityInfo != null)
                        {
                            if (string.IsNullOrWhiteSpace(CurrentQualityInfo.Name))
                            {
                                Common.MessageBox.Show("品质名不能为空!");
                                return;
                            }
                            if (CurrentQualityInfo.ID <= 0)
                            {
                                _rule.AddQualityInfoAsyns(CurrentQualityInfo);
                            }
                            else
                            {
                                _rule.ModifyQualityInfoAsyns(CurrentQualityInfo);
                            }
                        }
                    });
                }
                return _cmdSave;
            }
        }

        #endregion
    }
}
