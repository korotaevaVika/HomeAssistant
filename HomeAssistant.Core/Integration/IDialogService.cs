using System.Windows;

namespace HomeAssistant.Core.Integration
{
    public interface IDialogService
    {
        MessageBoxResult ShowQuery(
            string caption,
            string text,
            MessageBoxButton buttons,
            MessageBoxImage image);
    }
}
