using UTS_72220577.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTS_72220577.Pages
{
    public partial class Categories : ContentPage
    {
        private readonly ccService _service;
        private List<category> _allCategories;

        public Categories()
        {
            InitializeComponent();
            _service = new ccService(new HttpClient());
            LoadCategories();
        }

        private async Task LoadCategories()
        {
            var categories = await _service.GetCategoriesAsync();
            _allCategories = categories.ToList(); // Store all categories for filtering
            CategoriesCollectionView.ItemsSource = _allCategories;
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
                bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the category '{selectedCategory.name}'?", "Yes", "No");
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
            SearchEntry.Text = string.Empty; // Clear the search bar
            await LoadCategories(); // Refresh the category list
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue?.ToLower() ?? string.Empty;

            // Filter categories by name
            CategoriesCollectionView.ItemsSource = _allCategories
                .Where(c => c.name?.ToLower().Contains(searchText) ?? false)
                .ToList();
        }
    }
}
