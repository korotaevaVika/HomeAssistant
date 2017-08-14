using HomeAssistant.Core.Integration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeAssistant.Core.ViewModel
{
    public class RelayCommand : BindableBase, ICommand
    {
        private string _displayName; //ddddd
        private bool hasUserAccess;
        private readonly IErrorManagement _errorManagement;
        private readonly Predicate<object> _canExecuteDelegate;
        private readonly Action<object> _executeDelegate;
        private readonly Func<object, object, Task> _executeDelegateAsync;

        //Конструктор
        public RelayCommand(IErrorManagement errorManagement, Action<object> action, Predicate<object> canexecute)
        {
            _errorManagement = errorManagement;
            _executeDelegate = action;
            _canExecuteDelegate = canexecute;
        }

        public RelayCommand(IErrorManagement errorManagement, Action<object> action)
            : this(errorManagement, action, null)
        {
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public bool HasUserAccess
        {
            get { return hasUserAccess; }
            set
            {
                if (hasUserAccess != value)
                {
                    hasUserAccess = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteDelegate != null)
            {
                return _canExecuteDelegate(parameter);
            }
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                _executeDelegate?.Invoke(parameter);
            }
            catch (Exception exc)
            {
                if (_errorManagement != null)
                {
                    _errorManagement.ShowError(exc);
                }
                else
                {
                    Trace.WriteLine(exc.Message);
                }
            }
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
