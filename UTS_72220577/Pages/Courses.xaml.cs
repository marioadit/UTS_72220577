using UTS_72220577.Data;
using System.Linq;

namespace UTS_72220577.Pages;

public partial class Courses : ContentPage
{
    private readonly ccService _service;

    public Courses()
    {
        InitializeComponent();
        _service = new ccService(new HttpClient());
        LoadCourses();
    }

    private async Task LoadCourses()
    {
        try
        {
            var courses = await _service.GetCoursesAsync();
            CoursesCollectionView.ItemsSource = courses.ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
        }
    }

    private async void OnEditCourseClicked(object sender, EventArgs e)
    {
        var selectedCourse = CoursesCollectionView.SelectedItem as course;
        if (selectedCourse != null)
        {
            await Navigation.PushAsync(new EditCourses(selectedCourse.courseId));
        }
        else
        {
            await DisplayAlert("Warning", "Please select a course to edit.", "OK");
        }
    }

    private async void OnDeleteCourseClicked(object sender, EventArgs e)
    {
        var selectedCourse = CoursesCollectionView.SelectedItem as course;
        if (selectedCourse != null)
        {
            bool confirmed = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the course '{selectedCourse.name}'?", "Yes", "No");
            if (confirmed)
            {
                try
                {
                    await _service.DeleteCourseAsync(selectedCourse.courseId);
                    await LoadCourses(); // Refresh the list after deletion
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete the course: {ex.Message}", "OK");
                }
            }
        }
        else
        {
            await DisplayAlert("Warning", "Please select a course to delete.", "OK");
        }
    }

    private async void OnRefreshCoursesClicked(object sender, EventArgs e)
    {
        SearchEntry.Text = string.Empty; // Clear the search bar
        await LoadCourses(); // Refresh the list of courses
    }

    private async void OnFindButtonClicked(object sender, EventArgs e)
    {
        string searchText = SearchEntry.Text?.Trim();
        if (!string.IsNullOrEmpty(searchText))
        {
            try
            {
                // Fetch courses by name using the API
                var filteredCourses = await _service.GetCoursesByNameAsync(searchText);
                CoursesCollectionView.ItemsSource = filteredCourses;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to find courses: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Warning", "Please enter a search term.", "OK");
        }
    }
}
