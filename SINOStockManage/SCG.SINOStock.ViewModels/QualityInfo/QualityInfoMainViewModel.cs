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
using System.Windows;

/**
 *   命名空间:   SCG.SINOStock.ViewModels.QualityInfo
 *   文件名:     QualityInfoMainViewModel
 *   说明:       
 *   创建时间:   2014/3/4 15:18:43
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class QualityInfoMainViewModel : ViewModelBase
    {
        private QualityInfoRule _rule = null;
        private RoleRule _roleRule;
        public QualityInfoMainViewModel()
        {
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _roleRule = new RoleRule();
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        StrName = string.Empty;
                        Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (!loginAcount.LoginNumber.Equals("admin"))
                        {
                            AddVisibility = Visibility.Collapsed;
                            ModifyVisibility = Visibility.Collapsed;
                            DeleteVisibility = Visibility.Collapsed;

                            string roleDetail = loginAcount.Role.RoleDetail;
                            if (roleDetail.Contains(_roleRule.str品质信息管理 + _roleRule.str添加))
                                AddVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str品质信息管理 + _roleRule.str修改))
                                ModifyVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str品质信息管理 + _roleRule.str删除))
                                DeleteVisibility = Visibility.Visible;
                        }
                        else
                        {
                            AddVisibility = Visibility.Visible;
                            ModifyVisibility = Visibility.Visible;
                            DeleteVisibility = Visibility.Visible;
                        }
                        break;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "QualityInfoMainViewModel");
            _rule = new QualityInfoRule();
            _rule.GetQualityInfosCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        QualityInfoList = new ObservableCollection<QualityInfo>(e.Results);
                    }
                };
            _rule.DelQualityInfoCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        Common.MessageBox.Show("删除成功");
                        Dictionary<string, string> queryList = new Dictionary<string, string>();
                        if (!string.IsNullOrWhiteSpace(StrName))
                            queryList.Add("Name", StrName.Trim());
                        _rule.GetQualityInfosAsyns(queryList);
                    }
                };
        }

        #region 界面绑定属性
        private Visibility _addVisibility;
        public Visibility AddVisibility
        {
            get { return _addVisibility; }
            set
            {
                _addVisibility = value;
                this.RaisePropertyChanged("AddVisibility");
            }
        }
        private Visibility _modifyVisibility;
        public Visibility ModifyVisibility
        {
            get { return _modifyVisibility; }
            set
            {
                _modifyVisibility = value;
                this.RaisePropertyChanged("ModifyVisibility");
            }
        }
        private Visibility _deleteVisibility;
        public Visibility DeleteVisibility
        {
            get { return _deleteVisibility; }
            set
            {
                _deleteVisibility = value;
                this.RaisePropertyChanged("DeleteVisibility");
            }
        }
        private ObservableCollection<QualityInfo> _QualityInfoList;
        public ObservableCollection<QualityInfo> QualityInfoList
        {
            get { return _QualityInfoList; }
            set
            {
                _QualityInfoList = value;
                this.RaisePropertyChanged("QualityInfoList");
            }
        }
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
        private string _strName;

        public string StrName
        {
            get { return _strName; }
            set
            {
                _strName = value;
                this.RaisePropertyChanged("StrName");
            }
        }
        private DelegateCommand _cmdAdd;
        public DelegateCommand CmdAdd
        {
            get
            {
                if (_cmdAdd == null)
                {
                    _cmdAdd = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.QualityInfoDetailView,
                            Target = "Sell",
                        });
                        //   GetAccountsAsyns();
                    });
                }
                return _cmdAdd;
            }
            set { _cmdAdd = value; }
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
                        Dictionary<string, string> queryList = new Dictionary<string, string>();
                        if (!string.IsNullOrWhiteSpace(StrName))
                            queryList.Add("Name", StrName.Trim());
                        _rule.GetQualityInfosAsyns(queryList);
                    });
                }
                return _cmdPageLoad;
            }
            set { _cmdPageLoad = value; }
        }

        private DelegateCommand _cmdModify;
        public DelegateCommand CmdModify
        {
            get
            {
                if (_cmdModify == null)
                {
                    _cmdModify = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.QualityInfoDetailView,
                            Target = "Sell",
                            Entity = CurrentQualityInfo,
                        });
                    });
                }
                return _cmdModify;
            }
        }
        private DelegateCommand _cmdDelete;
        public DelegateCommand CmdDelete
        {
            get
            {
                if (_cmdDelete == null)
                {
                    _cmdDelete = new DelegateCommand(() =>
                    {
                        if (Common.MessageBox.Show("确认删除该品质信息?", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                        {
                            _rule.DelQualityInfoAsyns(CurrentQualityInfo);
                        }
                    });
                }
                return _cmdDelete;
            }
        }
        #endregion
    }
}
