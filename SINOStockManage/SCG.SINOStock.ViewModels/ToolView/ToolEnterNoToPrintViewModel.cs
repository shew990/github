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
/**
 *   命名空间:   SCG.SINOStock.ViewModels.ToolView
 *   文件名:     ToolEnterNoToPrintViewModel
 *   说明:       
 *   创建时间:   2014/2/26 16:24:20
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class ToolEnterNoToPrintViewModel : ViewModelBase
    {
        private StockBoxRule _stockboxRule;
        private TrayRule _trayRule;
        private bool _IsQiangDa;
        private string _TrayID;
        private bool _isTuoHao;//是否为托号


        public ToolEnterNoToPrintViewModel()
        {
            this._eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _stockboxRule = new StockBoxRule();
            _trayRule = new TrayRule();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                CurrentPrintType = (PrintType)param.Entity;

                int LotID = Convert.ToInt32(param.Tag);
                string ErrMsg = string.Empty;
                IsMessageEnable = false;
                CancelVisibility = Visibility.Collapsed;
                txtIsEnabled = false;
                switch (CurrentPrintType)
                {
                    case PrintType.Box:
                        Title = "打印 BOX ID标签";
                        Head = "请输入BOX ID标签";
                        _isTuoHao = false;
                      //  CurrentStockBox = _stockboxRule.GetMaxStockBox_Ex(ref ErrMsg);
                        CurrentStockBox = param.Entity1 as StockBox;
                        Content = CurrentStockBox.BarCode;
                        break;
                    case PrintType.Tray:
                        Title = "打印 托号标签";
                        Head = "请输入托号标签";
                        _isTuoHao = true;
                        _IsQiangDa = false;
                        Content = _trayRule.GetTrayMaxBarCode(LotID, ref ErrMsg);
                        _TrayID = Content;
                        IsMessageEnable = true;
                        break;
                    case PrintType.ForceBox:
                        Title = "打印 BOX ID标签[强制打印]";
                        Head = "请输入BOX ID标签";
                        _isTuoHao = false;
                      //  CurrentStockBox = _stockboxRule.GetMaxStockBox_Ex(ref ErrMsg);
                        CurrentStockBox = param.Entity1 as StockBox;
                        Content = CurrentStockBox.BarCode;
                        CancelVisibility = Visibility.Visible;
                        break;
                    case PrintType.ForceTray:
                        Title = "打印 托号标签[强制打印]";
                        Head = "请输入托号标签";
                        _isTuoHao = true;
                        Content = _trayRule.GetTrayMaxBarCode(LotID, ref ErrMsg);
                        _TrayID = Content;
                        CancelVisibility = Visibility.Visible;
                        _IsQiangDa = true;
                        break;
                    case PrintType.AgainBox:
                        Title = "打印 托号标签[补打打印]";
                        Head = "请输入BOX ID标签";
                        CancelVisibility = Visibility.Visible;
                        break;
                    case PrintType.AgainTray:
                        Title = "打印 托号标签[补打打印]";
                        Head = "请输入托号标签";
                        CancelVisibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "ToolEnterNoToPrintViewModel");
        }

        private PrintType _currentPrintType;
        public PrintType CurrentPrintType
        {
            get { return _currentPrintType; }
            set
            {
                _currentPrintType = value;
                this.RaisePropertyChanged("CurrentPrintType");
            }
        }
        private StockBox _currentStockBox;
        public StockBox CurrentStockBox
        {
            get { return _currentStockBox; }
            set
            {
                _currentStockBox = value;
                this.RaisePropertyChanged("CurrentStockBox");
            }
        }
        private bool _isMessageEnable;
        public bool IsMessageEnable
        {
            get { return _isMessageEnable; }
            set
            {
                _isMessageEnable = value;
                this.RaisePropertyChanged("IsMessageEnable");
            }
        }
        private bool _txtIsEnabled;
        public bool txtIsEnabled
        {
            get { return _txtIsEnabled; }
            set
            {
                _txtIsEnabled = value;
                this.RaisePropertyChanged("txtIsEnabled");
            }
        }
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.RaisePropertyChanged("Title");
            }
        }
        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                this.RaisePropertyChanged("Message");
            }
        }
        private string _head;

        public string Head
        {
            get { return _head; }
            set
            {
                _head = value;
                this.RaisePropertyChanged("Head");
            }
        }
        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                this.RaisePropertyChanged("Content");
            }
        }
        private Visibility _cancelVisibility;
        public Visibility CancelVisibility
        {
            get { return _cancelVisibility; }
            set
            {
                _cancelVisibility = value;
                this.RaisePropertyChanged("CancelVisibility");
            }
        }
        private DelegateCommand _cmdOK;
        public DelegateCommand CmdOK
        {
            get
            {
                if (_cmdOK == null)
                {
                    _cmdOK = new DelegateCommand(() =>
                    {
                        btnPrint_Click();
                      
                    });
                }
                return _cmdOK;
            }
        }
        private DelegateCommand _cmdCancel;
        public DelegateCommand CmdCancel
        {
            get
            {
                if (_cmdCancel == null)
                {
                    _cmdCancel = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.CancelPrint,
                            // Entity = _currentStockBox,
                            //      Tag = param.Tag,
                            Target = "StockOutMainView",
                        });
                    });
                }
                return _cmdCancel;
            }
        }
        private DelegateCommand _cmdEnabled;
        public DelegateCommand CmdEnabled
        {
            get
            {
                if (_cmdEnabled == null)
                {
                    _cmdEnabled = new DelegateCommand(() =>
                    {
                        //if (_isTuoHao)
                        //{
                        //    MessageBox.Show("托号不允许修改");
                        //    return;
                        //}
                        txtIsEnabled = true;
                    });
                }
                return _cmdEnabled;
            }
        }
        #region 私有方法
        private void btnPrint_Click()
        {
            //   string strBarCode = txtBarCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(Content))
            {
                switch (CurrentPrintType)
                {
                    case PrintType.Box:
                        Common.MessageBox.Show("请输入Box ID标签");
                        return;
                    case PrintType.Tray:
                        Common.MessageBox.Show("请输入托号标签");
                        return;
                }

            }
            string ErrMsg = string.Empty;
            switch (CurrentPrintType)
            {
                case PrintType.Box:
                    if (_currentStockBox.BarCode != Content)
                    {

                        //StockBox sb = _stockboxRule.ChangeBoxBarCode(Content, ref ErrMsg);
                        StockBox sb = _stockboxRule.ChangeBoxBarCode_Pro(Content, _currentStockBox.BarCode, ref ErrMsg);

                        if (sb == null)
                        {
                            Common.MessageBox.Show(ErrMsg);
                            return;
                        }
                        else
                        {
                            _currentStockBox = sb;
                        }
                    }

                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.SendPrintData_Box,
                        Entity = _currentStockBox,
                        //      Tag = param.Tag,
                        Target = "StockOutMainViewModel",
                    });

                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.CancelPrint,
                        // Entity = _currentStockBox,
                        //      Tag = param.Tag,
                        Target = "StockOutMainView",
                    });
                    return;
                case PrintType.Tray:
                    if (_TrayID != Content)
                    {
                        if (!_trayRule.ChangeTryBarCode(Content, ref ErrMsg))
                        {
                            Common.MessageBox.Show(ErrMsg);
                            return;
                        }
                    }
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.SendPrintData_Tray,
                        Entity = Content,
                        //      Tag = param.Tag,
                        Target = "StockOutMainViewModel",
                        IsQiangDa = _IsQiangDa,
                    });
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.CancelPrint,
                        // Entity = _currentStockBox,
                        //      Tag = param.Tag,
                        Target = "StockOutMainView",
                    });
                    return;
                case PrintType.ForceBox:

                    if (_currentStockBox.BarCode != Content)
                    {
                       // string ErrMsg = string.Empty;
                        //StockBox sb = _stockboxRule.ChangeBoxBarCode(Content, ref ErrMsg);
                        StockBox sb = _stockboxRule.ChangeBoxBarCode_Pro(Content, _currentStockBox.BarCode, ref ErrMsg);
                        if (sb == null)
                        {
                            Common.MessageBox.Show(ErrMsg);
                            return;
                        }
                        else
                        {
                            _currentStockBox = sb;
                        }
                    }
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.SendPrintData_Box,
                        Entity = _currentStockBox,
                        //      Tag = param.Tag,
                        Target = "StockOutMainViewModel",
                    });
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.CancelPrint,
                        // Entity = _currentStockBox,
                        //      Tag = param.Tag,
                        Target = "StockOutMainView",
                    });
                    break;
                case PrintType.ForceTray:
                    if (_TrayID != Content)
                    {
                        if (!_trayRule.ChangeTryBarCode(Content, ref ErrMsg))
                        {
                            Common.MessageBox.Show(ErrMsg);
                            return;
                        }
                    }
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.SendPrintData_Tray,
                        Entity = Content,
                        //      Tag = param.Tag,
                        Target = "StockOutMainViewModel",
                        IsQiangDa = _IsQiangDa,
                    });
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.CancelPrint,
                        // Entity = _currentStockBox,
                        //      Tag = param.Tag,
                        Target = "StockOutMainView",
                    });
                    break;

            }

        }

        #endregion
    }
    public enum PrintType
    {
        Box,//箱
        Tray,//托

        ForceBox,//强制箱
        ForceTray,//强制托
        AgainBox,//重打箱
        AgainTray,//重打托
    }
}
