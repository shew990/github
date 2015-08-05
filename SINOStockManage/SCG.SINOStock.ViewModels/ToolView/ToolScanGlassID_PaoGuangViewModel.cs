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
 *   文件名:     ToolScanGlassID_PaoGuangViewModel
 *   说明:       
 *   创建时间:   2014/2/26 10:36:39
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class ToolScanGlassID_PaoGuangViewModel : ViewModelBase
    {
        private FormworkRule _rule = null;
        public ToolScanGlassID_PaoGuangViewModel()
        {
            this._eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _rule = new FormworkRule();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        CurrentStockDetail = param.Entity as StockDetail;
                        CurrentStockDetail.PaoGuangDT = DateTime.Now;
                        CurrentStockDetail.IsFanGong = false;
                        //CurrentStockDetail.IsPaoGuang = true;

                        if (param.Tag != null && param.Tag.Contains("抛光"))
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
                            Target = "ToolScanGlassID_PaoGuangView",
                        });
                        break;

                    case CmdName.SendTag:
                        if (string.IsNullOrWhiteSpace(CurrentStockDetail.PaoGuangNum))
                        {
                            Common.MessageBox.Show("必须输入抛光机号");
                            return;
                        }

                        if ((!string.IsNullOrWhiteSpace(param.Tag)) && CurrentStockDetail.StockLot.ImageHOLD.Contains("抛光"))
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
                            Target = "ToolScanGlassID_PaoGuangView",
                        });

                        CurrentStockDetail.IsHOLD = IsHOLD;
                        CurrentStockDetail.PaoGuangImgInfo = param.Entity.ToString();
                        CurrentStockDetail.PaoGuangInfo = param.Tag;
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.SaveGlassID,
                            Entity = CurrentStockDetail,
                            Target = "Process_PaoGuangViewModel",
                        });
                        break;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "ToolScanGlassID_PaoGuangViewModel");
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
    }
}
