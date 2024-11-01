using UTS_72220577.Data;
using System.Net.Http;

namespace UTS_72220577.Pages
{
    public partial class CreateCourses : ContentPage
    {
        private readonly ccService _ccService;

        public CreateCourses()
        {
            InitializeComponent();
            _ccService = new ccService(new HttpClient());
            LoadCategories();
        }

        private async void LoadCategories()
        {
            // Fetch available categories from the service
            var categories = await _ccService.GetCategoriesAsync();
            categoryPicker.ItemsSource = categories.ToList();
            categoryPicker.ItemDisplayBinding = new Binding("name");
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Get selected category from Picker
            var selectedCategory = categoryPicker.SelectedItem as category;

            if (selectedCategory != null)
            {
                // Create a new course object with entered data
                var newCourse = new course
                {
                    name = nameEntry.Text,
                    imageName = imageNameEntry.Text,
                    duration = int.TryParse(durationEntry.Text, out int duration) ? duration : 0,
                    description = descriptionEditor.Text,
                    categoryId = selectedCategory.categoryId // Store only the category ID
                };

                await _ccService.AddCourseAsync(newCourse);
                await DisplayAlert("Success", "Course created successfully.", "OK");
                await Navigation.PushAsync(new Categories());
            }
            else
            {
                await DisplayAlert("Error", "Please select a category.", "OK");
            }
        }
    }
}
