using System.ComponentModel;

namespace WellNet.Utils
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Propertys
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (value.Equals(_status))
                    return;
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        #endregion Propertys

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
