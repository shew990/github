using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     QualityInfoRule
 *   说明:       
 *   创建时间:   2014/3/4 15:04:27
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class QualityInfoRule : RuleBase
    {
        public event EventHandler<ResultsArgs<QualityInfo>> GetQualityInfosCompleted;
        private ObservableCollection<QualityInfo> _qualityInfoCollection;
        public ObservableCollection<QualityInfo> QualityInfoCollection
        {
            get { return _qualityInfoCollection; }
            set
            {
                if (!ReferenceEquals(_qualityInfoCollection, value))
                {
                    _qualityInfoCollection = value;
                    OnPropertyChanged("QualityInfoCollection");
                }
            }
        }

        public List<QualityInfo> GetQualityInfoList(Dictionary<string, string> queryList, ref string ErrMsg)
        {
            return Proxy.GetQualityInfoList(CurrentAccount.CheckCode, CurrentAccount.ID, queryList, ref ErrMsg).ToList();
        }
        public void GetQualityInfosAsyns( Dictionary<string, string> queryList )
        {
            string ErrMsg = string.Empty;

            //if (!string.IsNullOrWhiteSpace(StrLoginName))
            //    queryList.Add("Name", StrLoginName.Trim());
            //if (!string.IsNullOrWhiteSpace(StrLoginNumber))
            Proxy.BeginGetQualityInfoList(CurrentAccount.CheckCode, CurrentAccount.ID, queryList, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {

                    try
                    {
                        QualityInfoCollection = new ObservableCollection<QualityInfo>(Proxy.EndGetQualityInfoList(ref ErrMsg, result));
                        if (GetQualityInfosCompleted != null)
                        {
                            GetQualityInfosCompleted(this, new ResultsArgs<QualityInfo>(QualityInfoCollection, null, false, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetQualityInfosCompleted != null)
                        {
                            GetQualityInfosCompleted(this, new ResultsArgs<QualityInfo>(null, ex, true, result.AsyncState));
                        }
                        _lastError = ex;
                    }
                });

            }, null);
        }

        public event EventHandler<ResultArgs<bool>> AddQualityInfoCompleted;
        public void AddQualityInfoAsyns(QualityInfo entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginAddQualityInfo(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndAddQualityInfo(ref ErrMsg, result);
                        if (bResult)
                        {
                            AddQualityInfoCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            AddQualityInfoCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (AddQualityInfoCompleted != null)
                        {
                            AddQualityInfoCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }
        public event EventHandler<ResultArgs<bool>> ModifyQualityInfoCompleted;

        public void ModifyQualityInfoAsyns(QualityInfo entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginModifyQualityInfo(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndModifyQualityInfo(ref ErrMsg, result);
                        if (bResult)
                        {
                            ModifyQualityInfoCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            ModifyQualityInfoCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ModifyQualityInfoCompleted != null)
                        {
                            ModifyQualityInfoCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<bool>> DelQualityInfoCompleted;

        public void DelQualityInfoAsyns(QualityInfo entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginDelQualityInfo(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndDelQualityInfo(ref ErrMsg, result);
                        if (bResult)
                        {
                            DelQualityInfoCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            DelQualityInfoCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DelQualityInfoCompleted != null)
                        {
                            DelQualityInfoCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }
    }
}
