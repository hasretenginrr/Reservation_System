using Reservation_SystemWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Reservation_SystemWPF.Views
{
    public partial class SignupWindow : Window
    {
        public SignupWindow()
        {
            InitializeComponent();
            DataContext = new SignupViewModel();
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var loginWindow = new MainWindow();
            loginWindow.Show();
        }
    }

}
