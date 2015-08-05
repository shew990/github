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
/**
 *   命名空间:   SCG.SINOStock.ViewModels.ToolView
 *   文件名:     ToolScanGlassID_StockOutViewModel
 *   说明:       
 *   创建时间:   2014/3/5 2:35:29
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class ToolScanGlassID_StockOutViewModel : ViewModelBase
    {
        private FormworkRule _rule = null;
        public ToolScanGlassID_StockOutViewModel()
        {
            this._eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _rule = new FormworkRule();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        CurrentStockDetail = param.Entity as StockDetail;
                        CurrentStockDetail.DuMoDT = DateTime.Now;
                        IsWu = true;
                        IsDanMian = false;
                        IsShuangMian = false;
                        //CurrentStockDetail.IsPaoGuang = true;

                        if (param.Tag != null && param.Tag.Contains("镀膜"))
                        {
                            IsImgHOLD = true;
                        }
                        else
                            IsImgHOLD = false;

                        string ErrMsg = string.Empty;
                        FormWork fw = _rule.GetFormWorkByProModel(CurrentStockDetail.StockLot.ProModel, ref ErrMsg);
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.New,
                            Entity = fw,
                            Entity1 = CurrentStockDetail.StockLot,
                            //      Tag = param.Tag,
                            Target = "ToolScanGlassID_StockOutView",
                        });
                        break;

                    case CmdName.SendTag:
                        if ((!string.IsNullOrWhiteSpace(param.Tag)) && CurrentStockDetail.StockLot.ImageHOLD.Contains("镀膜"))
                        {
                            if (string.IsNullOrWhiteSpace(param.Entity.ToString()))
                            {
                                Common.MessageBox.Show("当前已勾选品质信息，所以在图形界面中至少需要勾选一个或以上的单元");
                                return;
                            }
                        }

                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.Close,
                            Target = "ToolScanGlassID_StockOutView",
                        });


                        CurrentStockDetail.DuMoImgInfo = param.Entity.ToString();
                        CurrentStockDetail.DuMoInfo = param.Tag;
                        if (IsWu)
                            CurrentStockDetail.IsPaoGuangOverInfo = "无";
                        if (IsDanMian)
                            CurrentStockDetail.IsPaoGuangOverInfo = "单面";
                        if (IsShuangMian)
                            CurrentStockDetail.IsPaoGuangOverInfo = "双面";
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.SaveGlassID,
                            Entity = CurrentStockDetail,
                            Target = "StockOutMainViewModel",
                        });
                        break;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "ToolScanGlassID_StockOutViewModel");
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
        private bool _isWu;
        public bool IsWu
        {
            get { return _isWu; }
            set
            {
                _isWu = value;
                this.RaisePropertyChanged("IsWu");
            }
        }
        private bool _isDanMian;
        public bool IsDanMian
        {
            get { return _isDanMian; }
            set
            {
                _isDanMian = value;
                this.RaisePropertyChanged("IsDanMian");
            }
        }
        private bool _isShuangMian;
        public bool IsShuangMian
        {
            get { return _isShuangMian; }
            set
            {
                _isShuangMian = value;
                this.RaisePropertyChanged("IsShuangMian");
            }
        }
    }
}
