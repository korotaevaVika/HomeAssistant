using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HomeAssistant.Core.ViewModel
{
    public class NotifyTaskCompletion : INotifyPropertyChanged
    {
        private readonly Action _onComplete;
        public NotifyTaskCompletion(Task task, Action onComplete = null)
        {
            _onComplete = onComplete;
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
            else
            {
                this.OnTaskComplete(task);
            }
        }
        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
            this.OnTaskComplete(task);
        }
        private void OnTaskComplete(Task task)
        {
            _onComplete?.Invoke();
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("IsRunning"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));
            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this,
                  new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
            }
            else
            {
                propertyChanged(this,
                  new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }
        public Task Task { get; private set; }

        public TaskStatus Status { get { return Task.Status; } }
        public bool IsRunning { get { return Task.Status == TaskStatus.Running; } }
        public bool IsCompleted { get { return Task.IsCompleted; } }
        public bool IsNotCompleted { get { return !Task.IsCompleted; } }
        public bool IsSuccessfullyCompleted
        {
            get
            {
                return Task.Status ==
                    TaskStatus.RanToCompletion;
            }
        }
        public bool IsCanceled { get { return Task.IsCanceled; } }
        public bool IsFaulted { get { return Task.IsFaulted; } }
        public AggregateException Exception { get { return Task.Exception; } }
        public Exception InnerException
        {
            get
            {
                return (Exception == null) ?
                    null : Exception.InnerException;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return (InnerException == null) ?
                    null : InnerException.Message;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public sealed class NotifyTaskCompletion<TResult> : NotifyTaskCompletion
    {
        public NotifyTaskCompletion(Task<TResult> task)
            : base(task)
        {
        }
        public new Task<TResult> Task { get; private set; }
        public TResult Result
        {
            get
            {
                return (Task.Status == TaskStatus.RanToCompletion) ?
                    Task.Result : default(TResult);
            }
        }
    }
}
