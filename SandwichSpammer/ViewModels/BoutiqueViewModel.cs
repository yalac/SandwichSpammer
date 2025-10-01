using System.Collections.ObjectModel;
using SandwichSpammer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SandwichSpammer.ViewModels;

public partial class BoutiqueViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Accompaniment> _ameliorations;
    
    [ObservableProperty]
    private ObservableCollection<Accompaniment> _malus;
    
    public BoutiqueViewModel()
    {
        Ameliorations = new ObservableCollection<Accompaniment>();
    }
    
    [RelayCommand]
    private void OpenMainPage()
    {
        Shell.Current.GoToAsync("MainPage", true);
    }
}