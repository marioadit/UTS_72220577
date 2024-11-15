using UTS_72220577.Data;
using System.Net.Http;

namespace UTS_72220577.Pages
{
    public partial class EditCourses : ContentPage
    {
        private readonly int _courseId;
        private readonly ccService _ccService;
        private int _selectedCategoryId;

        public EditCourses(int courseId)
        {
            InitializeComponent();
            _courseId = courseId;
            _ccService = new ccService(new HttpClient());
            LoadCourseData();
            LoadCategories();
        }

        private async void LoadCourseData()
        {
            var course = await _ccService.GetCourseByIdAsync(_courseId);
            if (course != null)
            {
                courseIdLabel.Text = course.courseId.ToString();
                nameEntry.Text = course.name;
                imageNameEntry.Text = course.imageName;
                durationEntry.Text = course.duration.ToString();
                descriptionEditor.Text = course.description;
                _selectedCategoryId = course.categoryId;
            }
        }

        private async void LoadCategories()
        {
            var categories = await _ccService.GetCategoriesAsync();
            categoryPicker.ItemsSource = categories.ToList();
            categoryPicker.ItemDisplayBinding = new Binding("name");

            var selectedCategory = categories.FirstOrDefault(c => c.categoryId == _selectedCategoryId);
            if (selectedCategory != null)
            {
                categoryPicker.SelectedItem = selectedCategory;
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var selectedCategory = categoryPicker.SelectedItem as category;

            if (selectedCategory != null)
            {
                var updatedCourse = new course
                {
                    courseId = _courseId,
                    name = nameEntry.Text,
                    imageName = imageNameEntry.Text,
                    duration = int.Parse(durationEntry.Text),
                    description = descriptionEditor.Text,
                    categoryId = selectedCategory.categoryId
                };

                await _ccService.UpdateCourseAsync(updatedCourse);
                await DisplayAlert("Success", "Course updated successfully.", "OK");
                await Shell.Current.GoToAsync("//courses");
            }
            else
            {
                await DisplayAlert("Error", "Please select a category.", "OK");
            }
        }
    }
}
