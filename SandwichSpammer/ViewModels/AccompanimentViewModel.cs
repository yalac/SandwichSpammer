using System.Collections.ObjectModel;
using SandwichSpammer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SandwichSpammer.Database;

namespace SandwichSpammer.ViewModels;

public partial class AccompanimentViewModel : ObservableObject
{
    protected readonly DatabaseService _databaseService;
    
    [ObservableProperty]
    private ObservableCollection<Accompaniment> _accompaniments;
    
    [ObservableProperty]
    private string _newAccompanimentName;
    
    public AccompanimentViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        Accompaniments = new ObservableCollection<Accompaniment>();
    }
    
    [RelayCommand]
    public void AddAccompaniment()
    {
        Accompaniments.Add(new Accompaniment { Name = NewAccompanimentName});
        NewAccompanimentName = string.Empty;
    }
}