using DeskHub.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;

namespace DeskHub.ViewModels
{
    public class EmployeeViewModel
    {
        private ObservableCollection<Employee> employeeList = new ObservableCollection<Employee>();
        string maindir = AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "Employees.txt";

        public event PropertyChangedEventHandler PropertyChanged;

        public EmployeeViewModel()
        {
            employeeList = new ObservableCollection<Employee>();
            ConvertToEmployeeList();
        }

        public ObservableCollection<Employee> EmployeeList
        {
            get { return employeeList; }
            set { 
                if(employeeList != value)
                {
                    employeeList = value;
                    OnPropertyChanged(nameof(EmployeeList));
                }
            }
        }

        protected virtual void OnPropertyChanged(string EmployeeList)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(EmployeeList));
        }

        public void AddEmployee(Employee employee)
        {
            employeeList.Add(employee);
            SaveToFile();
        }

        public void RemoveEmployee(Employee employee)
        {
            employeeList.Remove(employee);
            SaveToFile();
        }

        public void EditEmployee(Employee updatedEmployee)
        {
            var existingRoom = employeeList.FirstOrDefault(e => e.ID == updatedEmployee.ID);

            if (existingRoom != null)
            {
                existingRoom.Email = updatedEmployee.Email;
                existingRoom.Username = updatedEmployee.Username;
                existingRoom.Password = updatedEmployee.Password;
                OnPropertyChanged(nameof(EmployeeList));
                SaveToFile();
            }
        }

        public void SaveToFile()
        {
            var json = string.Empty;
            json = JsonSerializer.Serialize(employeeList);
            File.WriteAllText(maindir + fileName, json);
        }

        public void ConvertToEmployeeList()
        {
            if (File.Exists(maindir + fileName))
            {
                string jsonData = File.ReadAllText(maindir + fileName);
                employeeList = JsonSerializer.Deserialize<ObservableCollection<Employee>>(jsonData);
            }
        }

        public Employee GetEmployeeByUsername(string username)
        {
            return employeeList.FirstOrDefault(emp => emp.Username == username);
        }

    }
}
