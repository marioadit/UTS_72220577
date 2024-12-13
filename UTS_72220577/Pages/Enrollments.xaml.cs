using UTS_72220577.Data;

namespace UTS_72220577.Pages;

public partial class Enrollments : ContentPage
{
    private readonly ccService _service;

    public Enrollments()
    {
        InitializeComponent();
        _service = new ccService(new HttpClient());
        LoadEnrollments();
    }

    private async Task LoadEnrollments()
    {
        try
        {
            // Fetch enrollments data
            var enrollments = await _service.GetEnrollmentsAsync();
            var detailedEnrollments = enrollments.Select(e => new EnrollmentWithSelected
            {
                enrollmentId = e.enrollmentId,
                instructorId = e.instructorId,
                courseId = e.courseId,
                enrolledAt = e.enrolledAt,
                fullName = $"Instructor {e.instructorId}", // Adjust based on actual instructor data
                Name = $"Course {e.courseId}" // Adjust based on actual course data
            }).ToList();

            EnrollmentsCollectionView.ItemsSource = detailedEnrollments;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load enrollments: {ex.Message}", "OK");
        }
    }

    private async void OnCreateEnrollmentClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateEnrollment());
    }
}
