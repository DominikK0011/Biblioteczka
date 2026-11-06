using System.Windows;
using Biblioteczka.UI.Service.ConcreteServices;
using Biblioteczka.UI.ViewModel;

namespace Biblioteczka.UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
