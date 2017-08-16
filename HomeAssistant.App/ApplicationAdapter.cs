using HomeAssistant.Core.Integration;
using HomeAssistant.Core.Utilites;
using HomeAssistant.Core.ViewModel;
using Microsoft.Practices.Unity;
using NLog;
using NLog.LayoutRenderers;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HomeAssistant.App
{
    public class ApplicationAdapter : ContentPresenter,
        IDialogService,
        IDisplayManagement,
        IErrorManagement,
        ISettings,
        ISecurity,
        INotifyPropertyChanged

    {
        private MainWindow _mainWindow;
        private readonly UnityContainer _mainServiceContainer = new UnityContainer();
        private IUnityContainer _loginServiceContainer;

        private readonly ICommand _showAccounterDataCommand;
        private readonly ICommand _closeDialogCommand;

        private static Logger logger;

        public ApplicationAdapter(MainWindow mainWindow)
        {
            if (mainWindow == null)
            {
                throw new ArgumentNullException("mainWindow");
            }
            _mainWindow = mainWindow;
            Content = "It's me - App adapter";

            _mainServiceContainer.RegisterInstance<IErrorManagement>(this, new ExternallyControlledLifetimeManager());
            _mainServiceContainer.RegisterInstance<IDisplayManagement>(this, new ExternallyControlledLifetimeManager());
            _mainServiceContainer.RegisterInstance<IDialogService>(this, new ExternallyControlledLifetimeManager());
            _mainServiceContainer.RegisterInstance<ISettings>(this, new ExternallyControlledLifetimeManager());
            _mainServiceContainer.RegisterInstance<ISecurity>(this, new ExternallyControlledLifetimeManager());
            CreateLoginContainer();



            LayoutRenderer.Register("userName", (logEvent) => User);
            LayoutRenderer.Register("connString", (logEvent) => ConnectionString);

            logger = LogManager.GetCurrentClassLogger();

            _showAccounterDataCommand = new RelayCommand(this, ShowAccounterData);
            _closeDialogCommand = new RelayCommand(this, CloseDialog);

        }
        private void CreateLoginContainer()
        {
            //_loginServiceContainer = _mainServiceContainer.CreateChildContainer();
            //_loginServiceContainer.RegisterType<IProductCalculatorContext, ProductCalculatorContext>(new ContainerControlledLifetimeManager());
            //_loginServiceContainer.RegisterType<IProductCalculatorContextProvider, ProductCalculatorContextProvider>(new ContainerControlledLifetimeManager());
            //_loginServiceContainer.RegisterType<IProductCalculator, ProductCalculatorModel>(new ContainerControlledLifetimeManager());

            //_loginServiceContainer.RegisterType<IFatBalanceCatalog, FatBalanceCatalogModel>(new ContainerControlledLifetimeManager());
            //_loginServiceContainer.RegisterType<IFatBalanceMain, FatBalanceMainModel>(new ContainerControlledLifetimeManager());
            //_loginServiceContainer.RegisterType<IFatBalanceContextProvider, FatBalanceContextProvider>(new ContainerControlledLifetimeManager());
        }

        #region IDisplayManagement
        public bool ShowViewModel(Type viewModelType, bool showAsDialog, params object[] parameter)
        {
            if (showAsDialog)
            {
                return false;
                //return ViewModelDialog.Show(_serviceContainer, null, viewModelType, parameter);
            }
            else
            {
                //this.Host.ExecuteCommand(viewModelType.Name, parameter, this);
                CloseViewModel(false);
                if (null != parameter)
                {
                    Content = (ViewModelBase)_loginServiceContainer.Resolve(
                        viewModelType,
                        parameter.Select(x => new DependencyOverride(x.GetType(), x)).ToArray()
                        );
                }
                else
                {
                    Content = (ViewModelBase)_loginServiceContainer.Resolve(viewModelType);
                }
                // DialogContent = (ViewModelBase)_loginServiceContainer.Resolve(viewModelType);          
                return false;
            }
        }


        public ICommand CloseDialogCommand { get { return _closeDialogCommand; } }
        private void CloseDialog(object obj)
        {
            if (DialogContent is IDisposable)
                ((IDisposable)DialogContent).Dispose();
            DialogContent = null;
        }

        private ViewModelBase _dialogContent;
        public ViewModelBase DialogContent
        {
            get { return _dialogContent; }
            set { _dialogContent = value; OnPropertyChanged(nameof(DialogContent)); }
        }

        public void CloseViewModel(bool result)
        {
            if (this.Content != null && this.Content is IDisposable)
            {
                ((IDisposable)this.Content).Dispose();
                this.Content = null;
            }
        }
        #endregion IDisplayManagement

        #region IDialogService
        public MessageBoxResult ShowQuery(
            string caption,
            string text,
            MessageBoxButton buttons,
            MessageBoxImage image)
        {
            return MessageBox.Show(_mainWindow, text, caption, buttons, image);
        }
        #endregion IDialogService

        #region IErrorManagement
        public void ShowError(Exception error)
        {
            logger.Error(error);
            MessageBox.Show(
                _mainWindow,
                error.FullMessage(),
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public bool CheckAccess(Guid guid)
        {
            throw new NotImplementedException();
        }
        #endregion IErrorManagement

        #region ISettings
        private string _connectionString;


        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                    _connectionString = ConfigurationManager.
                        ConnectionStrings["MESConnectionString"].//HomeAssistantConnectionString
                        ConnectionString;
                return _connectionString;
            }
        }

        public string User
        {
            get
            {
                return "ToDo";
            }
        }
        #endregion ISettings

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion INotifyPropertyChanged

        public ICommand ShowAccounterDataCommand { get { return _showAccounterDataCommand; } }
        private void ShowAccounterData(object obj)
        {
            //ShowViewModel(typeof(AccounterDataViewModel), false, null);
        }
    }
}