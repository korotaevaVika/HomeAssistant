using System;

namespace HomeAssistant.Core.Integration
{
    public interface IErrorManagement
    {
        void ShowError(Exception error);
    }
}
