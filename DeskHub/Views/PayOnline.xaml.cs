using DeskHub.ViewModels;
using DeskHub.Models;
using System.Text.RegularExpressions;

namespace DeskHub.Views;

public partial class PayOnline : ContentPage
{
    User user = new User();
    private bool buttonClicked = true;
    public PayOnline()
    {
        InitializeComponent();
    }

    public PayOnline(string roomID, string roomType, double roomRate, string paycode) : this()
    {
        roomIDEntry.Text = roomID;
        roomTypeEntry.Text = roomType;
        roomRateEntry.Text = roomRate.ToString();
        txtPayCode.Text = paycode;
    }

    private void OnCreditCardEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            string cleanedText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
            if (cleanedText.Length > 4)
            {
                cleanedText = cleanedText.Insert(4, "-");
            }
            if (cleanedText.Length > 9)
            {
                cleanedText = cleanedText.Insert(9, "-");
            }
            if (cleanedText.Length > 14)
            {
                cleanedText = cleanedText.Insert(14, "-");
            }
            if (cleanedText.Length > 19)
            {
                cleanedText = cleanedText.Substring(0, 19);
            }

            txtCreditCard.Text = cleanedText;

        }
    }
    private async void OnbtnProceedPayment(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtCreditCard.Text))
        {
            await DisplayAlert("Required Fields", "Please enter the required details for email and credit card.", "OK");
            return;
        }

        if (buttonClicked)
        {
            buttonClicked = true;
            string roomID = roomIDEntry.Text;
            string roomType = roomTypeEntry.Text;
            string roomRate = roomRateEntry.Text;
            string payCode = txtPayCode.Text;
            string email = txtEmail.Text;
            string paymentDetails = $"Room ID: {roomID}\nRoom Type: {roomType}\nRoom Rate: {roomRate}\nPayment Code: {payCode}";

            if (string.IsNullOrWhiteSpace(roomID) || string.IsNullOrWhiteSpace(roomType) || string.IsNullOrWhiteSpace(roomRate) || string.IsNullOrWhiteSpace(payCode) || string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Empty Payment Details", "Please enter the payment details properly.", "OK");
            }
            else
            {
                bool paymentConfirmed = await DisplayAlert("Confirm Payment", $"Viewing Payment Details:\n{paymentDetails}\nContact Details: {email}\n\nProceed with payment?", "Yes", "No");

                if (paymentConfirmed)
                {
                    await DisplayAlert("Payment Successful", "Payment successful!", "OK");
                    if (user.UserType == "Admin")
                    {
                        var adminHome = new AdminHome();
                        adminHome.CurrentPage = adminHome.Children[3];
                        Application.Current.MainPage = adminHome;
                    }
                    else
                    {
                        Application.Current.MainPage = new ClientHome();
                    }
                }
                else if (user.UserType == "Admin")
                {
                    var adminHome = new AdminHome();
                    adminHome.CurrentPage = adminHome.Children[3];
                    Application.Current.MainPage = adminHome;
                }
            }
        }
    }

    private async void OnBtnCancelPayment(object sender, EventArgs e)
    {
        await DisplayAlert("Payment Cancelled", "Going back to Home", "OK");
        if (user.UserType == "Admin")
        {
            var adminHome = new AdminHome();
            adminHome.CurrentPage = adminHome.Children[3];
            Application.Current.MainPage = adminHome;
        }
        else if (user.UserType == "Client")
        {
            Application.Current.MainPage = new ClientHome();
        }
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