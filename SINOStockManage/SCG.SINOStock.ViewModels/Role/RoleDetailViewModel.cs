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
 *   文件名:     RoleDetailViewModel
 *   说明:       
 *   创建时间:   2014/1/13 21:35:01
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class RoleDetailViewModel : ViewModelBase
    {
        private RoleRule _rule = null;
        public RoleDetailViewModel()
        {
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                if (RoleFunctionList == null)
                    LoadRoleList();
                CurrentRole = param.Entity as Role;
                if (CurrentRole == null)
                {
                    CurrentRole = new Role();
                }
                else
                {
                    foreach (var item in RoleFunctionList)
                    {
                        if (CurrentRole.RoleMain.Contains(item.Name))
                            item.IsCheck = true;
                        else
                            item.IsCheck = false;


                        foreach (var detail in item.Children)
                        {
                            if (CurrentRole.RoleDetail.Contains(item.Name + detail.Name))
                                detail.IsCheck = true;
                            else
                                detail.IsCheck = false;
                        }
                    }
                }
            }, ThreadOption.UIThread, true, p => p.Target == "RoleDetailViewModel");
            _rule = new RoleRule();
            _rule.AddRoleCompleted += (s, e) =>
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
                        cmdViewName = CmdViewName.RoleMainView,
                        Target = "Sell",
                    });
                }
            };
            _rule.ModifyRoleCompleted += (s, e) =>
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
                        cmdViewName = CmdViewName.RoleMainView,
                        Target = "Sell",
                    });
                }
            };
        }
       #region 界面绑定属性
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
        private ObservableCollection<PropertyNodeItem> _roleFunctionList;
        public ObservableCollection<PropertyNodeItem> RoleFunctionList
        {
            get { return _roleFunctionList; }
            set
            {
                _roleFunctionList = value;
                this.RaisePropertyChanged("RoleFunctionList");
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
                            cmdViewName = CmdViewName.RoleMainView,
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
                        if (CurrentRole != null)
                        {
                            if (string.IsNullOrWhiteSpace(CurrentRole.RuleName))
                            {
                                Common.MessageBox.Show("角色名不能为空!");
                                return;
                            }
                            string strRoleMain = string.Empty;
                            string strRoleDetail = string.Empty;
                            foreach (PropertyNodeItem item in RoleFunctionList)
                            {
                                string tmpMain = _rule.RoleFunctionAsCode(item.DisplayName);
                                if (item.IsCheck)
                                {
                                    strRoleMain += (tmpMain + ",");
                                }
                                foreach (var detail in item.Children)
                                {
                                    if (detail.IsCheck)
                                    {
                                        strRoleDetail += (tmpMain + _rule.RoleFunctionAsCode(detail.DisplayName) + ",");
                                    }
                                }
                            }
                            CurrentRole.RoleMain = strRoleMain;
                            CurrentRole.RoleDetail = strRoleDetail;
                            if (CurrentRole.ID <= 0)
                            {
                                _rule.AddRoleAsyns(CurrentRole);
                            }
                            else
                            {
                                _rule.ModifyRoleAsyns(CurrentRole);
                            }
                        }
                    });
                }
                return _cmdSave;
            }
        }
        private DelegateCommand<RoutedEventArgs> _cmdCheckOperater;
        /// <summary>
        /// LotNo文本框回车绑定事件
        /// </summary>
        public DelegateCommand<RoutedEventArgs> CmdCheckOperater
        {
            get
            {
                if (_cmdCheckOperater == null)
                {
                    _cmdCheckOperater = new DelegateCommand<RoutedEventArgs>(e =>
                    {
                        Common.MessageBox.Show("a");
                        //var key = e as KeyEventArgs;
                        //if (key == null || key.Key == Key.Enter)//当key为空，或按键为回车键时
                        //{
                        //    if (string.IsNullOrWhiteSpace(LotNo))
                        //    {
                        //        Common.MessageBox.Show("请填写LOT NO");
                        //        return;
                        //    }
                        //    _stockLotRule.GetStockLotEntityByLotNoAsyns(LotNo, 4);
                        //}
                    });
                }
                return _cmdCheckOperater;
            }
        }

        private void LoadRoleList()
        {
            RoleFunctionList = new ObservableCollection<PropertyNodeItem>();
            PropertyNodeItem node账户管理 = new PropertyNodeItem() { DisplayName = "账户管理", IsCheck = false, Name = _rule.str账户管理 };

            node账户管理.Children.Add(new PropertyNodeItem() { DisplayName = "添加", IsCheck = false, Name = _rule.str添加 });
            node账户管理.Children.Add(new PropertyNodeItem() { DisplayName = "修改", IsCheck = false, Name = _rule.str修改 });
            node账户管理.Children.Add(new PropertyNodeItem() { DisplayName = "删除", IsCheck = false, Name = _rule.str删除 });


            PropertyNodeItem node角色管理 = new PropertyNodeItem() { DisplayName = "角色管理", IsCheck = false, Name = _rule.str角色管理 };
            node角色管理.Children.Add(new PropertyNodeItem() { DisplayName = "添加", IsCheck = false, Name = _rule.str添加 });
            node角色管理.Children.Add(new PropertyNodeItem() { DisplayName = "修改", IsCheck = false, Name = _rule.str修改 });
            node角色管理.Children.Add(new PropertyNodeItem() { DisplayName = "删除", IsCheck = false, Name = _rule.str删除 });

            PropertyNodeItem node模版管理 = new PropertyNodeItem() { DisplayName = "模版管理", IsCheck = false, Name = _rule.str模版管理 };
            node模版管理.Children.Add(new PropertyNodeItem() { DisplayName = "添加", IsCheck = false, Name = _rule.str添加 });
            node模版管理.Children.Add(new PropertyNodeItem() { DisplayName = "修改", IsCheck = false, Name = _rule.str修改 });
            node模版管理.Children.Add(new PropertyNodeItem() { DisplayName = "删除", IsCheck = false, Name = _rule.str删除 });




            PropertyNodeItem node扫描入库 = new PropertyNodeItem() { DisplayName = "扫描入库", IsCheck = false, Name = _rule.str扫描入库 };
            node扫描入库.Children.Add(new PropertyNodeItem() { DisplayName = "解除HOLD", IsCheck = false, Name = _rule.str接触HOLD });
            node扫描入库.Children.Add(new PropertyNodeItem() { DisplayName = "入库结束", IsCheck = false, Name = _rule.str入库结束 });

            PropertyNodeItem node扫描入库_无对比 = new PropertyNodeItem() { DisplayName = "扫描入库(无对比)", IsCheck = false, Name = _rule.str扫描入库_无对比 };

            PropertyNodeItem node减薄后检验 = new PropertyNodeItem() { DisplayName = "减薄后检验", IsCheck = false, Name = _rule.str减薄后检验 };
            node减薄后检验.Children.Add(new PropertyNodeItem() { DisplayName = "解除HOLD", IsCheck = false, Name = _rule.str接触HOLD });
            
            PropertyNodeItem node抛光后检验 = new PropertyNodeItem() { DisplayName = "抛光后检验", IsCheck = false, Name = _rule.str抛光后检验 };
            node抛光后检验.Children.Add(new PropertyNodeItem() { DisplayName = "解除HOLD", IsCheck = false, Name = _rule.str接触HOLD });
           
            PropertyNodeItem node镀膜后检验 = new PropertyNodeItem() { DisplayName = "镀膜后检验", IsCheck = false, Name = _rule.str镀膜后检验 };
          //  node镀膜后检验.Children.Add(new PropertyNodeItem() { DisplayName = "解除HOLD", IsCheck = false, Name = _rule.str接触HOLD });
           
            PropertyNodeItem node镀膜后检验_无对比 = new PropertyNodeItem() { DisplayName = "镀膜后检验(无对比)", IsCheck = false, Name = _rule.str镀膜后检验_无对比 };
            PropertyNodeItem node导入GlassID = new PropertyNodeItem() { DisplayName = "新建LotNO", IsCheck = false, Name = _rule.str导入GlassID };
            PropertyNodeItem node查询GlassID = new PropertyNodeItem() { DisplayName = "查询LotNo", IsCheck = false, Name = _rule.str查询GlassID };

            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "入库", IsCheck = false, Name = _rule.str入库 });
            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "入库(无)", IsCheck = false, Name = _rule.str入库_无 });
            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "减薄", IsCheck = false, Name = _rule.str减薄 });
            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "抛光", IsCheck = false, Name = _rule.str抛光 });
            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "镀膜", IsCheck = false, Name = _rule.str镀膜 });
            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "GLASSID明细", IsCheck = false, Name = _rule.strGLASSID明细 });
            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "转移GLASSID", IsCheck = false, Name = _rule.str转移GLASSID });
            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "导出", IsCheck = false, Name = _rule.str导出 });
            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "删除", IsCheck = false, Name = _rule.str删除 });

            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "HOLD", IsCheck = false, Name = _rule.strHOLD });
            node查询GlassID.Children.Add(new PropertyNodeItem() { DisplayName = "解除HOLD", IsCheck = false, Name = _rule.str接触HOLD });


            PropertyNodeItem node品质信息管理 = new PropertyNodeItem() { DisplayName = "品质信息管理", IsCheck = false, Name = _rule.str品质信息管理 };
            node品质信息管理.Children.Add(new PropertyNodeItem() { DisplayName = "添加", IsCheck = false, Name = _rule.str添加 });
            node品质信息管理.Children.Add(new PropertyNodeItem() { DisplayName = "修改", IsCheck = false, Name = _rule.str修改 });
            node品质信息管理.Children.Add(new PropertyNodeItem() { DisplayName = "删除", IsCheck = false, Name = _rule.str删除 });

            PropertyNodeItem nodeGlassID后台 = new PropertyNodeItem() { DisplayName = "GlassID后台", IsCheck = false, Name = _rule.strGLassID后台 };
            nodeGlassID后台.Children.Add(new PropertyNodeItem() { DisplayName = "是否允许修改HOLD", IsCheck = false, Name = _rule.str是否允许修改HOLD });
            nodeGlassID后台.Children.Add(new PropertyNodeItem() { DisplayName = "替换GLASSID", IsCheck = false, Name = _rule.str替换GLASSID });
            nodeGlassID后台.Children.Add(new PropertyNodeItem() { DisplayName = "删除", IsCheck = false, Name = _rule.str删除 });

            RoleFunctionList.Add(node账户管理);
            RoleFunctionList.Add(node角色管理);
            RoleFunctionList.Add(node模版管理);
            RoleFunctionList.Add(node品质信息管理);
            RoleFunctionList.Add(node扫描入库);
            RoleFunctionList.Add(node扫描入库_无对比);
            RoleFunctionList.Add(node减薄后检验);
            RoleFunctionList.Add(node抛光后检验);
            RoleFunctionList.Add(node镀膜后检验);
            //     RoleFunctionList.Add(node镀膜后检验_无对比);
            RoleFunctionList.Add(node导入GlassID);
            RoleFunctionList.Add(node查询GlassID);
            RoleFunctionList.Add(nodeGlassID后台);
        }
        #endregion 
    }
}
