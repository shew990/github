using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.SINOStock.ServiceRule;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using SCG.SINOStock.WCFService.SINOStockService;

/**
 *   命名空间:   SCG.SINOStock.ViewModels.ToolView
 *   文件名:     ImportStockLotViewModel
 *   说明:       
 *   创建时间:   2014/2/10 14:37:52
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class ImportStockLotViewModel : ViewModelBase
    {
        private StockLotRule _rule = null;
        private FormworkRule _formworkRule = null;
        public ImportStockLotViewModel()
        {
            _rule = new StockLotRule();
            _formworkRule = new FormworkRule();
            _rule.AddStockLotAndDetailsCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        Common.MessageBox.Show("新建LotNo成功！");
                        GuanKongMulti = false;
                        GuanKongSingle = false;
                    }
                };
            _formworkRule.GetFormworkListCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    FormWorkCollection = new ObservableCollection<string>(e.Results.Select(p => p.ProductModel));
                }
            };
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {

            }, ThreadOption.UIThread, true, p => p.Target == "ImportStockLotViewModel");
        }
        private ObservableCollection<string> _formworkCollection;
        public ObservableCollection<string> FormWorkCollection
        {
            get { return _formworkCollection; }
            set
            {
                _formworkCollection = value;
                this.RaisePropertyChanged("FormWorkCollection");
            }
        }
        private string _currentFormwork;
        public string CurrentFormwork
        {
            get { return _currentFormwork; }
            set
            {
                _currentFormwork = value;
                this.RaisePropertyChanged("CurrentFormwork");
            }
        }
        private string _lotNo;
        public string LotNo
        {
            get { return _lotNo; }
            set
            {
                _lotNo = value;
                this.RaisePropertyChanged("LotNo");
            }
        }
        private bool _isImport;
        /// <summary>
        /// 是否为导入（客户列表）
        /// </summary>
        public bool IsImport
        {
            get { return _isImport; }
            set
            {
                _isImport = value;
                this.RaisePropertyChanged("IsImport");
            }
        }
        private bool _isFormwork;
        /// <summary>
        /// 入库方式是否为关键字模版
        /// </summary>
        public bool IsFormwork
        {
            get { return _isFormwork; }
            set
            {
                _isFormwork = value;
                this.RaisePropertyChanged("IsFormwork");

            }
        }

        private int _stockInQty;
        /// <summary>
        /// 需要入库的数量
        /// </summary>
        public int StockInQty
        {
            get { return _stockInQty; }
            set
            {
                _stockInQty = value;
                this.RaisePropertyChanged("StockInQty");
            }
        }
        private bool _isJianBao;
        /// <summary>
        /// 工艺流程：是否有减薄工序
        /// </summary>
        public bool IsJianBao
        {
            get { return _isJianBao; }
            set
            {
                _isJianBao = value;
                this.RaisePropertyChanged("IsJianBao");
            }
        }
        private bool _isDuMo;
        /// <summary>
        /// 工艺流程：是否有镀膜工序
        /// </summary>
        public bool IsDuMo
        {
            get { return _isDuMo; }
            set
            {
                _isDuMo = value;
                this.RaisePropertyChanged("IsDuMo");
            }
        }
        #region 品质信息绑定字段
        private bool _stockInHOLD;
        /// <summary>
        /// 品质信息：入库
        /// </summary>
        public bool StockInHOLD
        {
            get { return _stockInHOLD; }
            set
            {
                _stockInHOLD = value;
                this.RaisePropertyChanged("StockInHOLD");
            }
        }
        private bool _jianbaoHOLD;
        /// <summary>
        /// 品质信息：减薄
        /// </summary>
        public bool JianBaoHOLD
        {
            get { return _jianbaoHOLD; }
            set
            {
                _jianbaoHOLD = value;
                this.RaisePropertyChanged("JianBaoHOLD");
            }
        }

        private bool _paoGuangHOLD;
        /// <summary>
        /// 品质信息：抛光
        /// </summary>
        public bool PaoGuangHOLD
        {
            get { return _paoGuangHOLD; }
            set
            {
                _paoGuangHOLD = value;
                this.RaisePropertyChanged("PaoGuangHOLD");
            }
        }
        private bool _duMoHOLD;
        /// <summary>
        /// 品质信息：镀膜
        /// </summary>
        public bool DuMoHOLD
        {
            get { return _duMoHOLD; }
            set
            {
                _duMoHOLD = value;
                this.RaisePropertyChanged("DuMoHOLD");
            }
        }
        #endregion
        #region  管控
        private bool _guanKongSingle;
        /// <summary>
        /// 管控：单独　不能混批
        /// </summary>
        public bool GuanKongSingle
        {
            get { return _guanKongSingle; }
            set {
                _guanKongSingle = value;
                this.RaisePropertyChanged("GuanKongSingle");
            }
        }
        private bool _guanKongMulti;
        /// <summary>
        /// 管控：可混批
        /// </summary>
        public bool GuanKongMulti
        {
            get { return _guanKongMulti; }
            set
            {
                _guanKongMulti = value;
                this.RaisePropertyChanged("GuanKongMulti");
            }
        }
        #endregion


        #region 品质信息绑定字段

        private bool _jianbaoImgHOLD;
        /// <summary>
        /// 图形选择开关：减薄
        /// </summary>
        public bool JianBaoImgHOLD
        {
            get { return _jianbaoImgHOLD; }
            set
            {
                _jianbaoImgHOLD = value;
                this.RaisePropertyChanged("JianBaoImgHOLD");
            }
        }

        private bool _paoGuangImgHOLD;
        /// <summary>
        /// 图形选择开关：抛光
        /// </summary>
        public bool PaoGuangImgHOLD
        {
            get { return _paoGuangImgHOLD; }
            set
            {
                _paoGuangImgHOLD = value;
                this.RaisePropertyChanged("PaoGuangImgHOLD");
            }
        }
        private bool _duMoImgHOLD;
        /// <summary>
        /// 图形选择开关：镀膜
        /// </summary>
        public bool DuMoImgHOLD
        {
            get { return _duMoImgHOLD; }
            set
            {
                _duMoImgHOLD = value;
                this.RaisePropertyChanged("DuMoImgHOLD");
            }
        }
        #endregion
        #region 管控字段
        /// }
        //private bool _GuanKuong_NO;
        ///// <summary>
        ///// 图形选择开关：镀膜
        ///// </summary>
        //public bool DuMoImgHOLD
        //{
        //    get { return _duMoImgHOLD; }
        //    set
        //    {
        //        _duMoImgHOLD = value;
        //        this.RaisePropertyChanged("DuMoImgHOLD");
        //    }
        //}
        #endregion


        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                this.RaisePropertyChanged("FileName");
            }
        }
        public string FileFullName { get; set; }

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
                        _formworkRule.GetFormWorkListAsyns(queryList);
                        IsFormwork = true;

                        LotNo = string.Empty;
                        CurrentFormwork = null;
                        StockInQty = 0;
                        IsJianBao = false;
                        IsDuMo = false;
                        StockInHOLD = false;
                        JianBaoHOLD = false;
                        PaoGuangHOLD = false;
                        DuMoHOLD = false;
                        JianBaoImgHOLD = false;
                        PaoGuangImgHOLD = false;
                        DuMoImgHOLD = false;
                    });
                }
                return _cmdPageLoad;
            }
        }
        private DelegateCommand _cmdSelectFileName;
        public DelegateCommand CmdSelectFileName
        {
            get
            {
                if (_cmdSelectFileName == null)
                {
                    _cmdSelectFileName = new DelegateCommand(() =>
                    {
                        OpenFileDialog op = new OpenFileDialog();
                        op.RestoreDirectory = true;
                        op.Filter = "Excel文件|*.xls;*.xlsx";
                        bool? bresult = op.ShowDialog();
                        if (bresult != null && bresult.Value)
                        {
                            string ErrMsg = string.Empty;
                            Uri uri = new Uri(op.FileName, true);
                            FileName = uri.Segments[uri.Segments.Length - 1];//去处文件名用以显示在界面
                            FileFullName = op.FileName;//保存所选择的文件的完整路径

                            //_rule.ImportToXls(op.FileName, ref ErrMsg);
                        }

                    });
                }
                return _cmdSelectFileName;
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

                        //  StockLot entity = new StockLot();



                        if (string.IsNullOrWhiteSpace(LotNo))
                        {
                            Common.MessageBox.Show("请输入LotNo");
                            return;
                        }
                        //if (LotNo.Trim().Length > 35)
                        //{
                        //    Common.MessageBox.Show("LOTNO位数不能大于35位");
                        //    return;
                        //}
                        if (LotNo.Trim().Length > 50)
                        {
                            Common.MessageBox.Show("LOTNO位数不能大于50位");
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(CurrentFormwork))
                        {
                            Common.MessageBox.Show("请选择型号");
                            return;
                        }
                        if (IsImport)
                        {

                            if (string.IsNullOrWhiteSpace(FileFullName))
                            {
                                Common.MessageBox.Show("请选择需要导入的Excel文件");
                                return;
                            }
                        }
                        else
                        {
                            if (StockInQty <= 0)
                            {
                                Common.MessageBox.Show("请输入入库数量");
                                return;
                            }
                        }
                        if ((!IsJianBao) && (!IsDuMo))
                        {
                            Common.MessageBox.Show("工艺流程必须勾选");
                            return;
                        }


                        string strHOLD = string.Empty;
                        if (StockInHOLD)
                            strHOLD += "入库,";
                        if (JianBaoHOLD && IsJianBao)
                            strHOLD += "减薄,";
                        if (PaoGuangHOLD)
                            strHOLD += "抛光,";
                        if (DuMoHOLD && IsDuMo)
                            strHOLD += "镀膜,";

                        string strImgHOLD = string.Empty;
                        if (JianBaoImgHOLD && JianBaoHOLD && IsJianBao)
                            strImgHOLD += "减薄,";
                        if (PaoGuangImgHOLD && PaoGuangHOLD)
                            strImgHOLD += "抛光,";
                        if (DuMoImgHOLD && DuMoHOLD && IsDuMo)
                            strImgHOLD += "镀膜,";

                     
                        
                        StockLot entity = new StockLot();
                        entity.DetailInfoHOLD = strHOLD;
                        entity.IsDuMo = IsDuMo;
                        entity.IsJianBao = IsJianBao;
                        entity.LotNo = LotNo;
                        entity.IsImport = IsImport;
                        entity.ProModel = CurrentFormwork;
                        entity.PCSQty = StockInQty;
                        entity.ImageHOLD = strImgHOLD;

                        entity.GuanKong = _guanKongSingle ? "单独管控品" : "可混批";
                       
                        _rule.AddStockLotAndDetailsAsyns(FileFullName, entity);
                    });
                }
                return _cmdSave;
            }
        }//CmdGotoList
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

                    });
                }
                return _cmdGotoList;
            }
        }//CmdGotoList
    }
}
