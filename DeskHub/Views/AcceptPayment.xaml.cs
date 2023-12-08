using System.Text.RegularExpressions;
using DeskHub.Models;
using DeskHub.ViewModels;
using DeskHub.Views;
namespace DeskHub.Views;

public partial class AcceptPayment : ContentPage
{
    RoomViewModel roomViewModel = new RoomViewModel();
    public AcceptPayment()
    {
        InitializeComponent();
        roomRateEntry.Text += PayInPerson.storedRoomRate;
        txtPayCode.Text += PayInPerson.storedPaycode;
        txtDuration.Text += PayInPerson.storedDuration;
        txtAmount.Text += PayInPerson.storedCalculation;
    }

    public AcceptPayment(User user)
    {
        BindingContext = user;
        InitializeComponent();
    }

    private async void OnbtnConfirm_Clicked(object sender, EventArgs e)
    {
        string enteredPaycode = txtPayCode.Text;

        if (string.IsNullOrWhiteSpace(enteredPaycode))
        {
            await DisplayAlert("Payment Error", "Paycode field is required. Please enter a valid paycode.", "OK");
            return;
        }

        if (enteredPaycode == PayInPerson.storedPaycode)
        {
            if (double.TryParse(txtAR.Text, out double enteredAmount))
            {
                if (double.TryParse(PayInPerson.storedCalculation, out double calculation))
                {
                    if (enteredAmount < calculation)
                    {
                        await DisplayAlert("Payment Error", "Entered amount is less than the room rate.", "OK");
                    }
                    else
                    {
                        double change = enteredAmount - calculation;
                        if (change < 0)
                        {
                            await DisplayAlert("Payment Error", "Entered amount is less than the room rate.", "OK");
                        }
                        else if (change == 0)
                        {
                            await DisplayAlert("Payment Successful", "Payment codes match. Payment confirmed.", "OK");
                            Room roomBooked = roomViewModel.GetRoomByID(PayInPerson.storedRoomID);

                            if (roomBooked != null)
                            {
                                roomBooked.IsAvailable = false;
                                roomViewModel.EditRoom(roomBooked);
                                await DisplayAlert("Payment Successful", "Room availability updated. Payment confirmed.", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Payment Successful", $"Payment codes match. Payment confirmed.\nYour Change is: {change:F2}", "OK");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Invalid room rate.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid entered amount.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Payment Error", "Payment codes do not match. Please enter a valid paycode.", "OK");
        }
    }


    private async void OnbtnCancel_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Payment Cancelled", "Going back to Home", "OK");
        var adminHome = new AdminHome();
        adminHome.CurrentPage = adminHome.Children[3];
        Application.Current.MainPage= adminHome;
    }

    private void OnEmailTextChanged(object sender, TextChangedEventArgs e)
    {
        string email = e.NewTextValue;
        bool isValid = IsEmailValid(email);
    }
    private bool IsEmailValid(string email)
    {
        string pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
        return Regex.IsMatch(email, pattern);
    }
}