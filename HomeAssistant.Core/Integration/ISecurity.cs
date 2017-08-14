using System;

namespace HomeAssistant.Core.Integration
{
    public interface ISecurity
    {
        bool CheckAccess(Guid guid);
        string User { get; }
    }
}
