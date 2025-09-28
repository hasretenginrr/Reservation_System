using Reservation_SystemWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Reservation_SystemWPF.ViewModels
{
    public class SignupViewModel : ObservableObject
    {
       
        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

  
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand SignUpCommand { get; }

        public SignupViewModel()
        {
            SignUpCommand = new RelayCommand(async () => await SignUp());
        }

        private async Task SignUp()
        {
            var httpClient = new HttpClient();

            var UserDto = new
            {
                Username = _username,
                Mail = _email,
                Phone = _phone
            };

            try
            {
                var response = await httpClient.PostAsJsonAsync("https://localhost:7067/api/user", UserDto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Kayıt Başarılı, Giriş Yapabilirsin.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }
                else
                {
                    // FluentValidation hatalarını içeren response modeline uygun şekilde deserialize et
                    //var errorResponse = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

                    //if (errorResponse != null && errorResponse.Errors != null)
                    //{
                    //    string allErrors = string.Join("\n", errorResponse.Errors);
                    //    MessageBox.Show(allErrors, "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    MessageBox.Show("Kayıt başarısız.");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorContent); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
    }
    
    }
