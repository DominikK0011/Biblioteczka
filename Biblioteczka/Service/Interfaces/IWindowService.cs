using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteczka.UI.Service.Interfaces
{
    public interface IWindowService
    {
        public void ShowLoginWindow();
        public void ShowRegisterWindow();
        public void CloseCurrentWindow();
    }
}
