using System;

namespace HomeAssistant.Core.Integration
{
    public interface IDisplayManagement
    {
        bool ShowViewModel(Type viewModelType, bool showAsDialog, params object[] parameter);

        void CloseViewModel(bool result);
    }
}
