using UTS_72220577.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

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
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            try
            {
                // Attempt to load categories from the API
                var categories = await _service.GetCategoriesAsync().ConfigureAwait(false);

                // If categories are null or empty, display an alert
                if (categories == null || !categories.Any())
                {
                    await DisplayAlert("No Categories", "No categories available.", "OK");
                    return;
                }

                // Populate the collection view with the loaded categories
                _allCategories = categories.ToList();

                // Ensure we're on the UI thread when updating ItemsSource
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CategoriesCollectionView.ItemsSource = _allCategories;
                });
            }
            catch (Exception ex)
            {
                // If an error occurs, display the error message
                await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }
        }


        private async void OnEditCategoryClicked(object sender, EventArgs e)
        {
            var selectedCategory = CategoriesCollectionView.SelectedItem as category;
            if (selectedCategory != null)
            {
                // Navigate to the EditCategories page for the selected category
                await Navigation.PushAsync(new EditCategories(selectedCategory.categoryId));
            }
            else
            {
                // If no category is selected, show a warning
                await DisplayAlert("Warning", "Please select a category to edit.", "OK");
            }
        }

        private async void OnDeleteCategoryClicked(object sender, EventArgs e)
        {
            var selectedCategory = CategoriesCollectionView.SelectedItem as category;
            if (selectedCategory != null)
            {
                // Confirm deletion before proceeding
                bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the category '{selectedCategory.name}'?", "Yes", "No");
                if (confirm)
                {
                    try
                    {
                        // Delete the selected category
                        await _service.DeleteCategoryAsync(selectedCategory.categoryId);
                        await LoadCategories(); // Refresh the list of categories after deletion
                    }
                    catch (Exception ex)
                    {
                        // Display an error if category deletion fails
                        await DisplayAlert("Error", $"Failed to delete category: {ex.Message}", "OK");
                    }
                }
            }
            else
            {
                // If no category is selected, show a warning
                await DisplayAlert("Warning", "Please select a category to delete.", "OK");
            }
        }

        private async void OnRefreshCategoriesClicked(object sender, EventArgs e)
        {
            SearchEntry.Text = string.Empty; // Clear the search bar
            await LoadCategories(); // Refresh the list of categories
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue?.Trim().ToLower() ?? string.Empty;

            // Filter the categories based on the search text
            if (_allCategories != null)
            {
                var filteredCategories = _allCategories
                    .Where(c => c.name?.ToLower().Contains(searchText) ?? false)
                    .ToList();

                // Directly call BeginInvokeOnMainThread without await
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CategoriesCollectionView.ItemsSource = filteredCategories;
                });
            }
        }

    }
}
