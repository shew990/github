using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ViewModels.ToolView
 *   文件名:     ToolScanGlassID_StockInViewModel
 *   说明:       
 *   创建时间:   2014/2/23 16:34:04
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class ToolScanGlassID_StockInViewModel : ViewModelBase
    {
        public ToolScanGlassID_StockInViewModel()
        {
            this._eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        CurrentStockLot = param.Entity as StockLot;
                        if (CurrentStockLot != null)
                        {
                            if (CurrentStockLot.DetailInfoHOLD != null)
                                ControlsEnabled = CurrentStockLot.DetailInfoHOLD.Contains("入库");
                            else
                                ControlsEnabled = false;

                            _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                            {
                                cmdName = CmdName.New,
                                Entity = CurrentStockLot,
                                Target = "ToolScanGlassID_StockInView",
                            });
                        }
                        IsHOLD = false;
                        break;
                    case CmdName.SendTag:
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                       {
                           cmdName = CmdName.Close,
                           Target = "ToolScanGlassID_StockInView",
                       });
                        StockDetail detail = new StockDetail();
                        detail.IsHOLD = IsHOLD;
                        detail.StockInInfo = param.Tag;
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.SaveGlassID,
                            Entity = detail,
                            Target = "StockInMainViewModel",
                        });
                        break;
                    default:
                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "ToolScanGlassID_StockInViewModel");

        }

        private StockLot _currentStockLot;

        public StockLot CurrentStockLot
        {
            get { return _currentStockLot; }
            set
            {
                _currentStockLot = value;
                this.RaisePropertyChanged("CurrentStockLot");
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
        private DelegateCommand _cmdSave;
        public DelegateCommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.Close,
                            Target = "ToolScanGlassID_StockInView",
                        });
                        StockDetail detail = new StockDetail();
                        detail.IsHOLD = IsHOLD;
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.SaveGlassID,
                            Entity = detail,
                            Target = "StockInMainViewModel",
                        });
                    });
                }
                return _cmdSave;
            }
        }
    }
}
