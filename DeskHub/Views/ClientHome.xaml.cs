using DeskHub.Models;
using DeskHub.ViewModels;
using System.Text;

namespace DeskHub.Views;

public partial class ClientHome : ContentPage
{
    RoomViewModel roomViewModel = new RoomViewModel();

    public ClientHome()
	{
		InitializeComponent();
        this.Title = "DeskHub";
	}

    public ClientHome(User user)
    {
        InitializeComponent();
        BindingContext = user;
    }

    public void UpdateViewModel()
    {
        roomViewModel = new RoomViewModel();
        BindingContext = roomViewModel;
    }

    private async void DisplayAvailableRooms()
    {
        UpdateViewModel();
        TimeSpan timeIn = pkrTimeIn.Time;
        TimeSpan timeOut = pkrTimeOut.Time;

        if (timeIn > timeOut)
        {
            await DisplayAlert("Invalid Selection", "Time In cannot be later than Time Out.", "OK");
        }
        else
        {
            if (timeIn == TimeSpan.Zero || timeOut == TimeSpan.Zero)
            {
                await DisplayAlert("Invalid Selection", "Please select a time in both Time In and Time Out.", "OK");
            }
            else
            {
                var availableRooms = roomViewModel.RoomList.Where(room => room.IsAvailable).ToList();

                if (availableRooms.Count == 0)
                {
                    await DisplayAlert("No Available Rooms", "There are no available rooms.", "OK");
                    return;
                }

                listRooms.ItemsSource = availableRooms;
                listRooms.IsVisible = availableRooms.Any();
            }
        }
    }


    private void OnbtnCheckAvailability_Clicked(object sender, EventArgs e)
    {
        DisplayAvailableRooms();
    }

    private void OnbtnSelectRoom_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var selectedRoom = (Room)button.BindingContext;

        string paycode = GeneratePayCode();
        Application.Current.MainPage = new PayOnline(selectedRoom.RoomID, selectedRoom.RoomType, selectedRoom.Rate, paycode);
    }

    public string GeneratePayCode()
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        StringBuilder paycode = new StringBuilder();
        Random random = new Random();

        for (int i = 0; i < 16; i++)
        {
            int index = random.Next(characters.Length);
            paycode.Append(characters[index]);
        }
        return "PAY" + paycode.ToString();
    }

    private async void OnLogoutClient_Clicked(object sender, EventArgs e)
    {
        string[] logoutOptions = { "Yes, log out of this device", "No, do not log out" };

        string selectedOption = await DisplayActionSheet("Are you sure you want to log out?", null, null, logoutOptions);

        if (selectedOption == "Yes, log out of this device")
        {
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Log Out Cancelled", "You are still logged in.", "OK");
            Application.Current.MainPage = new ClientHome();
        }
    }
}