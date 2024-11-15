using UTS_72220577.Data;
using System;
using System.Linq;

namespace UTS_72220577.Pages
{
    public partial class Categories : ContentPage
    {
        private readonly ccService _service;

        public Categories()
        {
            InitializeComponent();
            _service = new ccService(new HttpClient());
            LoadCategories();
        }

        private async Task LoadCategories()
        {
            var categories = await _service.GetCategoriesAsync();
            CategoriesCollectionView.ItemsSource = categories;
        }

        private async void OnEditCategoryClicked(object sender, EventArgs e)
        {
            var selectedCategory = CategoriesCollectionView.SelectedItem as category;

            if (selectedCategory != null)
            {
                // Navigate to the EditCategories page with the category ID as a parameter
                await Navigation.PushAsync(new EditCategories(selectedCategory.categoryId));
            }
            else
            {
                await DisplayAlert("Warning", "Please select a category to edit.", "OK");
            }
        }

        private async void OnDeleteCategoryClicked(object sender, EventArgs e)
        {
            var selectedCategory = CategoriesCollectionView.SelectedItem as category;
            if (selectedCategory != null)
            {
                bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this category?", "Yes", "No");
                if (confirm)
                {
                    await _service.DeleteCategoryAsync(selectedCategory.categoryId);
                    await LoadCategories(); // Refresh the category list after deletion
                }
            }
            else
            {
                await DisplayAlert("Warning", "Please select a category to delete.", "OK");
            }
        }

        private async void OnRefreshCategoriesClicked(object sender, EventArgs e)
        {
            await LoadCategories(); // Refresh the category list
        }
    }
}
