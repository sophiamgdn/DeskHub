
using DeskHub.Views;
using DeskHub.ViewModels;
using DeskHub.Models;

namespace DeskHub.Views;

public partial class AdminHome : TabbedPage
{
    public AdminHome()
    {
        InitializeComponent();
    }

    public AdminHome(User user) 
    {
        InitializeComponent();
        BindingContext = user;
    }
   
}