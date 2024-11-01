using UTS_72220577.Data;

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

    private async void LoadCourses()
    {
        var courses = await _service.GetCoursesAsync();
        CoursesCollectionView.ItemsSource = courses;
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
                LoadCourses(); // Refresh the list after deletion
            }
        }
        else
        {
            await DisplayAlert("Warning", "Please select a course to delete.", "OK");
        }
    }

    private async void OnRefreshCoursesClicked(object sender, EventArgs e)
    {
        LoadCourses(); // Refresh the list of courses
    }
}
