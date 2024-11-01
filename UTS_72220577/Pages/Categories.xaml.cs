using UTS_72220577.Data;

namespace UTS_72220577.Pages;

public partial class Categories : ContentPage
{
    private readonly ccService _service;

    public Categories()
    {
        InitializeComponent();
        _service = new ccService(new HttpClient());
        LoadCategories();
    }

    private async void LoadCategories()
    {
        var categories = await _service.GetCategoriesAsync();
        CategoriesCollectionView.ItemsSource = categories;
    }

    private async void OnRefreshCategoriesClicked(object sender, EventArgs e)
    {
        LoadCategories();
    }
}