using DeskHub.Models;
using DeskHub.ViewModels;
namespace DeskHub.Views;

public partial class SignInPage : ContentPage
{
    UserViewModel userViewModel = new UserViewModel();
    EmployeeViewModel employeeViewModel = new EmployeeViewModel();
    
    public SignInPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

    private void OnbtnSignUpClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("///SignUpPage");
    }

    private async void LoginClicked(object sender, EventArgs e)
    {
        string username = txtusernameEntry.Text;
        string password = txtpasswordEntry.Text;

        UserViewModel userViewModel = new UserViewModel();
        EmployeeViewModel employeeViewModel = new EmployeeViewModel();

        User user = userViewModel.GetUserByUsername(username);
        Employee employee = employeeViewModel.GetEmployeeByUsername(username);

        if (user == null && employee == null)
        {
            await DisplayAlert("Login Failed", "User does not exist.", "OK");
        }
        else if (user != null && user.Password == password)
        {
            if (user.UserType == "Admin")
            {
                await DisplayAlert("Login Successful", "You have successfully logged in.", "OK");
                var adminHome = new AdminHome(user);
                adminHome.CurrentPage = adminHome.Children[4];
                Application.Current.MainPage = adminHome;
            }
            else
            {
                await DisplayAlert("Login Successful", "You have successfully logged in.", "OK");
                var clientHome = new ClientHome(user);
                Application.Current.MainPage = clientHome;

                ClearEntryFields();
            }
        }
        else if (employee != null && employee.Password == password)
        {
            Application.Current.MainPage = new EmployeeHome();
            ClearEntryFields();
        }
        else
        {
            await DisplayAlert("Login Failed", "Invalid username or password.", "OK");
        }
    }

    private void ClearEntryFields()
    {
        txtusernameEntry.Text = "";
        txtpasswordEntry.Text = "";
    }

    private string FilterNonAlphaNumeric(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "[^a-zA-Z0-9]", "");
    }

    private void OnUsernameEntryChanged(object sender, TextChangedEventArgs e)
    {
        string username = e.NewTextValue;
        string numericText = FilterNonAlphaNumeric(username);
        txtusernameEntry.Text = numericText;
    }

    private void OnPasswordEntryChanged(object sender, TextChangedEventArgs e)
    {
        string password = e.NewTextValue;
        string numericText = FilterNonAlphaNumeric(password);
        txtpasswordEntry.Text = numericText;
    }
}