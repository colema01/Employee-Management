// Open new terminal and enter cd EmployeeManagementSystem
// Then enter dotnet run to start


using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics; // Required to open Notepad

class Employee
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public double Salary { get; set; }

    // Formats employee data into a string for saving
    public string ToFileString()
    {
        return $"{ID},{Name},{Department},{Position},{Salary}";
    }

    // Loads employee data from a string
    public static Employee FromFileString(string fileLine)
    {
        string[] data = fileLine.Split(',');
        return new Employee
        {
            ID = int.Parse(data[0]),
            Name = data[1],
            Department = data[2],
            Position = data[3],
            Salary = double.Parse(data[4])
        };
    }
}

class EmployeeManagementSystem
{
    static List<Employee> employees = new List<Employee>();

    // Use Environment.GetFolderPath to get the Documents folder path
    static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "employee_data.txt");

    static void Main(string[] args)
    {
        LoadEmployeesFromFile(); // Load employees from file when program starts

        int choice;

        do
        {
            Console.WriteLine("Employee Management System");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Edit Employee");
            Console.WriteLine("3. Display Employees");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
            choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddEmployee();
                    break;
                case 2:
                    EditEmployee();
                    break;
                case 3:
                    DisplayEmployees();
                    break;
                case 4:
                    DeleteEmployee();
                    break;
                case 5:
                    Console.WriteLine("Exiting...");
                    SaveEmployeesToFile(); // Save employees to file before exiting
                    OpenNotepad(); // Open saved file in Notepad
                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        } while (choice != 5);
    }

    static void AddEmployee()
    {
        Employee emp = new Employee();

        // Validate Employee ID input (must be an integer)
        while (true)
        {
            Console.Write("Enter Employee ID (must be a number): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int id))
            {
                emp.ID = id;
                break; // Exit the loop if a valid integer is entered
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number for Employee ID.");
            }
        }

        // Continue with other employee details
        Console.Write("Enter Employee Name: ");
        emp.Name = Console.ReadLine();
        Console.Write("Enter Employee Department: ");
        emp.Department = Console.ReadLine();
        Console.Write("Enter Employee Position: ");
        emp.Position = Console.ReadLine();

        // Validate salary input (must be a double)
        while (true)
        {
            Console.Write("Enter Employee Salary: ");
            string input = Console.ReadLine();

            if (double.TryParse(input, out double salary))
            {
                emp.Salary = salary;
                break; // Exit the loop if a valid salary is entered
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number for salary.");
            }
        }

        employees.Add(emp);
        Console.WriteLine("Employee added successfully.");
    }

    static void EditEmployee()
    {
        Console.Write("Enter Employee ID to edit: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Employee emp = employees.Find(e => e.ID == id);

        if (emp != null)
        {
            Console.Write("Enter new Name: ");
            emp.Name = Console.ReadLine();
            Console.Write("Enter new Department: ");
            emp.Department = Console.ReadLine();
            Console.Write("Enter new Position: ");
            emp.Position = Console.ReadLine();
            Console.Write("Enter new Salary: ");
            emp.Salary = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Employee updated successfully.");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    static void DisplayEmployees()
    {
        if (employees.Count > 0)
        {
            Console.WriteLine("Employee List:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"ID: {emp.ID}, Name: {emp.Name}, Department: {emp.Department}, Position: {emp.Position}, Salary: {emp.Salary}");
            }
        }
        else
        {
            Console.WriteLine("No employees found.");
        }
    }

    static void DeleteEmployee()
    {
        Console.Write("Enter Employee ID to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Employee emp = employees.Find(e => e.ID == id);

        if (emp != null)
        {
            employees.Remove(emp);
            Console.WriteLine("Employee deleted successfully.");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    // Save employee data to a text file in the Documents folder
    static void SaveEmployeesToFile()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var emp in employees)
                {
                    writer.WriteLine(emp.ToFileString());
                }
            }
            Console.WriteLine($"Employees saved to {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    // Method to load employee data from the text file
    static void LoadEmployeesFromFile()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    Employee emp = Employee.FromFileString(line);
                    employees.Add(emp);
                }
                Console.WriteLine("Employees loaded from file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("No saved employee data found.");
        }
    }

    // Method to open Notepad and display the saved employee data
    static void OpenNotepad()
    {
        Process.Start("notepad.exe", filePath);
    }
}
