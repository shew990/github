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
/**
 *   命名空间:   SCG.SINOStock.ViewModels.ToolView
 *   文件名:     ToolScanGlassID_JianBaoViewModel
 *   说明:       
 *   创建时间:   2014/2/24 1:47:30
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class ToolScanGlassID_JianBaoViewModel : ViewModelBase
    {
        private FormworkRule _rule = null;
        public ToolScanGlassID_JianBaoViewModel()
        {
            this._eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _rule = new FormworkRule();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {

                switch (param.cmdName)
                {
                    case CmdName.New:
                        CurrentStockDetail = param.Entity as StockDetail;
                        CurrentStockDetail.JianBaoDT = DateTime.Now;
                        CurrentStockDetail.IsPaoGuang = true;
                        if (param.Tag != null && param.Tag.Contains("减薄"))
                        {
                            IsImgHOLD = true;
                        }
                        else
                            IsImgHOLD = false;
                        IsHOLD = false;
                        string ErrMsg = string.Empty;
                        FormWork fw = _rule.GetFormWorkByProModel(CurrentStockDetail.StockLot.ProModel, ref ErrMsg);
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.New,
                            Entity = fw,
                            Entity1 = CurrentStockDetail.StockLot,
                            //      Tag = param.Tag,
                            Target = "ToolScanGlassID_JianBaoView",
                        });
                        break;

                    case CmdName.SendTag:
                        if (string.IsNullOrWhiteSpace(CurrentStockDetail.JianBaoNum))
                        {
                            Common.MessageBox.Show("必须输入减薄槽号");
                            return;
                        }
                        //如果勾选了品质信息和LOT 具有图片减薄品质信息开关
                        if ((!string.IsNullOrWhiteSpace(param.Tag)) && CurrentStockDetail.StockLot.ImageHOLD.Contains("减薄"))
                        {
                            if (string.IsNullOrWhiteSpace(param.Entity.ToString()))
                            {
                                Common.MessageBox.Show("当前已勾选品质信息，所以在图形界面中至少需要勾选一个或以上的单元");
                                return;
                            }
                        }
                        //CF面判断，必须选择至少一项
                        if (CurrentStockDetail.IsPaoGuang)
                        {
                            if ((!IsTFT) && (!IsCF))
                            {
                                Common.MessageBox.Show("抛光面至少需要选择一项");
                                return;
                            }
                        }
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                       {
                           cmdName = CmdName.Close,
                           Target = "ToolScanGlassID_JianBaoView",
                       });

                        CurrentStockDetail.IsHOLD = IsHOLD;
                        CurrentStockDetail.JianBaoImgInfo = param.Entity.ToString();
                        CurrentStockDetail.JianBaoInfo = param.Tag;
                        if (CurrentStockDetail.IsPaoGuang)
                        {
                            if (IsFuFei)
                                CurrentStockDetail.PaoguangType = "付费抛光";
                            else
                                CurrentStockDetail.PaoguangType = "制程抛光";
                            string strPaoGuangMian = string.Empty;//抛光面
                            if (IsTFT)
                                strPaoGuangMian += "TFT面,";
                            if (IsCF)
                                strPaoGuangMian += "CF面";

                            CurrentStockDetail.PaoGuangMian = strPaoGuangMian;
                        }
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.SaveGlassID,
                            Entity = CurrentStockDetail,
                            Target = "Process_JianBaoViewModel",
                        });
                        break;
                }

                //if (CurrentStockLot != null)
                //{
                //    if (CurrentStockLot.DetailInfoHOLD != null)
                //        ControlsEnabled = CurrentStockLot.DetailInfoHOLD.Contains("入库");
                //    else
                //        ControlsEnabled = false;

                //    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                //    {
                //        cmdName = CmdName.New,
                //        Target = "ToolScanGlassID_StockInView",
                //    });
                //}
            }, ThreadOption.UIThread, true, p => p.Target == "ToolScanGlassID_JianBaoViewModel");
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
        private string _checkResult;

        public string CheckResult
        {
            get { return _checkResult; }
            set
            {
                _checkResult = value;
                this.RaisePropertyChanged("CheckResult");
            }
        }
        private DateTime _currentDt = DateTime.Now;
        public DateTime CurrentDt
        {
            get { return _currentDt; }
            set
            {

                _currentDt = value;
                this.RaisePropertyChanged("CurrentDt");
            }
        }
        private bool _controlsEnabled;
        public bool ControlsEnabled
        {
            get { return _controlsEnabled; }
            set
            {
                _controlsEnabled = value;
                this.RaisePropertyChanged("ControlsEnabled");
            }
        }
        private bool _isHOLD;
        public bool IsHOLD
        {
            get { return _isHOLD; }
            set
            {
                _isHOLD = value;
                this.RaisePropertyChanged("IsHOLD");
            }
        }
        private bool _isImgHOLD;
        public bool IsImgHOLD
        {
            get { return _isImgHOLD; }
            set
            {
                _isImgHOLD = value;
                this.RaisePropertyChanged("IsImgHOLD");
            }
        }

        private bool _isAllCheck;
        /// <summary>
        /// 品质信息选择
        /// </summary>
        public bool IsAllCheck
        {
            get { return _isAllCheck; }
            set
            {
                _isAllCheck = value;
                this.RaisePropertyChanged("IsAllCheck");
            }
        }




        #region 抛光种类和抛光面
        private bool _isZhiCheng;
        /// <summary>
        /// 制程抛光
        /// </summary>
        public bool IsZhiCheng
        {
            get { return _isZhiCheng; }
            set
            {
                _isZhiCheng = value;
                this.RaisePropertyChanged("IsZhiCheng");
            }
        }
        private bool _isFuFei;
        /// <summary>
        /// 付费抛光
        /// </summary>
        public bool IsFuFei
        {
            get { return _isFuFei; }
            set
            {
                _isFuFei = value;
                this.RaisePropertyChanged("IsFuFei");
            }
        }
        private bool _isTFT;
        /// <summary>
        ///TFT抛光面
        /// </summary>
        public bool IsTFT
        {
            get { return _isTFT; }
            set
            {
                _isTFT = value;
                this.RaisePropertyChanged("IsTFT");
            }
        }
        private bool _isCF;
        /// <summary>
        /// CF抛光面
        /// </summary>
        public bool IsCF
        {
            get { return _isCF; }
            set
            {
                _isCF = value;
                this.RaisePropertyChanged("IsCF");
            }
        }
        #endregion
        private DelegateCommand _cmdSave;
        public DelegateCommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new DelegateCommand(() =>
                    {

                    });
                }
                return _cmdSave;
            }
        }
        private DelegateCommand _cmdAllCheck;
        public DelegateCommand CmdAllCheck
        {
            get
            {
                if (_cmdAllCheck == null)
                {
                    _cmdAllCheck = new DelegateCommand(() =>
                    {
                        Common.MessageBox.Show(IsAllCheck.ToString());
                    });
                }
                return _cmdAllCheck;
            }
        }
    }
}
