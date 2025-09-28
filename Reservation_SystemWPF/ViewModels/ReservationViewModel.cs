using Reservation_SystemWPF.Models;
using Reservation_SystemWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Reservation_SystemWPF.ViewModels
{
    public class ReservationViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;

        public ReservationViewModel()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7067/api/")
            };
            _ = LoadCategories();
            CreateReservationCommand = new RelayCommand(() => CreateReservation().ConfigureAwait(false));

        }


        private List<CategoryDto> _categories;
        public List<CategoryDto> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories)); 
            }
        }
        private CategoryDto _selectedCategory;
        public CategoryDto SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }
        private int _categoryId;
        public int CategoryId
        {
            get => _categoryId;
            set { _categoryId = value; OnPropertyChanged(); }
        }
        private DateTime _reservationDate = DateTime.Now;
        public DateTime ReservationDate
        {
            get => _reservationDate;
            set { _reservationDate = value; OnPropertyChanged(); }
        }

        private int _peopleCount;
        public int PeopleCount
        {
            get => _peopleCount;
            set { _peopleCount = value; OnPropertyChanged(); }
        }

        public ICommand CreateReservationCommand { get; }

        private async Task CreateReservation()
        {
            try
            {
                var reservationDto = new ReservationDto
                {
                    // UserId = null --> API login kullanıcıdan alacak
                    CategoryId = CategoryId,
                    ReservationDate = ReservationDate,
                    PeopleCount = PeopleCount
                };

                var response = await _httpClient.PostAsJsonAsync("Reservation", reservationDto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Rezervasyon başarıyla oluşturuldu.");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hata: {error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private async Task LoadCategories()
        {
            try
            {
                var response = await _httpClient.GetAsync("Category"); 
                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
                    Categories = categories ?? new List<CategoryDto>();
                }
                else
                {
                    MessageBox.Show("Kategoriler yüklenemedi.");
                    Categories = new List<CategoryDto>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
                Categories = new List<CategoryDto>();
            }
        }


    }
}

