// csharp
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using SandwichSpammer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SandwichSpammer.Database;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace SandwichSpammer.ViewModels;

public partial class AccompanimentViewModel : ObservableObject
{
    protected readonly DatabaseService _databaseService;

    [ObservableProperty]
    private int _counter;
    
    [ObservableProperty]
    private int _clickBonus;

    [ObservableProperty]
    private ObservableCollection<Accompaniment> _accompaniments = new();

    [ObservableProperty]
    private string _entryText = string.Empty;

    [ObservableProperty]
    private string _entryStatus = string.Empty;

    public AccompanimentViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        Counter = _database_service_GetTotalClicksSafe();
        ClickBonus = _database_service_GetClickBonusSafe();

        // Charger accompagnements depuis la base
        var list = _databaseService.GetAccompaniments() ?? new List<Accompaniment>();
        Debug.WriteLine("Accompaniments en base:");
        foreach (var a in list) Debug.WriteLine($"- '{a.Name}' (norm='{NormalizeString(a.Name ?? string.Empty)}')");

        Accompaniments = new ObservableCollection<Accompaniment>(list);
    }
    
    private static string NormalizeString(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return string.Empty;

        // idem nettoyage ici pour garantir même comportement que la DB
        var cleaned = s.Replace('\u00A0', ' ');
        cleaned = new string(cleaned.Where(c => !char.IsControl(c)).ToArray());

        var normalized = cleaned.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var ch in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
    
    [RelayCommand]
    private void IncrementCounter()
    {
        Counter += ClickBonus;
        _databaseService.SaveTotalClicks(Counter);
    }

    public void SetClickBonus(int bonus)
    {
        ClickBonus += bonus;
        _database_service_SaveClickBonusSafe(ClickBonus);
    }

    [RelayCommand]
    private async Task ResetCalorieAndMainPage()
    {
        Counter = 0;
        ClickBonus = 1;
        _databaseService.SaveTotalClicks(Counter);
        _databaseService.SaveClickBonus(ClickBonus);
        _database_service_ResetUpgradesSafe();

        await Shell.Current.GoToAsync("//MainPage", true);
    }
    
    [RelayCommand]
    public async Task ResetAccompaniments()
    {
        _databaseService.ResetFoundAccompaniments();
        
        ClickBonus = _databaseService.GetClickBonus();
        Counter = _databaseService.GetTotalClicks();
        
        MessagingCenter.Send(this, "AccompanimentsReset");
        await Shell.Current.GoToAsync("//MainPage", true);
    }

    // Vérifie si EntryText correspond à un accompagnement existant (insensible à la casse)
    [RelayCommand]
    private void CheckEntry()
    {
        if (string.IsNullOrWhiteSpace(EntryText))
        {
            EntryStatus = string.Empty;
            return;
        }

        // Nettoyage léger avant normalisation (remplacer NBSP et retirer contrôles)
        var original = EntryText;
        var cleanedInput = original.Replace('\u00A0', ' ');
        cleanedInput = new string(cleanedInput.Where(c => !char.IsControl(c)).ToArray());
        var nameNorm = NormalizeString(cleanedInput);

        Debug.WriteLine($"[CheckEntry] input original: '{original}'");
        Debug.WriteLine($"[CheckEntry] input cleaned:  '{cleanedInput}'");
        Debug.WriteLine($"[CheckEntry] input normalized:'{nameNorm}'");

        Debug.WriteLine("[CheckEntry] accompaniments in memory:");
        foreach (var a in Accompaniments)
        {
            var n = a?.Name ?? string.Empty;
            Debug.WriteLine($"  - DB name: '{n}'  normalized: '{NormalizeString(n)}'");
        }

        var found = Accompaniments.FirstOrDefault(a => !string.IsNullOrWhiteSpace(a.Name) && NormalizeString(a.Name) == nameNorm);
        if (found == null)
        {
            EntryStatus = "Échec";
            Debug.WriteLine("[CheckEntry] result: NOT FOUND");
            return;
        }

        // Persister seulement si nouvel accompagnement
        var added = _databaseService.SaveFoundAccompaniment(found);
        EntryStatus = added ? "Succès" : "Déjà trouvé";

        Debug.WriteLine($"[CheckEntry] result: FOUND '{found.Name}'  added={added}");

        if (added)
        {
            // Mettre à jour le ClickBonus local depuis la DB (ajout de +100 effectué côté DB)
            ClickBonus = _databaseService.GetClickBonus();
            MessagingCenter.Send(this, "AccompanimentChecked", found);
        }
    }
    
    private int _database_service_GetTotalClicksSafe() => _databaseService.GetTotalClicks();
    private int _database_service_GetClickBonusSafe() => _databaseService.GetClickBonus();
    private void _database_service_SaveClickBonusSafe(int bonus) => _databaseService.SaveClickBonus(bonus);
    private void _database_service_ResetUpgradesSafe() => _databaseService.ResetUpgrades();
}
