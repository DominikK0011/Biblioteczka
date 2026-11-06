using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Biblioteczka.UI.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteczka.UI.Service.ConcreteServices
{
    public class WindowService : IWindowService
    {
        public void CloseCurrentWindow()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive)?
                .Close();
        }

        public void ShowLoginWindow()
        {
            var loginWindow = App.Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }
        public void ShowRegisterWindow()
        {
            var registerWindow = App.Services.GetRequiredService<RegisterWindow>();
            registerWindow.Show();
        }
    }
}
