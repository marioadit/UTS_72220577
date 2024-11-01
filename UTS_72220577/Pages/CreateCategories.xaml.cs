using UTS_72220577.Data;

namespace UTS_72220577.Pages
{
    public partial class CreateCategories : ContentPage
    {
        private readonly ccService _service;

        public CreateCategories()
        {
            InitializeComponent();
            _service = new ccService(new HttpClient());
        }

        private async void OnSaveCategoryClicked(object sender, EventArgs e)
        {
            var name = nameEntry.Text;
            var description = descriptionEntry.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Error", "Category name cannot be empty.", "OK");
                return;
            }


            var newCategory = new category
            {
                name = name,
                description = description
            };

            // Call the service to add the new category
            await _service.AddCategoryAsync(newCategory);

            await DisplayAlert("Success", "Category added successfully!", "OK");
            await Navigation.PushAsync(new Categories());
        }
    }
}
