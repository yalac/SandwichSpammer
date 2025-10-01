using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SandwichSpammer.Models;

namespace SandwichSpammer.ViewModels;

public partial class StudentViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Student> _students;
    
    [ObservableProperty]
    private string _newFirstName;
    
    [ObservableProperty]
    private string _newLastName;
    
    [ObservableProperty]
    private double _newGrade;
    
    [ObservableProperty]
    private double _newTempGrade;
    
    [ObservableProperty]
    private double _newAverageGrade;
    
    public StudentViewModel()
    {
        Students = new ObservableCollection<Student>
        {
            new Student { FirstName = "Ema", LastName = "Boye", Grade = 9 },
            new Student { FirstName = "Elody", LastName = "Demasto", Grade = 5 }
        };
        UpdateAverageGrade();
    }
    
    [RelayCommand]
    public void AddStudent()
    {
        if (!string.IsNullOrEmpty(NewFirstName) && !string.IsNullOrEmpty(NewLastName) && NewGrade >= 0)
        {
            Students.Add(new Student { FirstName = NewFirstName, LastName = NewLastName, Grade = NewGrade });

            NewFirstName = string.Empty;
            NewLastName = string.Empty;
            NewGrade = 0;
            UpdateAverageGrade();
        }
    }
    
    private void UpdateAverageGrade()
    {
        NewAverageGrade = Students.Any() ? Students.Average(s => s.Grade) : 0;
    }
}