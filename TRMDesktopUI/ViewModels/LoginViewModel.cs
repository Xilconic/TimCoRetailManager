using Caliburn.Micro;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(nameof(UserName));
                NotifyOfPropertyChange(nameof(CanLogIn));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(nameof(Password));
                NotifyOfPropertyChange(nameof(CanLogIn));
            }
        }

        public bool CanLogIn =>
            UserName?.Length > 0 &&
            Password?.Length > 0;

        public void LogIn()
        {
        }
    }
}