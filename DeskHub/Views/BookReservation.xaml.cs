using DeskHub.Models;
using DeskHub.ViewModels;
using System.Text;

namespace DeskHub.Views;

public partial class BookReservation : ContentPage
{
    RoomViewModel roomViewModel = new RoomViewModel();

    public BookReservation()
    {
        InitializeComponent();
        pkrTimeIn.FadeTo(1, 4000);
        pkrTimeOut.FadeTo(1, 4000);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        roomViewModel.ConvertToRoomList();
        UpdateViewModel();
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

    public string CalculateTotalTime(TimeSpan timeIn, TimeSpan timeOut)
    {
        if (timeIn > timeOut)
        {
            return "Time In cannot be later than Time Out.";
        }
        else if (timeIn == TimeSpan.Zero || timeOut == TimeSpan.Zero)
        {
            return "Please select a time in both Time In and Time Out.";
        }
        else
        {
            TimeSpan totalTime = timeOut - timeIn;
            return totalTime.ToString(@"hh\:mm");
        }
    }

  
    private async void OnbtnSelectRoom_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var selectedRoom = (Room)button.BindingContext;

        TimeSpan timeIn = pkrTimeIn.Time;
        TimeSpan timeOut = pkrTimeOut.Time;
        
        string selectPaymentMethod = await DisplayActionSheet("Select Payment Method", "Cancel", null, "Pay in Person", "Pay Online");
        if (selectPaymentMethod == "Pay in Person")
        {
            string paycode = GeneratePayCode();
            string result = CalculateTotalTime(timeIn, timeOut);

            var adminHome = Application.Current.MainPage.Navigation.NavigationStack.OfType<AdminHome>().LastOrDefault();

            if (adminHome != null)
            {
                adminHome.CurrentPage = new PayInPerson(selectedRoom.RoomID, selectedRoom.RoomType, selectedRoom.Rate, paycode, result);
            }
            else
            {
                User user = UserDataService.Users.FirstOrDefault();
                adminHome = new AdminHome(user); 
                adminHome.CurrentPage = new PayInPerson(selectedRoom.RoomID, selectedRoom.RoomType, selectedRoom.Rate, paycode, result);
                Application.Current.MainPage = adminHome;
            }

        }
        else if (selectPaymentMethod == "Pay Online")
        {
            string paycode = GeneratePayCode();

            var adminHome = Application.Current.MainPage.Navigation.NavigationStack.OfType<AdminHome>().LastOrDefault();

            if (adminHome != null)
            {
                User user = UserDataService.Users.FirstOrDefault();
                adminHome.CurrentPage = new PayOnline(selectedRoom.RoomID, selectedRoom.RoomType, selectedRoom.Rate, paycode);
            }
            else
            {
                User user = UserDataService.Users.FirstOrDefault();
                adminHome = new AdminHome(user); 
                adminHome.CurrentPage = new PayOnline(selectedRoom.RoomID, selectedRoom.RoomType, selectedRoom.Rate, paycode);
                Application.Current.MainPage = adminHome;
            }
        }
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
}