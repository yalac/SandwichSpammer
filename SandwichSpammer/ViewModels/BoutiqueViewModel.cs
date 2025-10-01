using System.Collections.ObjectModel;
using SandwichSpammer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SandwichSpammer.ViewModels;

public partial class BoutiqueViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Amelioration> _ameliorations;
    
    [ObservableProperty]
    private ObservableCollection<Amelioration> _malus;
    
    public BoutiqueViewModel()
    {
        Ameliorations = new ObservableCollection<Amelioration>();
    }
    
    [RelayCommand]
    private void OpenMainPage()
    {
        Shell.Current.GoToAsync("MainPage", true);
    }
}