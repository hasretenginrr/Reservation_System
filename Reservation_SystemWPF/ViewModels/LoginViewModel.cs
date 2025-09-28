using Reservation_SystemWPF.Models;
using Reservation_SystemWPF.ViewModels.Base;
using Reservation_SystemWPF.Views;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

public class LoginViewModel : ObservableObject
{
    private string _username;
    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(nameof(Username)); }
    }

    private string _phone;
    public string Phone
    {
        get => _phone;
        set { _phone= value; OnPropertyChanged(nameof(Phone)); }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(Login);
    }

    private async void Login()
    {
       
        var loginDto = new LoginDto
        {
            UserName = _username, 
            Phone = _phone         
        };

        var client = new HttpClient();
        var response = await client.PostAsJsonAsync("https://localhost:7067/api/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
            Application.Current.Properties["UserId"] = result.Result.UserId;
            Application.Current.Properties["Username"] = result.Result.UserName;

            MessageBox.Show($"Hoş geldiniz {result.Result.UserName}");


            var reservationWindow = new ReservationWindow();
            reservationWindow.Show();

            // MainWindow'daki login user control'ü gizle
            if (Application.Current.MainWindow.Content is Grid mainGrid)
            {
                var loginControl = mainGrid.Children.OfType<LoginView>().FirstOrDefault();
                if (loginControl != null)
                    loginControl.Visibility = Visibility.Collapsed;
            }
        }
        else
        {
            MessageBox.Show("Giriş başarısız.");
        }
    }

    
}
