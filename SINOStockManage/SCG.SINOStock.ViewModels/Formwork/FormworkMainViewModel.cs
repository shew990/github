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
 *   命名空间:   SCG.SINOStock.ViewModels.Formwork
 *   文件名:     FormworkMainViewModel
 *   说明:       模版管理viewmodel
 *   创建时间:   2014/1/14 15:42:18
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class FormworkMainViewModel : ViewModelBase
    {
        private FormworkRule _rule = null;
        private RoleRule _roleRule;
        public FormworkMainViewModel()
        {
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _roleRule = new RoleRule();
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        StrProduct = string.Empty;
                        Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (!loginAcount.LoginNumber.Equals("admin"))
                        {
                            AddVisibility = Visibility.Collapsed;
                            ModifyVisibility = Visibility.Collapsed;
                            DeleteVisibility = Visibility.Collapsed;

                            string roleDetail = loginAcount.Role.RoleDetail;
                            if (roleDetail.Contains(_roleRule.str模版管理 + _roleRule.str添加))
                                AddVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str模版管理 + _roleRule.str修改))
                                ModifyVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str模版管理 + _roleRule.str删除))
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
            }, ThreadOption.UIThread, true, p => p.Target == "FormworkMainViewModel");
            _rule = new FormworkRule();
            _rule.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("IsBusy"))
                {
                    IsBusy = _rule.IsBusy;
                }
            };
            _rule.GetFormworkListCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    FormworkCollection = new ObservableCollection<FormWork>(e.Results);
                }
            };
            _rule.DelFormworkCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        Common.MessageBox.Show("删除成功");
                        Dictionary<string, string> queryList = new Dictionary<string, string>();
                        if (!string.IsNullOrWhiteSpace(StrProduct))
                            queryList.Add("Product", StrProduct.Trim());
                        _rule.GetFormWorkListAsyns(queryList);
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
        private string _strProduct;

        public string StrProduct
        {
            get { return _strProduct; }
            set
            {
                _strProduct = value;
                this.RaisePropertyChanged("StrProduct");
            }
        }
        private ObservableCollection<FormWork> _formworkCollection;
        public ObservableCollection<FormWork> FormworkCollection
        {
            get { return _formworkCollection; }
            set
            {
                if (!ReferenceEquals(_formworkCollection, value))
                {
                    _formworkCollection = value;
                    this.RaisePropertyChanged("FormworkCollection");
                }
            }
        }

        private FormWork _currentFormwork;
        public FormWork CurrentFormwork
        {
            get { return _currentFormwork; }
            set
            {
                _currentFormwork = value;
                this.RaisePropertyChanged("CurrentFormwork");
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
                        Dictionary<string, string> queryList = new Dictionary<string, string>();
                        if (!string.IsNullOrWhiteSpace(StrProduct))
                            queryList.Add("Product", StrProduct.Trim());
                        _rule.GetFormWorkListAsyns(queryList);
                    });
                }
                return _cmdPageLoad;
            }
            set { _cmdPageLoad = value; }
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
                            cmdViewName = CmdViewName.FormworkDetailView,
                            Entity = null,
                            Target = "Sell",
                        });
                        //   GetAccountsAsyns();
                    });
                }
                return _cmdAdd;
            }
            set { _cmdAdd = value; }
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
                            cmdViewName = CmdViewName.FormworkDetailView,
                            Entity = CurrentFormwork,
                            Target = "Sell",
                        });
                        //   GetAccountsAsyns();
                    });
                }
                return _cmdModify;
            }
            set { _cmdModify = value; }
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
                        if (Common.MessageBox.Show("确认删除该模版?", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                        {
                            _rule.DelFormworkAsyns(CurrentFormwork);
                        }
                    });
                }
                return _cmdDelete;
            }
        }
        #endregion
    }
}
