using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Shared;
using Biblioteczka.UI.Commands;
using Biblioteczka.UI.Service.Interfaces;

namespace Biblioteczka.UI.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly IWindowService _windowService;

        public LoginViewModel(IUserService userService, IWindowService windowService)
        {
            _userService = userService;
            _windowService = windowService;
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            ShowRegisterWindowCommand = new AsyncRelayCommand(ShowRegisterWindowAsync);
        }

        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
        private string _errorMessage = string.Empty;
        public ICommand LoginCommand { get; }
        public ICommand ShowRegisterWindowCommand { get; }

        public string ErrorMessage { 
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public override string Title { get; set; } = "Login Panel";

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Password is empty";
                return;
            }
            var loginUserData = new LoginUserRequest(Login, Password);

            var response = await _userService.LoginUserAsync(loginUserData);

            if (!response.IsSuccess)
            {
                ErrorMessage = response.Error?.Message;
            }
            else
            {
                _windowService.CloseCurrentWindow();
            }

        }
        private async Task ShowRegisterWindowAsync()
        {
            _windowService.CloseCurrentWindow();
            _windowService.ShowRegisterWindow();
        }
    }
}
