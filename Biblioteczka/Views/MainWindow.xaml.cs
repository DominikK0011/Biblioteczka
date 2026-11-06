using System.Windows;
using System.Windows.Controls;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.UI;
using Biblioteczka.UI.Service.ConcreteServices;
using Biblioteczka.UI.Service.Interfaces;
using Biblioteczka.UI.Session;
using Biblioteczka.UI.ViewModel;

namespace Biblioteczka.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}