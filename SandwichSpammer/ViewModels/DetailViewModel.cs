// csharp
using CommunityToolkit.Mvvm.ComponentModel;
using SandwichSpammer.Database;
using SandwichSpammer.Models;
using Microsoft.Maui.Controls;

namespace SandwichSpammer.ViewModels;

public partial class DetailViewModel : ObservableObject
{
    private readonly DatabaseService _db;
    private readonly AccompanimentViewModel? _accompVm;
    private readonly BoutiqueViewModel? _boutiqueVm;

    [ObservableProperty]
    private int _upgrade1Count;
    
    [ObservableProperty]
    private int _upgrade1Cost;
    
    [ObservableProperty]
    private int _upgrade2Count;
    
    [ObservableProperty]
    private int _upgrade2Cost;
    
    [ObservableProperty]
    private int _counter;
    
    [ObservableProperty]
    private int _clickBonus;
    
    [ObservableProperty]
    private int _foundCount;

    [ObservableProperty]
    private string _foundAccompaniment = string.Empty;
    
    public DetailViewModel(DatabaseService db, AccompanimentViewModel? accompVm = null, BoutiqueViewModel? boutiqueVm = null)
    {
        _db = db;
        _accompVm = accompVm;
        _boutiqueVm = boutiqueVm;

        Refresh();
        
        // Si une nouvelle entrée est ajoutée en direct, recharger les valeurs depuis la DB
        MessagingCenter.Subscribe<AccompanimentViewModel, Accompaniment>(this, "AccompanimentChecked", (sender, accompaniment) =>
        {
            // on recharge depuis la DB pour être sûr d'afficher la liste complète et le compteur unique
            Refresh();
        });
    }

    public void Refresh()
    {
        Upgrade1Cost = _db.GetUpgrade1Cost();
        Upgrade1Count = _db.GetUpgrade1Count();
        Upgrade2Cost = _db.GetUpgrade2Cost();
        Upgrade2Count = _db.GetUpgrade2Count();
        Counter = _db.GetTotalClicks();
        ClickBonus = _db.GetClickBonus();

        // Charger les valeurs persistées des accompagnements trouvés (liste complète)
        FoundCount = _db.GetFoundAccompanimentCount();
        FoundAccompaniment = _db.GetLastFoundAccompaniment();
    }
}
