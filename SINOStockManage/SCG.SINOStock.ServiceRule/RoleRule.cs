using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     RoleRule
 *   说明:       
 *   创建时间:   2014/3/3 21:42:38
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class RoleRule : RuleBase
    {
        public event EventHandler<ResultArgs<Role>> LoginCompleted;
        public event EventHandler<ResultsArgs<Role>> GetRolesCompleted;
        private ObservableCollection<Role> _RoleCollection;
        public ObservableCollection<Role> RoleCollection
        {
            get { return _RoleCollection; }
            set
            {
                if (!ReferenceEquals(_RoleCollection, value))
                {
                    _RoleCollection = value;
                    OnPropertyChanged("RoleCollection");
                }
            }
        }

        public List<Role> GetRoleList(Dictionary<string, string> queryList, ref string ErrMsg)
        {
            return Proxy.GetRoleList(CurrentAccount.CheckCode, CurrentAccount.ID, queryList, ref ErrMsg).ToList();
        }
        public void GetRolesAsyns(Dictionary<string, string> queryList)
        {
            string ErrMsg = string.Empty;
            //  = new Dictionary<string, string>();

            Proxy.BeginGetRoleList(CurrentAccount.CheckCode, CurrentAccount.ID, queryList, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {

                    try
                    {
                        RoleCollection = new ObservableCollection<Role>(Proxy.EndGetRoleList(ref ErrMsg, result));
                        if (GetRolesCompleted != null)
                        {
                            GetRolesCompleted(this, new ResultsArgs<Role>(RoleCollection, null, false, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetRolesCompleted != null)
                        {
                            GetRolesCompleted(this, new ResultsArgs<Role>(null, ex, true, result.AsyncState));
                        }
                        _lastError = ex;
                    }
                });

            }, null);
        }

        public event EventHandler<ResultArgs<bool>> AddRoleCompleted;
        public void AddRoleAsyns(Role entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginAddRole(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndAddRole(ref ErrMsg, result);
                        if (bResult)
                        {
                            AddRoleCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            AddRoleCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (AddRoleCompleted != null)
                        {
                            AddRoleCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }
        public event EventHandler<ResultArgs<bool>> ModifyRoleCompleted;

        public void ModifyRoleAsyns(Role entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginModifyRole(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndModifyRole(ref ErrMsg, result);
                        if (bResult)
                        {
                            ModifyRoleCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            ModifyRoleCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ModifyRoleCompleted != null)
                        {
                            ModifyRoleCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<bool>> DelRoleCompleted;

        public void DelRoleAsyns(Role entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginDelRole(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndDelRole(ref ErrMsg, result);
                        if (bResult)
                        {
                            DelRoleCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            DelRoleCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DelRoleCompleted != null)
                        {
                            DelRoleCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }


        #region 自定义字段（不可更改值，只可增加）
        public readonly string str账户管理 = "a";
        public readonly string str角色管理 = "b";
        public readonly string str模版管理 = "c";
        public readonly string str扫描入库 = "d";
        public readonly string str扫描入库_无对比 = "e";
        public readonly string str减薄后检验 = "f";
        public readonly string str抛光后检验 = "g";
        public readonly string str镀膜后检验 = "h";
        public readonly string str镀膜后检验_无对比 = "i";
        public readonly string str导入GlassID = "j";
        public readonly string str查询GlassID = "k";
        public readonly string str品质信息管理 = "l";
        public readonly string strGLassID后台 = "m";

        public readonly string str添加 = "1";
        public readonly string str修改 = "2";
        public readonly string str删除 = "3";
        public readonly string str导出 = "4";
        
        public readonly string str接触HOLD = "5";
        public readonly string str是否允许修改HOLD = "6";
        public readonly string str入库 = "7";
        public readonly string str减薄 = "8";
        public readonly string str抛光 = "9";
        public readonly string str镀膜 = "0";
        public readonly string str转移GLASSID = "z";
        public readonly string strGLASSID明细 = "y";
        public readonly string str入库_无 = "x";
        public readonly string str替换GLASSID = "w";
        public readonly string str入库结束 = "v";
        public readonly string strHOLD = "u";

        #endregion
        public string RoleFunctionAsCode(string condition)
        {
            string result = string.Empty;
            switch (condition)
            {
                case "账户管理":
                    result = str账户管理;
                    break;
                case "角色管理":
                    result = str角色管理;
                    break;
                case "模版管理":
                    result = str模版管理;
                    break;
                case "扫描入库":
                    result = str扫描入库;
                    break;
                case "扫描入库(无对比)":
                    result = str扫描入库_无对比;
                    break;
                case "减薄后检验":
                    result = str减薄后检验;
                    break;
                case "抛光后检验":
                    result = str抛光后检验;
                    break;
                case "镀膜后检验":
                    result = str镀膜后检验;
                    break;
                case "镀膜后检验(无对比)":
                    result = str镀膜后检验_无对比;
                    break;
                case "新建LotNO":
                    result = str导入GlassID;
                    break;
                case "查询LotNo":
                    result = str查询GlassID;
                    break;
                case "品质信息管理":
                    result = str品质信息管理;
                    break;
                case "解除HOLD":
                    result = str接触HOLD;
                    break;
                case "GlassID后台":
                    result = strGLassID后台;
                    break;
                case "HOLD":
                    result = strHOLD;
                    break;


                case "添加":
                    result = str添加;
                    break;
                case "修改":
                    result = str修改;
                    break;
                case "删除":
                    result = str删除;
                    break;
                case "导出":
                    result = str导出;
                    break;
                case "是否允许修改HOLD":
                    result = str是否允许修改HOLD;
                    break;
                case "入库":
                    result = str入库;
                    break;
                case "入库(无)":
                    result = str入库_无;
                    break;
                case "减薄":
                    result = str减薄;
                    break;
                case "抛光":
                    result = str抛光;
                    break;
                case "镀膜":
                    result = str镀膜;
                    break;
                case "GLASSID明细":
                    result = strGLASSID明细;
                    break;
                case "转移GLASSID":
                    result = str转移GLASSID;
                    break;
                case "替换GLASSID":
                    result = str替换GLASSID;
                    break;
                case "入库结束":
                    result = str入库结束;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
