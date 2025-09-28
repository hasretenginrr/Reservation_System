using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Reservation_SystemWPF.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();

            

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Phone = ((PasswordBox)sender).Password;
            }
        }
        private void SignUp_Click(object sender, MouseButtonEventArgs e)
        {
            // Yeni pencere oluştur
            var signupWindow = new SignupWindow();

            // Pencereyi aç
            signupWindow.Show();

            // Bu pencereyi kapatmak istiyorsan (LoginView bir UserControl olduğu için genelde kapatma yapılmaz)
            // Eğer LoginView bir Window içindeyse şöyle yapılabilir:
            // Window.GetWindow(this)?.Close();
        }




    }
}