using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.Helpers;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IApiHelper _apiHelper;
        private string _userName;
        private string _password;
        private string _errorMessage;

        public LoginViewModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

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

        /// <remarks>
        /// <para>
        /// Binding a plain string to a <see cref="System.Windows.Controls.PasswordBox"/>
        /// introduces a vulnerability of exposing sensitive data (through RAM, or poor
        /// programming choices later down the line on the usage of this property).
        /// </para>
        /// <para>
        /// In the video https://www.youtube.com/watch?v=3eIc68VWxgE Tim Corey expresses
        /// that the Caliburn.Micro framework usage patterns requires breaking the security
        /// open here. Not sure if that's completely true.
        /// Secondly in that same video he also mentions that the plain text password still
        /// needs to be sent over HTTPS towards the TimCo Retail Manager Api.
        /// </para>
        /// </remarks>
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

        public bool IsErrorVisible => ErrorMessage?.Length > 0;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(nameof(ErrorMessage));
                NotifyOfPropertyChange(nameof(IsErrorVisible));
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = string.Empty;
                var result = await _apiHelper.AuthenticateAsync(UserName, Password);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }
    }
}