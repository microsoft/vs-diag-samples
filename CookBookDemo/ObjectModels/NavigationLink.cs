using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Cookbook.Models
{
    public class NavigationLink : INotifyPropertyChanged
    {
        
        public NavigationLink()
        {
            this.LabelVisibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private Symbol _Symbol;
        public Symbol Symbol
        {
            get
            {
                return _Symbol;
            }
            set
            {
                _Symbol = value;
                OnPropertyChanged("Symbol");
            }
        }

        private string _Label;
        public string Label
        {
            get { return _Label; }
            set
            {
                _Label = value;
                OnPropertyChanged("Label");
            }
        }

        private Type _NavigationTarget;
        public Type NavigationTarget
        {
            get { return _NavigationTarget; }
            set { _NavigationTarget = value; OnPropertyChanged("NavigationTarget"); }
        }

        private Windows.UI.Xaml.Visibility _LabelVisibitliy;
        public Windows.UI.Xaml.Visibility LabelVisibility
        {
            get { return _LabelVisibitliy; }
            set { _LabelVisibitliy = value; OnPropertyChanged("LabelVisibility"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
            }
        }
    }
}
