using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Windows.Controls;
using Biblioteczka.Contracts.DTO;

namespace Biblioteczka.UI.Session
{
    public static class UserSession
    {
        public static string? Token { get; private set; }
        public static UserDto? ActualUser { get; private set; }
        public static bool IsLogged => !string.IsNullOrEmpty(Token);
        public static Action? SessionChanged;
        
        public static void Login(string token, UserDto user)
        {
            Token = token;
            ActualUser = user;

            SessionChanged?.Invoke();
        }
        public static void Logout()
        {
            Token = string.Empty;
            ActualUser = null;

            SessionChanged?.Invoke();
        }
        public static void UpdateUserAvatar(string avatarUrl)
        {
            if(ActualUser != null)
            {
                ActualUser.AvatarUrl = avatarUrl;
                SessionChanged?.Invoke();
            }
        }
    }
}
