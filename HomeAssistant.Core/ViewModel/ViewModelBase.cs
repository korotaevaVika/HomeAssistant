using HomeAssistant.Core.Integration;
using System;
using System.Windows.Input;

namespace HomeAssistant.Core.ViewModel
{
    public class ViewModelBase : BindableBase
    {
        protected IDisplayManagement DisplayManagement { get; private set; }
        protected IDialogService DialogService { get; private set; }
        protected IErrorManagement ErrorManagement { get; private set; }
        protected ISecurity Security { get; private set; }

        private readonly RelayCommand _generalRefreshStateErrorCommand;
        private readonly RelayCommand _generalRefreshStateReloadCommand;
        private readonly RelayCommand _generalRefreshStateCancelCommand;

        public ViewModelBase(
            IDisplayManagement displayManagement, 
            IDialogService dialogService, 
            ISecurity security,
            IErrorManagement errorManagement)
        {
            if (displayManagement == null)
            {
                throw new ArgumentNullException("displayManagement");
            }
            if (dialogService == null)
            {
                throw new ArgumentNullException("dialogService");
            }
            if (errorManagement == null)
            {
                throw new ArgumentNullException("errorManagement");
            }
            //if (security == null)
            //{
            //    throw new ArgumentNullException("security");
            //}
            DisplayManagement = displayManagement;//?? throw new ArgumentNullException("displayManagement");
            DialogService = dialogService;// ?? throw new ArgumentNullException("dialogService");
            ErrorManagement = errorManagement;//?? throw new ArgumentNullException("errorManagement");
            Security = security;

            _generalRefreshStateErrorCommand = new RelayCommand(this.ErrorManagement, GeneralRefreshStateShowError);
            _generalRefreshStateReloadCommand = new RelayCommand(this.ErrorManagement, GeneralRefreshStateReload);
            _generalRefreshStateCancelCommand = new RelayCommand(this.ErrorManagement, GeneralRefreshStateCancel);

        }

        //protected int? ToNullableInt(string s)
        //{
        //    int i;
        //    if (int.TryParse(s, out i)) return i;
        //    return null;
        //}

        private NotifyTaskCompletion _generalRefreshState;
        public NotifyTaskCompletion GeneralRefreshState
        {
            get
            {
                return _generalRefreshState;
            }
            set
            {
                _generalRefreshState = value;
                this.OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public ICommand GeneralRefreshStateErrorCommand
        { get { return _generalRefreshStateErrorCommand; } }
        private void GeneralRefreshStateShowError(object obj)
        {
            this.ErrorManagement.ShowError(GeneralRefreshState.Exception);
        }

        public ICommand GeneralRefreshStateReloadCommand
        { get { return _generalRefreshStateReloadCommand; } }
        protected virtual void GeneralRefreshStateReload(object obj)
        {
        }

        public ICommand GeneralRefreshStateCancelCommand
        { get { return _generalRefreshStateCancelCommand; } }
        private void GeneralRefreshStateCancel(object obj)
        {
            GeneralRefreshState = null;
        }

    }

}
