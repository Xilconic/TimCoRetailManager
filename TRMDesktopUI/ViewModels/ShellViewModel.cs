using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly IEventAggregator _events;
        private readonly SalesViewModel _salesViewModel;
        private readonly ILoggedInUserModel _user;

        public ShellViewModel(
            IEventAggregator events,
            SalesViewModel salesViewModel,
            ILoggedInUserModel user)
        {
            _events = events;
            _events.Subscribe(this);

            _salesViewModel = salesViewModel;
            _user = user;

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrWhiteSpace(_user.Token))
                {
                    output = true;
                }

                return output;
            }
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            // TODO: This doesn't clear credentials of previous user due to LoginViewModel instance being reused; Going along with course for now...
            NotifyOfPropertyChange(nameof(IsLoggedIn));
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesViewModel);
            NotifyOfPropertyChange(nameof(IsLoggedIn));
        }
    }
}