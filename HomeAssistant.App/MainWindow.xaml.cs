using System.ComponentModel;
using System.Windows;

namespace HomeAssistant.App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent = new ApplicationAdapter(this);
            DataContext = MainContent;
        }

        private FrameworkElement _mainContent;
        public FrameworkElement MainContent
        {
            get { return _mainContent; }
            private set { _mainContent = value; OnPropertyChanged(nameof(MainContent)); }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion INotifyPropertyChanged

    }
}
