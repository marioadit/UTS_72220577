using UTS_72220577.Data;

namespace UTS_72220577.Pages;

public partial class CreateEnrollment : ContentPage
{
    private readonly ccService _service;

    public CreateEnrollment()
    {
        InitializeComponent();
        _service = new ccService(new HttpClient());
    }

    private async void OnSaveEnrollmentClicked(object sender, EventArgs e)
    {
        try
        {
            var enrollment = new enrollments
            {
                instructorId = int.Parse(InstructorEntry.Text),
                courseId = int.Parse(CourseEntry.Text),
                enrolledAt = EnrolledAtEntry.Text
            };

            await _service.AddEnrollmentAsync(enrollment);
            await DisplayAlert("Success", "Enrollment created successfully.", "OK");

            await Navigation.PopAsync(); // Return to the enrollments page
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to create enrollment: {ex.Message}", "OK");
        }
    }
}
