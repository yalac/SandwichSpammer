// csharp
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SandwichSpammer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SandwichSpammer.Database;
using Microsoft.Maui.Controls;

namespace SandwichSpammer.ViewModels;

public partial class BoutiqueViewModel : ObservableObject
{
    private readonly AccompanimentViewModel _accompanimentViewModel;
    private readonly DatabaseService _databaseService;

  
    private const int UpgradeBonus = 2;
    private const int Upgrade2Bonus = 10;
    
    private const double Upgrade1Multiplier = 1.15;
    private const double Upgrade2Multiplier = 1.20;
    
    [ObservableProperty]
    private int _upgrade1Count;
    
    [ObservableProperty]
    private int _upgrade1Cost;
    
    [ObservableProperty]
    private int _upgrade2Count;

    [ObservableProperty]
    private int _upgrade2Cost;
    
    [ObservableProperty]
    private bool _cheerleadersVisible;
    
    public int Counter => _accompanimentViewModel.Counter;

    public BoutiqueViewModel(AccompanimentViewModel accompagnementViewModel, DatabaseService databaseService)
    {
        _accompanimentViewModel = accompagnementViewModel;
        _databaseService = databaseService;

        Upgrade1Cost = _databaseService.GetUpgrade1Cost();
        Upgrade1Count = _databaseService.GetUpgrade1Count();
        Upgrade2Cost = _databaseService.GetUpgrade2Cost();
        Upgrade2Count = _databaseService.GetUpgrade2Count();

        // assurer cohérence initiale
        CheerleadersVisible = Upgrade2Count > 0;
        
        // Écoute les changements de Counter dans AccompanimentViewModel
        _accompagnementViewModel_PropertyChangedSetup();
        // Écoute les resets d'améliorations pour recharger l'UI
        _databaseService.UpgradesReset += OnUpgradesReset;
    }
    
    private void OnUpgradesReset()
    {
        Upgrade1Cost = _databaseService.GetUpgrade1Cost();
        Upgrade1Count = _databaseService.GetUpgrade1Count();
        Upgrade2Cost = _databaseService.GetUpgrade2Cost();
        Upgrade2Count = _databaseService.GetUpgrade2Count();
        CheerleadersVisible = Upgrade2Count > 0;
        OnPropertyChanged(nameof(Counter));
        OnPropertyChanged(nameof(CheerleadersVisible));
    }

    private void _accompagnementViewModel_PropertyChangedSetup()
    {
        _accompanimentViewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(_accompanimentViewModel.Counter))
            {
                OnPropertyChanged(nameof(Counter));
            }
        };
    }
    
    
    [RelayCommand]
    private async Task GainParClick()
    {
        int currentClicks = _databaseService.GetTotalClicks();
        int price = Upgrade1Cost;

        if (currentClicks >= price)
        {
            int newTotal = currentClicks - price;
            _databaseService.SaveTotalClicks(newTotal);
            _accompanimentViewModel.Counter = newTotal;

            _accompanimentViewModel.SetClickBonus(UpgradeBonus);

            Upgrade1Count++;
            _databaseService.SaveUpgrade1Count(Upgrade1Count);

            int newCost = (int)Math.Round(Upgrade1Cost * Upgrade1Multiplier);
            if (newCost < 1) newCost = 1;
            Upgrade1Cost = newCost;
            _databaseService.SaveUpgrade1Cost(Upgrade1Cost);

            OnPropertyChanged(nameof(Upgrade1Cost));
            OnPropertyChanged(nameof(Upgrade1Count));
            OnPropertyChanged(nameof(Counter));
        }
        else
        {
            await Shell.Current.DisplayAlert("Erreur", "Vous n'avez pas assez de calories pour acheter cette amélioration", "OK");
        }
    }

    [RelayCommand]
    private async Task BuyCheerleaders()
    {
        int currentClicks = _databaseService.GetTotalClicks();
        int price = Upgrade2Cost;

        if (currentClicks >= price)
        {
            int newTotal = currentClicks - price;
            _databaseService.SaveTotalClicks(newTotal);
            _accompanimentViewModel.Counter = newTotal;

            _accompanimentViewModel.SetClickBonus(Upgrade2Bonus);

            Upgrade2Count++;
            _databaseService.SaveUpgrade2Count(Upgrade2Count);
            CheerleadersVisible = Upgrade2Count > 0;
            
            int newCost = (int)Math.Round(Upgrade2Cost * Upgrade2Multiplier);
            if (newCost < 1) newCost = 1;
            Upgrade2Cost = newCost;
            _databaseService.SaveUpgrade2Cost(Upgrade2Cost);

            OnPropertyChanged(nameof(Upgrade2Cost));
            OnPropertyChanged(nameof(Upgrade2Count));
            OnPropertyChanged(nameof(CheerleadersVisible));
            OnPropertyChanged(nameof(Counter));
        }
        else
        {
            await Shell.Current.DisplayAlert("Erreur", "Vous n'avez pas assez de calories pour acheter cette amélioration", "OK");
        }
    }
}
