using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.Infrastructure
 *   文件名:     BaseTypeCustom
 *   说明:       
 *   创建时间:   2014/3/3 10:07:03
 *   作者:       liende
 */
namespace SCG.SINOStock.Infrastructure
{
    public class BaseTypeCustom
    {
    }
    public class PropertyNodeItem : NotificationObject
    {
        private bool _isCheck;
        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                _isCheck = value;
                this.RaisePropertyChanged("IsCheck");
            }
        }
        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                this.RaisePropertyChanged("DisplayName");
            }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public string Icon { get; set; }

        public string EditIcon { get; set; }




        public List<PropertyNodeItem> Children { get; set; }

        public PropertyNodeItem()
        {

            Children = new List<PropertyNodeItem>();

        }

    }

}
