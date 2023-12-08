using DeskHub.ViewModels;
using DeskHub.Models;
using System.Text.RegularExpressions;

namespace DeskHub.Views;

public partial class ModifyEmployee : ContentPage
{
    EmployeeViewModel employeeViewModel = new EmployeeViewModel();
    UserViewModel userViewModel = new UserViewModel();
    public ModifyEmployee()
    {
        InitializeComponent();
        BindingContext = employeeViewModel;
    }

    public void OnbtnAddEmployeeClicked(object sender, EventArgs e)
    {
        Employee employee = new Employee();
        User user = new User();
        employee.ID = txtID.Text;
        employee.Name = txtName.Text;
        employee.Email = txtEmail.Text;
        employee.Username = txtUsername.Text;
        employee.Password = txtPassword.Text;

        if (string.IsNullOrEmpty(employee.ID) || string.IsNullOrEmpty(employee.Name) || string.IsNullOrEmpty(employee.Email) || string.IsNullOrEmpty(employee.Username) || string.IsNullOrEmpty(employee.Password))
        {
            DisplayAlert("Error", "Please fill out all required fields.", "OK");
            return;
        }
        if (!IsEmailValid(employee.Email))
        {
            DisplayAlert("Error", "Invalid email address", "OK");
            return;
        }
        if (EmployeeExists(employee.ID, employee.Username))
        {
            DisplayAlert("Error", "An employee with the same ID or username already exists. Please enter a different ID or username.", "OK");
            return;
        }

        employeeViewModel.AddEmployee(employee);
        ClearForm();
    }

    public void ClearForm()
    {
        txtID.Text = string.Empty;
        txtName.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtUsername.Text = string.Empty;
        txtPassword.Text = string.Empty;
    }

    private async void OnEditEmployeeClicked(object sender, EventArgs e)
    {
        string inputID = await DisplayPromptAsync("Edit Employee Details", "Enter Employee ID to Edit:");

        if (string.IsNullOrWhiteSpace(inputID))
        {
            return;
        }

        Employee empToEdit = employeeViewModel.EmployeeList.FirstOrDefault(r => r.ID == inputID);

        if (empToEdit != null)
        {
            string[] editOptions = { "Email", "Username", "Password" };

            string selectedOption = await DisplayActionSheet("Select Property to Edit", "Cancel", null, editOptions);

            if (selectedOption == "Cancel")
            {
                return;
            }

            string newValue = null;

            if (selectedOption == "Email")
            {
                newValue = await DisplayPromptAsync("Edit Employee Email", "Enter new Email:", initialValue: empToEdit.Email);
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    empToEdit.Email = newValue;
                }
            }
            else if (selectedOption == "Username")
            {
                newValue = await DisplayPromptAsync("Edit Employee Username", "Enter new Username:", initialValue: empToEdit.Username);
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    empToEdit.Username = newValue;
                }
            }
            else if (selectedOption == "Password")
            {
                newValue = await DisplayPromptAsync("Edit Employee Password", "Enter new Password:", initialValue: empToEdit.Password);
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    empToEdit.Password = newValue;
                }
            }

            employeeViewModel.EditEmployee(empToEdit);
            await DisplayAlert("Edit Employee Details", "Employee detail edited successfully", "OK");
            UpdateViewModel();
        }
        else
        {
            await DisplayAlert("Edit Employee Details Error", "Employee not found", "OK");
        }
    }

    public void UpdateViewModel()
    {
        employeeViewModel = new EmployeeViewModel();
        BindingContext = employeeViewModel;
    }

    private bool EmployeeExists(string id, string username)
    {
        if (employeeViewModel.EmployeeList.Any(emp => emp.ID == id))
        {
            return true;
        }
        if (employeeViewModel.EmployeeList.Any(emp => emp.Username == username))
        {
            return true;
        }
        return false;
    }

    public async void OnbtnRemoveEmployeeClicked(object sender, EventArgs e)
    {
        string inputId = await DisplayPromptAsync("Delete Employee", "Enter Employee ID to delete:");

        if (!string.IsNullOrEmpty(inputId))
        {
            Employee employeeToRemove = employeeViewModel.EmployeeList.FirstOrDefault(emp => emp.ID == inputId);

            if (employeeToRemove != null)
            {
                employeeViewModel.RemoveEmployee(employeeToRemove);
            }
            else
            {
                await DisplayAlert("Employee Not Found", "No employee found with the specified ID.", "OK");
            }
        }
    }

    private bool IsEmailValid(string email)
    {
        string pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
        return Regex.IsMatch(email, pattern);
    }
}