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
using System.Windows.Input;
/**
 *   命名空间:   SCG.SINOStock.ViewModels.StockIn
 *   文件名:     ModifyGlassIDViewModel
 *   说明:       
 *   创建时间:   2014/3/11 14:09:38
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class ModifyGlassIDViewModel : ViewModelBase
    {
        private StockLotRule _rule = null;
        private RoleRule _roleRule = null;
        public ModifyGlassIDViewModel()
        {

            _rule = new StockLotRule();
            _roleRule = new RoleRule();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        CurrentStockDetail = new StockDetail();
                        ReadComTest();
                        Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (!loginAcount.LoginNumber.Equals("admin"))
                        {
                            HOLDIsEnabled = false;
                            DeleteVisibility = Visibility.Collapsed;
                            ReplaceVisibility = Visibility.Collapsed;


                            string roleDetail = loginAcount.Role.RoleDetail;
                            if (roleDetail.Contains(_roleRule.strGLassID后台 + _roleRule.str是否允许修改HOLD))
                                HOLDIsEnabled = true;
                            if (roleDetail.Contains(_roleRule.strGLassID后台 + _roleRule.str删除))
                                DeleteVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.strGLassID后台 + _roleRule.str替换GLASSID))
                                ReplaceVisibility = Visibility.Visible;
                        }
                        else
                        {
                            HOLDIsEnabled = true;
                            DeleteVisibility = Visibility.Visible;
                            ReplaceVisibility = Visibility.Visible;
                        }
                        break;


                    case CmdName.SaveGlassID:
                        CurrentStockDetail.GlassID = param.Entity.ToString();
                        ReadComTest();
                        break;

                    default:
                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "ModifyGlassIDViewModel");
            _rule.ModifyStockDetailListCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        Common.MessageBox.Show("修改成功");
                    }
                };
            ScanComExecute = (s, e) =>
            {
                string ErrMsg = string.Empty;
                CurrentStockDetail.GlassID = s.ToString();

                var tmp = _rule.GetStockDetailByGlassID(CurrentStockDetail.GlassID, ref ErrMsg);
                if (tmp == null)
                {
                    Common.MessageBox.Show("不存在该GlassID");
                    CurrentStockDetail.GlassID = string.Empty;
                    isDispose = false;
                    return;
                }
                CurrentStockDetail = tmp;
                isDispose = false;
                //    _stockLotRule.UpdateStockDetailStatusAsyns(GlassID, CurrentStockLot.ID, 2);//todo是否验证写死了
                //   AddStockDetailAsyns();
            };
        }
        private StockDetail _currentStockDetail;
        public StockDetail CurrentStockDetail
        {
            get { return _currentStockDetail; }
            set
            {
                _currentStockDetail = value;
                this.RaisePropertyChanged("CurrentStockDetail");
            }
        }
        private bool _hOLDIsEnabled;
        public bool HOLDIsEnabled
        {
            get { return _hOLDIsEnabled; }
            set
            {
                _hOLDIsEnabled = value;
                this.RaisePropertyChanged("HOLDIsEnabled");
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
        private Visibility _replaceVisibility;
        public Visibility ReplaceVisibility
        {
            get { return _replaceVisibility; }
            set
            {
                _replaceVisibility = value;
                this.RaisePropertyChanged("ReplaceVisibility");
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
                        string ErrMsg = string.Empty;
                        if (key == null || key.Key == Key.Enter)//当key为空，或按键为回车键时
                        {
                            if (!string.IsNullOrWhiteSpace(CurrentStockDetail.GlassID))
                            {
                                var tmp = _rule.GetStockDetailByGlassID(CurrentStockDetail.GlassID, ref ErrMsg);
                                if (tmp == null)
                                {
                                    Common.MessageBox.Show("不存在该GlassID");
                                    return;
                                }
                                CurrentStockDetail = tmp;

                            }
                        }
                    });
                }
                return _cmdDetailOperater;
            }
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

                        if (CurrentStockDetail != null)
                        {
                            if (string.IsNullOrWhiteSpace(CurrentStockDetail.GlassID) || CurrentStockDetail.ID <= 0)
                            {
                                Common.MessageBox.Show("请扫描GlssID");
                                return;
                            }
                            _rule.IsHouTai = true;
                            _rule.ModifyStockDetailListAsyns(CurrentStockDetail);
                        }
                    });
                }
                return _cmdSave;
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

                        if (CurrentStockDetail != null)
                        {
                            if (string.IsNullOrWhiteSpace(CurrentStockDetail.GlassID) || CurrentStockDetail.ID <= 0)
                            {
                                Common.MessageBox.Show("请扫描GlssID");
                                return;
                            }
                            string ErrMsg = string.Empty;
                            if (Common.MessageBox.Show("确认删除该GlassID?", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                            {
                                if (_rule.DeleteStockDetail(CurrentStockDetail, ref ErrMsg))
                                {
                                    Common.MessageBox.Show("删除成功");
                                    CurrentStockDetail = new StockDetail();
                                }
                                else
                                {
                                    Common.MessageBox.Show(ErrMsg);
                                }
                            }
                        }
                    });
                }
                return _cmdDelete;
            }
        }
        private DelegateCommand _cmdReplaceGlassID;
        public DelegateCommand CmdReplaceGlassID
        {
            get
            {
                if (_cmdReplaceGlassID == null)
                {
                    _cmdReplaceGlassID = new DelegateCommand(() =>
                    {
                        //我说要娶穿碎花衬衫的你
                        if (CurrentStockDetail == null || CurrentStockDetail.ID <= 0)
                        {
                            Common.MessageBox.Show("请先扫描GlassID");
                            return;
                        }
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.ToolReplaceGlassID,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdReplaceGlassID;
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
                            cmdViewName = CmdViewName.MainView,
                            Target = "Sell",
                        });
                        //   GetAccountsAsyns();
                    });
                }
                return _cmdGotoList;
            }
            set { _cmdGotoList = value; }
        }
    }

}
