using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     FormworkRule
 *   说明:       
 *   创建时间:   2014/2/2 16:02:51
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class FormworkRule : RuleBase
    {
        public event EventHandler<ResultsArgs<FormWork>> GetFormworkListCompleted;
        private ObservableCollection<FormWork> _formworkCollection;
        public ObservableCollection<FormWork> FormworkCollection
        {
            get { return _formworkCollection; }
            set
            {
                if (!ReferenceEquals(_formworkCollection, value))
                {
                    _formworkCollection = value;
                    OnPropertyChanged("FormworkCollection");
                }
            }
        }
        public void GetFormWorkListAsyns( Dictionary<string, string> queryList )
        {
            string ErrMsg = string.Empty;
            Proxy.BeginGetFormWorkList(CurrentAccount.CheckCode, CurrentAccount.ID, queryList, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        FormworkCollection = new ObservableCollection<FormWork>(Proxy.EndGetFormWorkList(ref ErrMsg, result));
                        if (GetFormworkListCompleted != null)
                        {
                            GetFormworkListCompleted(this, new ResultsArgs<FormWork>(FormworkCollection, null, false, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetFormworkListCompleted != null)
                        {
                            GetFormworkListCompleted(this, new ResultsArgs<FormWork>(null, ex, true, result.AsyncState));
                        }
                        _lastError = ex;
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<bool>> AddFormworkCompleted;
        public void AddFormworkAsyns(FormWork entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginAddFormWork(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndAddFormWork(ref ErrMsg, result);
                        if (bResult)
                        {
                            AddFormworkCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            AddFormworkCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (AddFormworkCompleted != null)
                        {
                            AddFormworkCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<bool>> ModifyFormworkCompleted;
        public void ModifyFormworkAsyns(FormWork entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginModifyFormWork(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndModifyFormWork(ref ErrMsg, result);
                        if (bResult)
                        {
                            ModifyFormworkCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            ModifyFormworkCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ModifyFormworkCompleted != null)
                        {
                            ModifyFormworkCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<bool>> DelFormworkCompleted;
        public void DelFormworkAsyns(FormWork entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginDelFormWork(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndDelFormWork(ref ErrMsg, result);
                        if (bResult)
                        {
                            DelFormworkCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            DelFormworkCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DelFormworkCompleted != null)
                        {
                            DelFormworkCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<FormWork>> GetFormWorkByProModelCompleted;
        public void GetFormWorkByProModelAsyns(string proModel)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginGetFormWorkByProModel(CurrentAccount.CheckCode, CurrentAccount.ID, proModel, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        FormWork entity = Proxy.EndGetFormWorkByProModel(ref ErrMsg, result);
                        if (entity != null)
                        {
                            GetFormWorkByProModelCompleted(this, new ResultArgs<FormWork>(entity, null, false, result.AsyncState));
                        }
                        else
                        {
                            GetFormWorkByProModelCompleted(this, new ResultArgs<FormWork>(null, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetFormWorkByProModelCompleted != null)
                        {
                            GetFormWorkByProModelCompleted(this, new ResultArgs<FormWork>(null, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public FormWork GetFormWorkByProModel( string proModel, ref string ErrMsg)
        {
            return Proxy.GetFormWorkByProModel(CurrentAccount.CheckCode, CurrentAccount.ID, proModel, ref ErrMsg);
        }
    }
}
