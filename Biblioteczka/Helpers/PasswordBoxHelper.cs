using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Biblioteczka.UI.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty = DependencyProperty.RegisterAttached(
            "BoundPassword", typeof(string), typeof(PasswordBoxHelper),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnBoundPasswordChanged));
        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }
        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }
        private static void OnBoundPasswordChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            if (obj is not PasswordBox passwordbox)
                return;

            passwordbox.PasswordChanged -= PasswordBox_PasswordChanged;
            string newPassword = arg.NewValue?.ToString() ?? string.Empty;

            if(passwordbox.Password != newPassword)
            {
                passwordbox.Password = newPassword;
            }

            passwordbox.PasswordChanged += PasswordBox_PasswordChanged;

        }
        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not PasswordBox passwordbox)
                return;

            SetBoundPassword(passwordbox, passwordbox.Password);

            BindingExpression? binding = passwordbox.GetBindingExpression(BoundPasswordProperty);

            binding?.UpdateSource();
        }
    }
}
