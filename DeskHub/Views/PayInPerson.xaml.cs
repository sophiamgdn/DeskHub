using DeskHub.ViewModels;
using DeskHub.Models;
using System.Text.RegularExpressions;

namespace DeskHub.Views;

public partial class PayInPerson : ContentPage
{
    RoomViewModel roomViewModel = new RoomViewModel();
    Room room = new Room();
    public static string storedPaycode;
    public static string storedRoomRate;
    public static string storedDuration;
    public static string storedCalculation;
    public static string storedRoomID;
    public PayInPerson()
	{
		InitializeComponent();
	}
    
    public PayInPerson(string roomID, string roomType, double roomRate, string paycode, string result) : this()
    {
        roomIDEntry.Text = roomID;
        storedRoomID = roomIDEntry.Text;
        roomTypeEntry.Text = roomType;
        roomRateEntry.Text = roomRate.ToString();
        storedRoomRate = roomRate.ToString();
        storedPaycode = paycode;
        txtPayCode.Text = paycode;
        storedDuration = result;

        double rate = roomRate;
        TimeSpan durationTimeSpan;
        if (TimeSpan.TryParse(result, out durationTimeSpan))
        {
            int durationInMinutes = (int)durationTimeSpan.TotalMinutes;
            storedCalculation = (roomRate * durationInMinutes/ 60).ToString();
        }
        else
        {
            storedCalculation = "Invalid duration format";
        }
    }

    User user = new User();

    private async void OnbtnProceedPayment(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            await DisplayAlert("Required Fields", "Please enter the required details for email in order to contact client", "OK");
            return;
        }

        var adminHome = new AdminHome(user);
        adminHome.CurrentPage = adminHome.Children[5]; 
        Application.Current.MainPage = adminHome;

        var acceptPayment = new AcceptPayment(user); 
        await Application.Current.MainPage.Navigation.PushAsync(acceptPayment);
    }

    private async void OnBtnCancel(object sender, EventArgs e)
    {
        await DisplayAlert("Payment Cancelled", "Going back to Home", "OK");
        var adminHome = new AdminHome();
        adminHome.CurrentPage = adminHome.Children[4];
        Application.Current.MainPage = adminHome;

    }

    private void OnEmailTextChanged(object sender, TextChangedEventArgs e)
    {
        string email = e.NewTextValue;
        bool isValid = IsEmailValid(email);
        if (!isValid)
        {
            txtEmail.TextColor = Colors.Red;
        }
        else
        {
            txtEmail.TextColor = Colors.Black;
        }
    }

    private bool IsEmailValid(string email)
    {
        string pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
        return Regex.IsMatch(email, pattern);
    }
}