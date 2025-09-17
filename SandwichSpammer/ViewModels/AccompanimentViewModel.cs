using System.Collections.ObjectModel;
using SandwichSpammer.Models;

namespace SandwichSpammer.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class AccompanimentViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Accompaniment> _accompaniments;
    
    [ObservableProperty]
    private string _newAccompanimentName;
    
    public AccompanimentViewModel()
    {
        Accompaniments = new ObservableCollection<Accompaniment>();
    }
    
    [RelayCommand]
    public void AddAccompaniment()
    {
        Accompaniments.Add(new Accompaniment { Name = NewAccompanimentName});
        NewAccompanimentName = string.Empty;
    }
}