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
    
    [ObservableProperty]
    private int _counter;
    
    public AccompanimentViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        Accompaniments = new ObservableCollection<Accompaniment>();
        
        Counter = _databaseService.GetTotalClicks();
    }
    
    [RelayCommand]
    public void AddAccompaniment()
    {
        Accompaniments.Add(new Accompaniment { Name = NewAccompanimentName});
        NewAccompanimentName = string.Empty;
    }
    
    [RelayCommand]
    private void IncrementCounter()
    {
        Counter++;
        _databaseService.SaveTotalClicks(Counter);
    }
    
    [RelayCommand]
    private async Task ResetCalorieAndMainPage()
    {
        Counter = 0;
        _databaseService.SaveTotalClicks(Counter);
        await Shell.Current.GoToAsync("//MainPage", true);
    }
}