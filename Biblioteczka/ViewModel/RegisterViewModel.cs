using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Biblioteczka.Contracts.Request;
using Biblioteczka.UI.Commands;
using Biblioteczka.UI.Service.Interfaces;

namespace Biblioteczka.UI.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly IWindowService _windowService;

        public RegisterViewModel(IUserService userService, IWindowService windowService)
        {
            _userService = userService;
            _windowService = windowService;
            RegistryCommand = new AsyncRelayCommand(Registry);
            ShowLoginWindowCommand = new AsyncRelayCommand(ShowLoginWindow);
        }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Password2 { get; set; } = string.Empty;
        private string _errorMessage = string.Empty;
        public string ErrorMessage {
            get => _errorMessage;
            set {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public override string Title { get; set; } = "Register Panel";

        public ICommand RegistryCommand { get; }
        public ICommand ShowLoginWindowCommand { get; }

        public async Task Registry()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Username is empty";
                return;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Email is empty";
                return;
            }
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Password2))
            {
                ErrorMessage = "Passwords are empty";
                return;
            }
            if(Password != Password2)
            {
                ErrorMessage = "Password not the same";
                return;
            }

            var user = new RegisterUserRequest(Email, Username, Password);
            var result = await _userService.RegisterUserAsync(user);

            result.Match(
                onSuccess: _ => {
                    ErrorMessage = "Register Succesfull";
                    Thread.Sleep(500);
                    _windowService.ShowLoginWindow();
                    return true;
                },
                onFailure: err => {
                    ErrorMessage = err.Message;
                    return false; 
                });
        }
        public Task ShowLoginWindow()
        {
            _windowService.CloseCurrentWindow();
            _windowService.ShowLoginWindow();

            return Task.CompletedTask;
        }
    }
}
