using DeskHub.Models;
using DeskHub.ViewModels;
namespace DeskHub.Views;

public partial class ModifyRooms : ContentPage
{
    RoomViewModel roomViewModel = new RoomViewModel();
    public ModifyRooms()
    {
        InitializeComponent();
        BindingContext = roomViewModel;
        pkrType.SelectedIndex = 0;
        pkrBranch.SelectedIndex = 0;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        roomViewModel.ConvertToRoomList();
        UpdateViewModel();
    }

    private void OnpkrTypeChanged(object sender, EventArgs e)
    {
        int selectedIndex = pkrType.SelectedIndex;
        string newRate = string.Empty;

        switch (selectedIndex)
        {
            case 0:
                newRate = string.Empty;
                break;
            case 1:
                newRate = "150";
                break;
            case 2:
                newRate = "300";
                break;
            case 3:
                newRate = "3000";
                break;
            case 4:
                newRate = "900";
                break;
            case 5:
                newRate = "2250";
                break;
            default:
                break;
        }

        txtRoomRate.Text = newRate;
    }

    public void UpdateViewModel()
    {
        roomViewModel = new RoomViewModel();
        BindingContext = roomViewModel;
    }

    public void OnbtnAddRoomClicked(object sender, EventArgs e)
    {
        Room room = new Room();
        room.RoomID = txtRoomID.Text;
        room.RoomName = txtRoomName.Text;
        room.RoomType = pkrType.SelectedItem.ToString();


        if (pkrBranch.SelectedItem != null)
        {
            room.Branch = pkrBranch.SelectedItem.ToString();
        }

        if (string.IsNullOrEmpty(room.RoomID) || string.IsNullOrEmpty(room.RoomName) || pkrBranch.SelectedIndex == 0 || pkrType.SelectedIndex == 0)
        {
            DisplayAlert("Error", "Please fill out all required fields.", "OK");
            return;
        }
        if (RoomExists(room.RoomID))
        {
            DisplayAlert("Error", "A room with the same room type already exists. Please enter a different ID or username.", "OK");
            return;
        }

        if (pkrType.SelectedIndex == 1)
        {
            //Hot Desk
            room.RoomCapacity = 1;
            room.Rate = 150;
        }
        else if (pkrType.SelectedIndex == 2)
        {
            //Duo Hot Desk
            room.RoomCapacity = 2;
            room.Rate = 300;
        }
        else if (pkrType.SelectedIndex == 3)
        {
            //Meeting Conference
            room.RoomCapacity = 20;
            room.Rate = 3000;
        }
        else if (pkrType.SelectedIndex == 4)
        {
            //Small Private Room
            room.RoomCapacity = 6;
            room.Rate = 900;
        }
        else if (pkrType.SelectedIndex == 5)
        {
            //Large Private Room
            room.RoomCapacity = 15;
            room.Rate = 2250;
        }

        room.IsAvailable = true;

        roomViewModel.AddRoom(room);
        ClearForm();
        UpdateViewModel();
    }
    public void ClearForm()
    {
        txtRoomName.Text = string.Empty;
        txtRoomID.Text = string.Empty;
        pkrBranch.SelectedIndex = 0;
        pkrType.SelectedIndex = 0;
        txtRoomRate.Text = string.Empty;
    }

    private bool RoomExists(string roomID)
    {
        if (roomViewModel.RoomList.Any(emp => emp.RoomID == roomID))
        {
            return true;
        }
        return false;
    }

    private async void OnEditRoomTapped(object sender, EventArgs e)
    {
        string inputRoom = await DisplayPromptAsync("Edit Room", "Enter Room ID to Edit:");

        if (string.IsNullOrWhiteSpace(inputRoom))
        {
            return;
        }

        Room roomToEdit = roomViewModel.RoomList.FirstOrDefault(r => r.RoomID == inputRoom);

        if (roomToEdit != null)
        {
            string[] editOptions = { "Room Name", "Branch", "Availability" };

            string selectedOption = await DisplayActionSheet("Select Property to Edit", "Cancel", null, editOptions);

            if (selectedOption == "Cancel")
            {
                return;
            }

            string newValue = null;

            if (selectedOption == "Room Name")
            {
                newValue = await DisplayPromptAsync("Edit Room", "Enter new Room Name:", initialValue: roomToEdit.RoomName);
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    roomToEdit.RoomName = newValue;
                }
            }
            else if (selectedOption == "Branch")
            {
                newValue = await DisplayPromptAsync("Edit Room", "Enter new Branch:", initialValue: roomToEdit.Branch);
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    roomToEdit.Branch = newValue;
                }
            }
            else if (selectedOption == "Availability")
            {
                bool isAvailable = await DisplayAlert("Edit Room", "Is the room available?", "Yes", "No");
                roomToEdit.IsAvailable = isAvailable;
            }

            roomViewModel.EditRoom(roomToEdit);
            await DisplayAlert("Edit Room", "Room edited successfully", "OK");
            UpdateViewModel();
        }
        else
        {
            await DisplayAlert("Edit Room", "Room not found", "OK");
        }
    }

    
    private async void OnDeleteRoomTapped(object sender, EventArgs e)
    {
        string inputRoom = await DisplayPromptAsync("Delete Room", "Enter Room ID to delete:");

        if (!string.IsNullOrEmpty(inputRoom))
        {
            Room roomToRemove = roomViewModel.RoomList.FirstOrDefault(room => room.RoomID == inputRoom);

            if (roomToRemove != null)
            {
                roomViewModel.RemoveRoom(roomToRemove);
            }
            else
            {
                await DisplayAlert("Room Not Found", "No room found with the specified ID.", "OK");
            }
        }
    }

    
}