using System;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private readonly IUserEndpoint _userEndpoint;
        private BindingList<UserModel> _users;
        private UserModel _selectedUser;
        private string _selectedUserName;
        private BindingList<string> _userRoles = new BindingList<string>();
        private BindingList<string> _availableRoles = new BindingList<string>();
        private string _selectedRoleToRemove;
        private string _selectedRoleToAdd;

        public BindingList<UserModel> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                NotifyOfPropertyChange(nameof(Users));
            }
        }

        public UserModel SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                SelectedUserName = value.Email;
                UserRoles.Clear();
                UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles(); // TODO: Getting the full data should be done once, in OnViewLoaded? Going along with course...
                NotifyOfPropertyChange(nameof(SelectedUser));
            }
        }

        public BindingList<string> UserRoles
        {
            get
            {
                return _userRoles;
            }
            set
            {
                _userRoles = value;
                NotifyOfPropertyChange(nameof(UserRoles));
            }
        }

        public string SelectedUserName
        {
            get
            {
                return _selectedUserName;
            }
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(nameof(SelectedUserName));
            }
        }

        public BindingList<string> AvailableRoles
        {
            get
            {
                return _availableRoles;
            }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(nameof(AvailableRoles));
            }
        }

        public string SelectedUserRoles
        {
            get
            {
                return _selectedRoleToRemove;
            }
            set
            {
                _selectedRoleToRemove = value;
                NotifyOfPropertyChange(nameof(SelectedUserRoles));
            }
        }

        public string SelectedAvailableRole
        {
            get
            {
                return _selectedRoleToAdd;
            }
            set
            {
                _selectedRoleToAdd = value;
                NotifyOfPropertyChange(nameof(SelectedAvailableRole));
            }
        }

        public UserDisplayViewModel(
            StatusInfoViewModel status,
            IWindowManager window,
            IUserEndpoint userEndpoint)
        {
            _status = status;
            _window = window;
            _userEndpoint = userEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            // TODO: Concerned about async void, which is typically discouraged as it cannot ever be awaited for. Going along with course...
            // TODO: DRY - Code pattern duplicated from SalesViewModel. Going along with course...
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject(); // TODO: _window.ShowDialog prototype indicated dictionary to be used; Going along with course...
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized") // TODO: Not a fan of the straight message compare, as it's likely going to break for different culture; Going along with course...
                {
                    _status.UpdateMessage("Unauthorized Access", "You do not have permission to interact with the Sales form.");
                    _window.ShowDialog(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    _window.ShowDialog(_status, null, settings);
                }

                TryClose();
            }
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAllAsync();
            Users = new BindingList<UserModel>(userList);
        }

        private async Task LoadRoles()
        {
            var roles = await _userEndpoint.GetAllRolesAsync();
            foreach (var role in roles)
            {
                if (UserRoles.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        public async Task AddSelectedRole()
        {
            // TODO: Add error handling for endpoint call; Going along with course...
            await _userEndpoint.AddUserToRoleAsync(SelectedUser.Id, SelectedAvailableRole);

            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);
        }

        public async Task RemoveSelectedRole()
        {
            // TODO: Add error handling for endpoint call; Going along with course...
            await _userEndpoint.RemoveUserFromRoleAsync(SelectedUser.Id, SelectedUserRoles);

            UserRoles.Remove(SelectedUserRoles);
            AvailableRoles.Add(SelectedUserRoles);
        }
    }
}