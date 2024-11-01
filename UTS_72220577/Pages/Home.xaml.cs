using UTS_72220577.Data;
using System.Collections.ObjectModel;

namespace UTS_72220577.Pages
{
    public partial class Home : ContentPage
    {
        private readonly ccService _ccService;
        private ObservableCollection<course> _allCourses;

        public Home()
        {
            InitializeComponent();
            _ccService = new ccService(new HttpClient());
            LoadCourses();
        }

        private async void LoadCourses()
        {
            var courses = await _ccService.GetCoursesAsync();
            _allCourses = new ObservableCollection<course>(courses);
            CoursesCollectionView.ItemsSource = _allCourses;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // Filter courses based on search query
            var searchText = e.NewTextValue?.ToLower() ?? string.Empty;
            CoursesCollectionView.ItemsSource = _allCourses.Where(c =>
                (c.name?.ToLower().Contains(searchText) ?? false) ||
                (c.category?.name?.ToLower().Contains(searchText) ?? false)).ToList();
        }
        private async void OnLogOutButtonClicked(object sender, EventArgs e)
        {
            bool confirmLogout = await DisplayAlert("Log Out", "Do you really want to log out?", "Yes", "No");

            if (confirmLogout)
            {
                // Navigate to the Login page if user confirms
                await Navigation.PushAsync(new Login());
            }
            // If the user selects "No", nothing happens and they stay on the current page
        }
        private async void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            SearchEntry.Text = string.Empty;
            LoadCourses();
        }

    }
}
