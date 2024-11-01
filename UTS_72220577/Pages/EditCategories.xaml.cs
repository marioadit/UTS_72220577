using UTS_72220577.Data;

namespace UTS_72220577.Pages;

public partial class EditCategories : ContentPage
{
    private readonly ccService _service;
    private category _category;

    public EditCategories(int categoryId)
    {
        InitializeComponent();
        _service = new ccService(new HttpClient());
        LoadCategoryDetails(categoryId);
    }

    private async void LoadCategoryDetails(int categoryId)
    {
        // Fetch the category details using the service
        _category = await _service.GetCategoryByIdAsync(categoryId);
        if (_category != null)
        {
            // Populate the UI with the category details
            categoryIdEntry.Text = _category.categoryId.ToString(); // Display the ID (read-only)
            nameEntry.Text = _category.name; // Display the current name
            descriptionEntry.Text = _category.description; // Display the current description
        }
    }

    private async void OnUpdateCategoryClicked(object sender, EventArgs e)
    {
        // Update the category properties from the UI inputs
        if (_category != null)
        {
            _category.name = nameEntry.Text; // Update the name
            _category.description = descriptionEntry.Text; // Update the description

            // Call the service to update the category
            await _service.UpdateCategoryAsync(_category);

            // Optionally navigate back to the Categories page or show a success message
            await DisplayAlert("Success", "Category updated successfully!", "OK");
            await Navigation.PushAsync(new Categories());
        }
    }
}
