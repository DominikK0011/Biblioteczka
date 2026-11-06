using System.Configuration;
using System.Data;
using System.Windows;
using Biblioteczka.UI;
using Biblioteczka.UI.Service.ConcreteServices;
using Biblioteczka.UI.Service.Interfaces;
using Biblioteczka.UI.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteczka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;
        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<IMovieService, MovieService>();
            services.AddSingleton<IApiClient, ApiClient>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<MainViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<RegisterWindow>();

            Services = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
