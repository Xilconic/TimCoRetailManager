using Caliburn.Micro;

namespace TRMDesktopUI.ViewModels
{
    public class StatusInfoViewModel : Screen
    {
        public string Header { get; private set; }
        public string Message { get; private set; }

        public void UpdateMessage(string header, string message)
        {
            Header = header;
            Message = message;

            NotifyOfPropertyChange(nameof(Header));
            NotifyOfPropertyChange(nameof(Message));
        }

        public void Close()
        {
            TryClose();
        }
    }
}