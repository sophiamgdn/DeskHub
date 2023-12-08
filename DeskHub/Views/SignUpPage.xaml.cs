using DeskHub.Models;
using DeskHub.ViewModels;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace DeskHub.Views;
public partial class SignUpPage : ContentPage, INotifyPropertyChanged
{
    UserViewModel userViewModel = new UserViewModel();
   
    public SignUpPage()
    {
        InitializeComponent();
        BindingContext = this;
        GenerateUserID();
    }

    private void OnLabelTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("///SignInPage");
    }

    private void OnSignUpClicked(object sender, EventArgs e)
    {
       
        User user = new User();
        user.FirstName =txtFirstName.Text;
        user.MiddleName =txtMiddleName.Text;
        user.LastName =txtLastName.Text;
        user.Email = txtemailEntry.Text;
        user.Phone = txtPhone.Text;

        if (rdoFemale.IsChecked)
        {
            user.Gender = "Female";
        }else if (rdoMale.IsChecked)
        {
            user.Gender = "Male";
        }
        else
        {
            DisplayAlert("Invalid Submission", "Kindly choose an option.", "OK");
        }

        user.Address = txtAddress.Text;
        user.Birthday = DateOnly.FromDateTime(pkrBirthday.Date); ;
        user.Username = txtusernameEntry.Text;
        user.Password = txtpasswordEntry.Text;

        string userType = GetSelectedUserType();
        user.UserType = userType;
        user.UserID = GenerateUserID();

        if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.MiddleName) || string.IsNullOrEmpty(user.LastName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
        {
            DisplayAlert("Error", "Please fill out all required fields.", "OK");
            return;
        }
        if (!IsEmailValid(user.Email))
        {
            DisplayAlert("Error", "Invalid email address", "OK");
            return;
        }
        userViewModel.AddUser(user);
        ClearForm();

        //store the user details
        UserDataService.AddUser(user);

        string message = $"Name: {user.LastName}, {user.FirstName} {user.MiddleName}\nEmail: {user.Email}\nUsername: {user.Username}\nUser Type: {user.UserType}";
        DisplayAlert("Registration Successful", message, "OK");
    }

    public string GenerateUserID()
    {
        Random random = new Random();
        int randomNumber = random.Next(10000, 99999);
        return $"2023-{randomNumber}";
    }

    public void ClearForm()
    {
        txtFirstName.Text = null;
        txtMiddleName.Text = null;
        txtLastName.Text = null;
        txtemailEntry.Text = null; 
        txtPhone.Text = null;
        rdoFemale.IsChecked = false;
        rdoMale.IsChecked = false;
        txtAddress.Text = null;
        pkrBirthday.Date = DateTime.Now;
        txtusernameEntry.Text = null;
        txtpasswordEntry.Text = null;
        rdoAdminRadioButton.IsChecked = false;
        rdoClientRadioButton.IsChecked = false;
    }

    private string GetSelectedUserType()
    {
        if (rdoAdminRadioButton.IsChecked)
        {
            return "Admin";
        }
        else if (rdoClientRadioButton.IsChecked)
        {
            return "Client";
        }
        else
        {
            return "Not specified";
        }
    }


    private void OnFirstNameChanged(object sender, TextChangedEventArgs e)
    {
        string text = e.NewTextValue;
        string numericText = AcceptSpaceBar(text);
        txtFirstName.Text = numericText;
    }

    private void OnMiddleNameChanged(object sender, TextChangedEventArgs e)
    {
        string text = e.NewTextValue;
        string numericText = AcceptSpaceBar(text);
        txtMiddleName.Text = numericText;
    }

    private void OnLastNameChanged(object sender, TextChangedEventArgs e)
    {
        string text = e.NewTextValue;
        string numericText = AcceptSpaceBar(text);
        txtLastName.Text = numericText;
    }

    public string AcceptSpaceBar(string input)
    {
        if (input != null)
        {
            string pattern = "[^a-zA-Z0-9 ]";
            return Regex.Replace(input, pattern, "");
        }
        return string.Empty; // Return empty string if input is null
    }
    private string FilterNonAlphaNumeric(string input)
    {
        if (input != null)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "[^a-zA-Z0-9]", "");
        }
        return string.Empty; // Return empty string if input is null
    }

    private void OnEmailTextChanged(object sender, TextChangedEventArgs e)
    {
        string email = e.NewTextValue;
        bool isValid = IsEmailValid(email);
        if (!isValid)
        {
            txtemailEntry.TextColor = Colors.Red;
        }
        else
        {
            txtemailEntry.TextColor = Colors.Black;
        }
    }
    private bool IsEmailValid(string email)
    {
        if (email != null)
        {
            string pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            return Regex.IsMatch(email, pattern);
        }
        return false; // Return false if email is null
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