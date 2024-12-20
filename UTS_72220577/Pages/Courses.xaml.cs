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
            // Log the error and display it
            await DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
            System.Diagnostics.Debug.WriteLine($"Error loading courses: {ex.Message}"); // Debug log for tracking the issue
        }
    }

    private async void OnEditCourseClicked(object sender, EventArgs e)
    {
        var selectedCourse = CoursesCollectionView.SelectedItem as course;
        if (selectedCourse != null)
        {
            try
            {
                await Navigation.PushAsync(new EditCourses(selectedCourse.courseId));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to navigate to edit course: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error navigating to edit course: {ex.Message}"); // Log
            }
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
                    System.Diagnostics.Debug.WriteLine($"Error deleting course: {ex.Message}"); // Log
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
        try
        {
            SearchEntry.Text = string.Empty; // Clear the search bar
            await LoadCourses(); // Refresh the list of courses
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to refresh courses: {ex.Message}", "OK");
            System.Diagnostics.Debug.WriteLine($"Error refreshing courses: {ex.Message}"); // Log
        }
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

                // Use LINQ to filter courses by name or description
                var query = filteredCourses.Where(c => c.name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                       c.category.name.Contains(searchText, StringComparison.OrdinalIgnoreCase));

                CoursesCollectionView.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to find courses: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error searching courses: {ex.Message}"); // Log
            }
        }
        else
        {
            await DisplayAlert("Warning", "Please enter a search term.", "OK");
        }
    }
}
