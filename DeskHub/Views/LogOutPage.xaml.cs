using DeskHub.Models;
using DeskHub.ViewModels;

namespace DeskHub.Views;

public partial class LogOutPage : ContentPage
{
    public LogOutPage() 
    { 
        InitializeComponent();
        DisplayLogoutConfirmation();
    }

    private async void DisplayLogoutConfirmation()
    {
        bool isLogout = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (isLogout)
        {
            var adminHome = new AppShell();
            Application.Current.MainPage = adminHome;
        }
        else
        {
            var adminHome = new AdminHome();
            adminHome.CurrentPage = adminHome.Children[4];
            Application.Current.MainPage = adminHome;
        }
    }



}
