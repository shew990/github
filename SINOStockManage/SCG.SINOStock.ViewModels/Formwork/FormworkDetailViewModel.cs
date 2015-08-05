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
 *   命名空间:   SCG.SINOStock.ViewModels.Formwork
 *   文件名:     FormworkDetailViewModel
 *   说明:       
 *   创建时间:   2014/2/2 20:20:06
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels.Formwork
{
    public class FormworkDetailViewModel : ViewModelBase
    {
        private FormworkRule _rule = null;
        public FormworkDetailViewModel()
        {
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                CurrentFormwork = param.Entity as FormWork;
                IdKeyWordsTmp = new IDKeyWordsEx();
                if (CurrentFormwork == null)
                {
                    CurrentFormwork = new FormWork();
                    IsProModelEnb = true;
                }
                else
                {
                    IsProModelEnb = false;
                    IdKeyWordsTmp.Assignment(CurrentFormwork.IDKeyWords, CurrentFormwork.IDNumber);
                }
            }, ThreadOption.UIThread, true, p => p.Target == "FormworkDetailViewModel");
            _rule = new FormworkRule();
            _rule.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName.Equals("IsBusy"))
                    {
                        this.IsBusy = _rule.IsBusy;
                    }
                };
            _rule.AddFormworkCompleted += (s, e) =>
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
                           cmdViewName = CmdViewName.FormworkMainView,
                           Target = "Sell",
                       });
                   }
               };
            _rule.ModifyFormworkCompleted += (s, e) =>
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
                            cmdViewName = CmdViewName.FormworkMainView,
                            Target = "Sell",
                        });
                    }
                };
        }
        #region 界面绑定属性
        private bool _isProModelEnb;
        public bool IsProModelEnb
        {
            get { return _isProModelEnb; }
            set
            {
                _isProModelEnb = value;
                this.RaisePropertyChanged("IsProModelEnb");
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

        private IDKeyWordsEx _idKeyWordsTmp;
        public IDKeyWordsEx IdKeyWordsTmp
        {
            get { return _idKeyWordsTmp; }
            set
            {
                _idKeyWordsTmp = value;
                this.RaisePropertyChanged("IdKeyWordsTmp");
            }
        }
        int i = 0;
        private DelegateCommand _cmdSave;
        public DelegateCommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new DelegateCommand(() =>
                    {

                        if (CurrentFormwork != null)
                        {
                            if (string.IsNullOrWhiteSpace(CurrentFormwork.ProductModel))
                            {
                                Common.MessageBox.Show("请输入产品型号");
                                return;
                            }
                            if (CurrentFormwork.BoxPCSQty <= 0)
                            {
                                Common.MessageBox.Show("请输入每箱数量或每箱数量输入格式不正确");
                                return;
                            }
                            if (CurrentFormwork.BoxQty <= 0)
                            {
                                Common.MessageBox.Show("请输入每托箱数或每托箱数输入格式不正确");
                                return;
                            }
                            if (CurrentFormwork.IDNumber <= 0)
                            {
                                Common.MessageBox.Show("请输入ID位数或ID位数输入格式不正确");
                                return;
                            }
                            if (CurrentFormwork.IDNumber < 4)
                            {
                                Common.MessageBox.Show("ID位数必须大于或等于4");
                                return;
                            }
                            //if (CurrentFormwork.QieShu <= 0)
                            //{
                            //    Common.MessageBox.Show("请输入切数或切数输入格式不正确");
                            //    return;
                            //}
                            if (CurrentFormwork.RowQty <= 0)
                            {
                                Common.MessageBox.Show("请输入行数或行数输入格式不正确");
                                return;
                            }
                            if (CurrentFormwork.RowQty > 22)
                            {
                                Common.MessageBox.Show("行数最大为22");
                                return;
                            }
                            if (CurrentFormwork.ColumnQty <= 0)
                            {
                                Common.MessageBox.Show("请输入列数或列数输入格式不正确");
                                return;
                            }
                            if (CurrentFormwork.ColumnQty > 18)
                            {
                                Common.MessageBox.Show("列数最大为18");
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(IdKeyWordsTmp.strKey1) || string.IsNullOrWhiteSpace(IdKeyWordsTmp.strKey2) || string.IsNullOrWhiteSpace(IdKeyWordsTmp.strKey3) || string.IsNullOrWhiteSpace(IdKeyWordsTmp.strKey4))
                            {
                                Common.MessageBox.Show("关键字前四位必须输入");
                                return;
                            }
                            CurrentFormwork.IDKeyWords = IdKeyWordsTmp.ToString().ToUpper();
                            if (CurrentFormwork.ID <= 0)
                            {
                                _rule.AddFormworkAsyns(CurrentFormwork);
                            }
                            else
                            {
                                _rule.ModifyFormworkAsyns(CurrentFormwork);
                            }
                        }
                    });
                }
                return _cmdSave;
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
                            cmdViewName = CmdViewName.FormworkMainView,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoList;
            }
        }

        private int _iText;

        public int IText
        {
            get { return _iText; }
            set
            {
                _iText = value;
                this.RaisePropertyChanged("IText");
            }
        }
        private DelegateCommand<RoutedEventArgs> _cmdTest;
        public DelegateCommand<RoutedEventArgs> CmdTest
        {
            get
            {
                if (_cmdTest == null)
                {
                    _cmdTest = new DelegateCommand<RoutedEventArgs>(e =>
                      {
                          if (CurrentFormwork != null)
                          {
                              IdKeyWordsTmp.EnableChenge(CurrentFormwork.IDNumber);
                          }
                          // IdKeyWordsTmp.EnableChenge(IText);
                      });
                }
                return _cmdTest;
            }
        }

        private DelegateCommand _cmdChangeQieShu;
        public DelegateCommand CmdChangeQieShu
        {
            get
            {
                if (_cmdChangeQieShu == null)
                {
                    _cmdChangeQieShu = new DelegateCommand(() =>
                    {
                        if (CurrentFormwork != null)
                        {
                            CurrentFormwork.QieShu = CurrentFormwork.RowQty * CurrentFormwork.ColumnQty;
                        }
                        // IdKeyWordsTmp.EnableChenge(IText);
                    });
                }
                return _cmdChangeQieShu;
            }
        }
        #endregion
    }
    public class IDKeyWordsEx : Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        // public string strKey1 { get; set; }
        private string _strKey1;
        public string strKey1
        {
            get { return _strKey1; }
            set
            {
                _strKey1 = value;
                this.RaisePropertyChanged("strKey1");
            }
        }
        private string _strKey2;
        public string strKey2
        {
            get { return _strKey2; }
            set
            {
                _strKey2 = value;
                this.RaisePropertyChanged("strKey2");
            }
        }
        private string _strKey3;
        public string strKey3
        {
            get { return _strKey3; }
            set
            {
                _strKey3 = value;
                this.RaisePropertyChanged("strKey3");
            }
        }
        private string _strKey4;
        public string strKey4
        {
            get { return _strKey4; }
            set
            {
                _strKey4 = value;
                this.RaisePropertyChanged("strKey4");
            }
        }
        private string _strKey5;
        public string strKey5
        {
            get { return _strKey5; }
            set
            {
                _strKey5 = value;
                this.RaisePropertyChanged("strKey5");
            }
        }
        private string _strKey6;
        public string strKey6
        {
            get { return _strKey6; }
            set
            {
                _strKey6 = value;
                this.RaisePropertyChanged("strKey6");
            }
        }
        private string _strKey7;
        public string strKey7
        {
            get { return _strKey7; }
            set
            {
                _strKey7 = value;
                this.RaisePropertyChanged("strKey7");
            }
        }
        private string _strKey8;
        public string strKey8
        {
            get { return _strKey8; }
            set
            {
                _strKey8 = value;
                this.RaisePropertyChanged("strKey8");
            }
        }
        private string _strKey9;
        public string strKey9
        {
            get { return _strKey9; }
            set
            {
                _strKey9 = value;
                this.RaisePropertyChanged("strKey9");
            }
        }
        private string _strKey10;
        public string strKey10
        {
            get { return _strKey10; }
            set
            {
                _strKey10 = value;
                this.RaisePropertyChanged("strKey10");
            }
        }
        private string _strKey11;
        public string strKey11
        {
            get { return _strKey11; }
            set
            {
                _strKey11 = value;
                this.RaisePropertyChanged("strKey11");
            }
        }
        private string _strKey12;
        public string strKey12
        {
            get { return _strKey12; }
            set
            {
                _strKey12 = value;
                this.RaisePropertyChanged("strKey12");
            }
        }
        private string _strKey13;
        public string strKey13
        {
            get { return _strKey13; }
            set
            {
                _strKey13 = value;
                this.RaisePropertyChanged("strKey13");
            }
        }
        private string _strKey14;
        public string strKey14
        {
            get { return _strKey14; }
            set
            {
                _strKey14 = value;
                this.RaisePropertyChanged("strKey14");
            }
        }
        private string _strKey15;
        public string strKey15
        {
            get { return _strKey15; }
            set
            {
                _strKey15 = value;
                this.RaisePropertyChanged("strKey15");
            }
        }
        private string _strKey16;
        public string strKey16
        {
            get { return _strKey16; }
            set
            {
                _strKey16 = value;
                this.RaisePropertyChanged("strKey16");
            }
        }
        private string _strKey17;
        public string strKey17
        {
            get { return _strKey17; }
            set
            {
                _strKey17 = value;
                this.RaisePropertyChanged("strKey17");
            }
        }
        private string _strKey18;
        public string strKey18
        {
            get { return _strKey18; }
            set
            {
                _strKey18 = value;
                this.RaisePropertyChanged("strKey18");
            }
        }
        private string _strKey19;
        public string strKey19
        {
            get { return _strKey19; }
            set
            {
                _strKey19 = value;
                this.RaisePropertyChanged("strKey19");
            }
        }
        private string _strKey20;
        public string strKey20
        {
            get { return _strKey20; }
            set
            {
                _strKey20 = value;
                this.RaisePropertyChanged("strKey20");
            }
        }

        private bool _isEnabled1;
        public bool IsEnabled1
        {
            get { return _isEnabled1; }
            set
            {
                _isEnabled1 = value;
                this.RaisePropertyChanged("IsEnabled1");
            }
        }
        private bool _isEnabled2;
        public bool IsEnabled2
        {
            get { return _isEnabled2; }
            set
            {
                _isEnabled2 = value;
                this.RaisePropertyChanged("IsEnabled2");
            }
        }
        private bool _isEnabled3;
        public bool IsEnabled3
        {
            get { return _isEnabled3; }
            set
            {
                _isEnabled3 = value;
                this.RaisePropertyChanged("IsEnabled3");
            }
        }
        private bool _isEnabled4;
        public bool IsEnabled4
        {
            get { return _isEnabled4; }
            set
            {
                _isEnabled4 = value;
                this.RaisePropertyChanged("IsEnabled4");
            }
        }
        private bool _isEnabled5;
        public bool IsEnabled5
        {
            get { return _isEnabled5; }
            set
            {
                _isEnabled5 = value;
                this.RaisePropertyChanged("IsEnabled5");
            }
        }
        private bool _isEnabled6;
        public bool IsEnabled6
        {
            get { return _isEnabled6; }
            set
            {
                _isEnabled6 = value;
                this.RaisePropertyChanged("IsEnabled6");
            }
        }
        private bool _isEnabled7;
        public bool IsEnabled7
        {
            get { return _isEnabled7; }
            set
            {
                _isEnabled7 = value;
                this.RaisePropertyChanged("IsEnabled7");
            }
        }
        private bool _isEnabled8;
        public bool IsEnabled8
        {
            get { return _isEnabled8; }
            set
            {
                _isEnabled8 = value;
                this.RaisePropertyChanged("IsEnabled8");
            }
        }
        private bool _isEnabled9;
        public bool IsEnabled9
        {
            get { return _isEnabled9; }
            set
            {
                _isEnabled9 = value;
                this.RaisePropertyChanged("IsEnabled9");
            }
        }
        private bool _isEnabled10;
        public bool IsEnabled10
        {
            get { return _isEnabled10; }
            set
            {
                _isEnabled10 = value;
                this.RaisePropertyChanged("IsEnabled10");
            }
        }
        private bool _isEnabled11;
        public bool IsEnabled11
        {
            get { return _isEnabled11; }
            set
            {
                _isEnabled11 = value;
                this.RaisePropertyChanged("IsEnabled11");
            }
        }
        private bool _isEnabled12;
        public bool IsEnabled12
        {
            get { return _isEnabled12; }
            set
            {
                _isEnabled12 = value;
                this.RaisePropertyChanged("IsEnabled12");
            }
        }
        private bool _isEnabled13;
        public bool IsEnabled13
        {
            get { return _isEnabled13; }
            set
            {
                _isEnabled13 = value;
                this.RaisePropertyChanged("IsEnabled13");
            }
        }
        private bool _isEnabled14;
        public bool IsEnabled14
        {
            get { return _isEnabled14; }
            set
            {
                _isEnabled14 = value;
                this.RaisePropertyChanged("IsEnabled14");
            }
        }
        private bool _isEnabled15;
        public bool IsEnabled15
        {
            get { return _isEnabled15; }
            set
            {
                _isEnabled15 = value;
                this.RaisePropertyChanged("IsEnabled15");
            }
        }
        private bool _isEnabled16;
        public bool IsEnabled16
        {
            get { return _isEnabled16; }
            set
            {
                _isEnabled16 = value;
                this.RaisePropertyChanged("IsEnabled16");
            }
        }
        private bool _isEnabled17;
        public bool IsEnabled17
        {
            get { return _isEnabled17; }
            set
            {
                _isEnabled17 = value;
                this.RaisePropertyChanged("IsEnabled17");
            }
        }
        private bool _isEnabled18;
        public bool IsEnabled18
        {
            get { return _isEnabled18; }
            set
            {
                _isEnabled18 = value;
                this.RaisePropertyChanged("IsEnabled18");
            }
        }
        private bool _isEnabled19;
        public bool IsEnabled19
        {
            get { return _isEnabled19; }
            set
            {
                _isEnabled19 = value;
                this.RaisePropertyChanged("IsEnabled19");
            }
        }
        private bool _isEnabled20;
        public bool IsEnabled20
        {
            get { return _isEnabled20; }
            set
            {
                _isEnabled20 = value;
                this.RaisePropertyChanged("IsEnabled20");
            }
        }
        public void Assignment(string strIdkeys, int iKey)
        {
            string[] tmpArray = strIdkeys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in tmpArray)
            {
                int iIndex = Int32.Parse(item.Substring(0, item.Length - 1));
                string strValue = item.Substring(item.Length - 1, 1);
                //   if (iIndex <= entity.IDNumber)
                //   {
                switch (iIndex)
                {
                    case 1:
                        strKey1 = strValue;
                        break;
                    case 2:
                        strKey2 = strValue;
                        break;
                    case 3:
                        strKey3 = strValue;
                        break;
                    case 4:
                        strKey4 = strValue;
                        break;
                    case 5:
                        strKey5 = strValue;
                        break;
                    case 6:
                        strKey6 = strValue;
                        break;
                    case 7:
                        strKey7 = strValue;
                        break;
                    case 8:
                        strKey8 = strValue;
                        break;
                    case 9:
                        strKey9 = strValue;
                        break;
                    case 10:
                        strKey10 = strValue;
                        break;
                    case 11:
                        strKey11 = strValue;
                        break;
                    case 12:
                        strKey12 = strValue;
                        break;
                    case 13:
                        strKey13 = strValue;
                        break;
                    case 14:
                        strKey14 = strValue;
                        break;
                    case 15:
                        strKey15 = strValue;
                        break;
                    case 16:
                        strKey16 = strValue;
                        break;
                    case 17:
                        strKey17 = strValue;
                        break;
                    case 18:
                        strKey18 = strValue;
                        break;
                    case 19:
                        strKey19 = strValue;
                        break;
                    case 20:
                        strKey20 = strValue;
                        break;
                    default:
                        break;
                }
            }
            EnableChenge(iKey);
            //    }
        }

        public void EnableChenge(int iKey)
        {
            IsEnabled1 = false;
            IsEnabled2 = false;
            IsEnabled3 = false;
            IsEnabled4 = false;
            IsEnabled5 = false;
            IsEnabled6 = false;
            IsEnabled7 = false;
            IsEnabled8 = false;
            IsEnabled9 = false;
            IsEnabled10 = false;
            IsEnabled11 = false;
            IsEnabled12 = false;
            IsEnabled13 = false;
            IsEnabled14 = false;
            IsEnabled15 = false;
            IsEnabled16 = false;
            IsEnabled17 = false;
            IsEnabled18 = false;
            IsEnabled19 = false;
            IsEnabled20 = false;
            if (iKey >= 1)
                IsEnabled1 = true;
            if (iKey >= 2)
                IsEnabled2 = true;
            if (iKey >= 3)
                IsEnabled3 = true;
            if (iKey >= 4)
                IsEnabled4 = true;
            if (iKey >= 5)
                IsEnabled5 = true;
            if (iKey >= 6)
                IsEnabled6 = true;
            if (iKey >= 7)
                IsEnabled7 = true;
            if (iKey >= 8)
                IsEnabled8 = true;
            if (iKey >= 9)
                IsEnabled9 = true;
            if (iKey >= 10)
                IsEnabled10 = true;
            if (iKey >= 11)
                IsEnabled11 = true;
            if (iKey >= 12)
                IsEnabled12 = true;
            if (iKey >= 13)
                IsEnabled13 = true;
            if (iKey >= 14)
                IsEnabled14 = true;
            if (iKey >= 15)
                IsEnabled15 = true;
            if (iKey >= 16)
                IsEnabled16 = true;
            if (iKey >= 17)
                IsEnabled17 = true;
            if (iKey >= 18)
                IsEnabled18 = true;
            if (iKey >= 19)
                IsEnabled19 = true;
            if (iKey >= 20)
                IsEnabled20 = true;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(strKey1))
                sb.Append("1" + strKey1 + ",");
            if (!string.IsNullOrEmpty(strKey2))
                sb.Append("2" + strKey2 + ",");
            if (!string.IsNullOrEmpty(strKey3))
                sb.Append("3" + strKey3 + ",");
            if (!string.IsNullOrEmpty(strKey4))
                sb.Append("4" + strKey4 + ",");
            if (!string.IsNullOrEmpty(strKey5))
                sb.Append("5" + strKey5 + ",");
            if (!string.IsNullOrEmpty(strKey6))
                sb.Append("6" + strKey6 + ",");
            if (!string.IsNullOrEmpty(strKey7))
                sb.Append("7" + strKey7 + ",");
            if (!string.IsNullOrEmpty(strKey8))
                sb.Append("8" + strKey8 + ",");
            if (!string.IsNullOrEmpty(strKey9))
                sb.Append("9" + strKey9 + ",");
            if (!string.IsNullOrEmpty(strKey10))
                sb.Append("10" + strKey10 + ",");
            if (!string.IsNullOrEmpty(strKey11))
                sb.Append("11" + strKey11 + ",");
            if (!string.IsNullOrEmpty(strKey12))
                sb.Append("12" + strKey12 + ",");
            if (!string.IsNullOrEmpty(strKey13))
                sb.Append("13" + strKey13 + ",");
            if (!string.IsNullOrEmpty(strKey14))
                sb.Append("14" + strKey14 + ",");
            if (!string.IsNullOrEmpty(strKey15))
                sb.Append("15" + strKey15 + ",");
            if (!string.IsNullOrEmpty(strKey16))
                sb.Append("16" + strKey16 + ",");
            if (!string.IsNullOrEmpty(strKey17))
                sb.Append("17" + strKey17 + ",");
            if (!string.IsNullOrEmpty(strKey18))
                sb.Append("18" + strKey18 + ",");
            if (!string.IsNullOrEmpty(strKey19))
                sb.Append("19" + strKey19 + ",");
            if (!string.IsNullOrEmpty(strKey20))
                sb.Append("20" + strKey20 + ",");
            return sb.ToString().ToUpper();
        }
    }
}
