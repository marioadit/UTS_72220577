using UTS_72220577.Data;
using System.Linq;

namespace UTS_72220577.Pages;

public partial class Courses : ContentPage
{
    private readonly ccService _service;
    private List<course> _allCourses;

    public Courses()
    {
        InitializeComponent();
        _service = new ccService(new HttpClient());
        LoadCourses();
    }

    private async Task LoadCourses()
    {
        var courses = await _service.GetCoursesAsync();
        _allCourses = courses.ToList(); // Convert to List<course> if necessary
        CoursesCollectionView.ItemsSource = _allCourses;
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
                await _service.DeleteCourseAsync(selectedCourse.courseId);
                await LoadCourses(); // Refresh the list after deletion
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

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        // Filter courses by name or category name
        CoursesCollectionView.ItemsSource = _allCourses
            .Where(c =>
                (c.name?.ToLower().Contains(searchText) ?? false) ||
                (c.category?.name?.ToLower().Contains(searchText) ?? false))
            .ToList();
    }
}
