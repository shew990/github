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
 *   命名空间:   SCG.SINOStock.ViewModels.Role
 *   文件名:     RoleMainViewModel
 *   说明:       角色管理ViewModel类
 *   创建时间:   2014/1/13 13:07:43
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class RoleMainViewModel : ViewModelBase
    {
        private RoleRule _rule = null;
        public RoleMainViewModel()
        {
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
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
                            if (roleDetail.Contains(_rule.str角色管理 + _rule.str添加))
                                AddVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_rule.str角色管理 + _rule.str修改))
                                ModifyVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_rule.str角色管理 + _rule.str删除))
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
            }, ThreadOption.UIThread, true, p => p.Target == "RoleMainViewModel");
            _rule = new RoleRule();
            _rule.GetRolesCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        RoleList = new ObservableCollection<Role>(e.Results);
                        _lineNumber = 1;
                    }
                };
            _rule.DelRoleCompleted += (s, e) =>
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
                        _rule.GetRolesAsyns(queryList);
                     
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
        private ObservableCollection<Role> _roleList;
        public ObservableCollection<Role> RoleList
        {
            get { return _roleList; }
            set
            {
                _roleList = value;
                this.RaisePropertyChanged("RoleList");
            }
        }
        private Role _currentRole;
        public Role CurrentRole
        {
            get { return _currentRole; }
            set
            {
                _currentRole = value;
                this.RaisePropertyChanged("CurrentRole");
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
        private int _lineNumber = 1;
        public int LineNumber
        {
            get { return _lineNumber++; }
            set
            {
                _lineNumber = value;
                this.RaisePropertyChanged("LineNumber");
            }
        }
        private DelegateCommand _cmdSorting;
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand CmdSorting
        {
            get
            {
                if (_cmdSorting == null)
                {
                    _cmdSorting = new DelegateCommand(() =>
                    {
                        _lineNumber = 1;
                    });
                }
                return _cmdSorting;
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
                            cmdViewName = CmdViewName.RoleMainDetailView,
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
                        _rule.GetRolesAsyns(queryList);
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
                            cmdViewName = CmdViewName.RoleMainDetailView,
                            Target = "Sell",
                            Entity = CurrentRole,
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
                        if (Common.MessageBox.Show("确认删除该角色?", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                        {
                            _rule.DelRoleAsyns(CurrentRole);
                        }
                    });
                }
                return _cmdDelete;
            }
        }
        #endregion
    }
}
