
using GamingApp.ViewModels;
namespace GamingApp;

public partial class UsersPage : ContentPage
{
    private UserViewModel _viewModel;
	public UsersPage()
	{
		InitializeComponent();
        _viewModel = new UserViewModel();
        BindingContext = _viewModel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadUsersAsync();
        Console.WriteLine($"Users loaded: {_viewModel.Users.Count}");
    }
}