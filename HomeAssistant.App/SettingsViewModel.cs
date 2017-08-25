using HomeAssistant.Core.Integration;
using HomeAssistant.Core.ViewModel;
using System.Windows.Input;

namespace HomeAssistant.App
{
    public class SettingsViewModel : ViewModelBase
    {
        //IProductCalculator productCalculator;
        private readonly RelayCommand _materialGroupAddCommand;

        
        #region Properties

        #endregion


        #region Commands
        
        //public ICommand MaterialCancelCommand { get { return _materialCancelCommand; } }

        
        #endregion
        public SettingsViewModel(IDisplayManagement displayManagement, IDialogService dialogService, IErrorManagement errorManagement)
            : base(displayManagement, dialogService, null, errorManagement)
        {
            //if (productCalculator == null)
            //    throw new ArgumentNullException("productCalculator model is empty");
            //this.productCalculator = productCalculator;

        }
       
    }
}
