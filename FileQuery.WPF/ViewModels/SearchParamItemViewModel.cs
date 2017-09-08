using System.Collections.Generic;
using FileQuery.Wpf.Util;

namespace FileQuery.Wpf.ViewModels
{
    public class SearchParamItemViewModel : ViewModelBase
    {

        private string _Value;
        private FilterOperatorItem _Operator;
        private string _Type;
        private bool _RemoveItem;

        private IEnumerable<FilterOperatorItem> _Operators;

        public string ParamValue
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = value;
                NotifyPropertyChanged();
            }
        }

        public FilterOperatorItem ParamOperator
        {
            get
            {
                return _Operator;
            }

            set
            {
                _Operator = value;
                NotifyPropertyChanged();
            }
        }

        public string ParamType
        {
            get
            {
                return _Type;
            }

            set
            {
                _Type = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("IsNotBoolParam");
                UpdateOperators();
            }
        }

        public bool RemoveItem
        {
            get
            {
                return _RemoveItem;
            }

            set
            {
                _RemoveItem = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerable<FilterOperatorItem> Operators
        {
            get
            {
                return _Operators;
            }

            set
            {
                _Operators = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsNotBoolParam
        {
            get
            {
                return ParamType != "Read Only";
            }
        }

        private void UpdateOperators()
        {
            Operators = FilterOperatorUtil.GetOperatorsForType(ParamType);
        }

        public bool IsValid
        {
            get
            {
                return ParamOperator != null && !string.IsNullOrEmpty(ParamType);// readonly doesn't have a value && !string.IsNullOrEmpty(ParamValue);
            }
        }
    }
}
