using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using DeskHub.Models;


namespace DeskHub.ViewModels
{
    public class UserViewModel
    {
        private ObservableCollection<User> userList = new ObservableCollection<User>();
        string maindir = AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "Users.txt";

        public User CurrentUser { get; set; }   

        public UserViewModel() {
            ConvertToUserList();
        }

        public ObservableCollection<User> UserList
        {
            get { return userList; }
            set { userList = value; }
        }

        public void AddUser (User user)
        {
            userList.Add(user);
            SaveToFile();
        }

        public void SaveToFile()
        {
            var json = string.Empty;
            json = JsonSerializer.Serialize(userList);
            File.WriteAllText(maindir + fileName, json);
        }

        public void ConvertToUserList()
        {
            if(File.Exists(maindir + fileName))
            {
                string jsonData = File.ReadAllText(maindir + fileName);
                userList = JsonSerializer.Deserialize<ObservableCollection<User>>(jsonData);
            }
        }

        public User GetUserByUsername(string username)
        {
            return userList.FirstOrDefault(user => user.Username == username);
        }

    }
}
