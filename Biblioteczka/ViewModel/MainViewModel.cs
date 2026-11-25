using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Shared;
using Biblioteczka.UI.Commands;
using Biblioteczka.UI.Service.Interfaces;
using Biblioteczka.UI.Session;
using Microsoft.Win32;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.UI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMovieService _movieServices;
        private readonly IWindowService _windowService;
        private readonly IUserService _userService;
        public ObservableCollection<MovieDto> Movies { get; } = new();
        public MainViewModel(IUserService userService, IMovieService movieServices, IWindowService windowService)
        {
            _userService = userService;
            _movieServices = movieServices;
            _windowService = windowService;

            LoadMoviesCommand = new AsyncRelayCommand(LoadMoviesAsync);
            LoadHomeCommand = new AsyncRelayCommand(LoadHomePageAsync);
            ChangeAvatarCommand = new AsyncRelayCommand(ChangeAvatarAsync);
            ShowLoginWindowCommand = new AsyncRelayCommand(OpenLoginWindowAsync);
            ShowRegisterWindowCommand = new AsyncRelayCommand(OpenRegisterWindowAsync);
            LogoutCommand = new AsyncRelayCommand(LogoutAsync);

            UserSession.SessionChanged += OnSessionChanged;
        }

        public bool IsLoggedIn => UserSession.IsLogged;
        public bool IsLoggedOut => !UserSession.IsLogged;
        public bool IsAdmin => UserSession.ActualUser?.Role == UserRole.Admin;
        public string Username => UserSession.ActualUser?.Username ?? "";
        public string UserAvatarUrl => UserSession.ActualUser?.AvatarUrl ?? "uploads/avatars/default.jpg";
        public ICommand LoadMoviesCommand { get; }
        public ICommand ShowLoginWindowCommand { get; }
        public ICommand ShowRegisterWindowCommand { get; }
        public ICommand LoadHomeCommand { get; }
        public ICommand ChangeAvatarCommand { get; }
        public ICommand LogoutCommand { get; }
        public override string Title { get; set; } = "Main Page";
        private string _currentPage = "Home";
        public string CurrentPage { 
            get { return _currentPage; }
            set { 
                _currentPage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsHomePage));
                OnPropertyChanged(nameof(IsMoviePage));
            }
        }
        public bool IsHomePage => _currentPage == "Home";
        public bool IsMoviePage => _currentPage == "Movie";

        public async Task LoadMoviesAsync()
        {
            List<MovieDto> movies = await _movieServices.GetMoviesAsync();

            Movies.Clear();

            foreach (var m in movies)
            {
                Movies.Add(m);
            }
            ChangeTextColourToGrey();
            MovieTextColour = Brushes.White;
            CurrentPage = "Movie";
        }
        public async Task LoadHomePageAsync()
        {
            ChangeTextColourToGrey();
            HomeTextColour = Brushes.White;
            Movies.Clear();
            CurrentPage = "Home";
        }
        public async Task<string> SelectPhotoAsync()
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Choose photo",
                Filter = "Images file |*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() != true)
                return string.Empty;

            string filePath = dialog.FileName;
            return filePath;
        }
        public async Task ChangeAvatarAsync()
        {
            var filePath = await SelectPhotoAsync();

            if (string.IsNullOrWhiteSpace(filePath))
                return;

            var response = await _userService.ChangeAvatarAsync(filePath);

            response.Match(
                onSuccess: url => { 
                    UserSession.UpdateUserAvatar(url);
                    return true;
                },
                onFailure: err =>
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                );
        }
        public async Task LogoutAsync()
        {
            ChangeTextColourToGrey();

            if (IsLoggedIn)
                UserSession.Logout();
        }
        private Task OpenLoginWindowAsync()
        {
            _windowService.ShowLoginWindow();
            return Task.CompletedTask;
        }
        private Task OpenRegisterWindowAsync()
        {
            _windowService.ShowRegisterWindow();
            return Task.CompletedTask;
        }
        public void OnSessionChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsLoggedOut));
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(UserAvatarUrl));
        }

        private Brush homeTextColour = Brushes.White;
        private Brush movieTextColour = Brushes.Gray;
        private Brush favoritiesTextColour = Brushes.Gray;
        private Brush reviewsTextColour = Brushes.Gray;
        private Brush profileTextColour = Brushes.Gray;

        public Brush HomeTextColour {
            get => homeTextColour;
            set
            {
                homeTextColour = value;
                OnPropertyChanged(nameof(HomeTextColour));
            }
        }
        public Brush MovieTextColour { 
            get => movieTextColour; 
            set
            {
                movieTextColour = value;
                OnPropertyChanged(nameof(MovieTextColour));
            }
        }
        public Brush FavoritiesTextColour { 
            get => favoritiesTextColour; 
            set 
            {
                favoritiesTextColour = value;
                OnPropertyChanged(nameof(FavoritiesTextColour));
            }
        }
        public Brush ReviewsTextColour { 
            get => reviewsTextColour; 
            set
            {
                reviewsTextColour = value;
                OnPropertyChanged(nameof(ReviewsTextColour));
            } 
        }
        public Brush ProfileTextColour {
            get => profileTextColour; 
            set
            {
                profileTextColour = value;
                OnPropertyChanged(nameof(ProfileTextColour));
            }
        }
        public void ChangeTextColourToGrey()
        {
            HomeTextColour = Brushes.Gray;
            MovieTextColour = Brushes.Gray;
            FavoritiesTextColour = Brushes.Gray;
            ReviewsTextColour = Brushes.Gray;
            ProfileTextColour = Brushes.Gray;
        }
    }
}
