// csharp

using System.Diagnostics;
using System.Globalization;
using System.Text;
using SandwichSpammer.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace SandwichSpammer.Database;

public partial class DatabaseService
{
    private SQLiteConnection _database;
    public const string DatabaseFilename = "SpammerDB.db3";
    public const SQLiteOpenFlags Flags =
        SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.Create |
        SQLiteOpenFlags.SharedCache;
    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    public const int DefaultUpgrade1Cost = 25;
    public const int DefaultUpgrade2Cost = 500;
    
    public event Action? UpgradesReset;

    public DatabaseService()
    {
        _database = new SQLiteConnection(DatabasePath, Flags);
        _database.CreateTable<Accompaniment>();
        _database.CreateTable<GameState>();

        var state = _database.Table<GameState>().FirstOrDefault();
        if (state == null)
        {
            _database.Insert(new GameState
            {
                TotalClicks = 0,
                ClickBonus = 1,
                InitialCost = 0,
                NbAmelioration = 0,
                Upgrade1Cost = DefaultUpgrade1Cost,
                Upgrade1Count = 0,
                Upgrade2Cost = DefaultUpgrade2Cost,
                Upgrade2Count = 0,
                FoundAccompanimentCount = 0,
                LastFoundAccompaniment = string.Empty
            });
        }

        // Seed des accompagnements si nécessaire
        if (!_database.Table<Accompaniment>().Any())
        {
            AddBeurre();
            AddCamembert();
            AddJambon();
            AddKetchup();
            AddPiseCheval();
        }
    }
    
    private static string NormalizeString(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return string.Empty;

        // remplacer espaces insécables par espace normal et retirer caractères de contrôle
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

    public int GetTotalClicks()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        return state?.TotalClicks ?? 0;
    }

    public void SaveTotalClicks(int newTotal)
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        if (state != null)
        {
            state.TotalClicks = newTotal;
            _database.Update(state);
        }
        else
        {
            _database.Insert(new GameState { TotalClicks = newTotal, ClickBonus = 1 });
        }
    }

    public int GetClickBonus()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        return state?.ClickBonus ?? 1;
    }

    public void SaveClickBonus(int bonus)
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        if (state != null)
        {
            state.ClickBonus = bonus;
            _database.Update(state);
        }
        else
        {
            _database.Insert(new GameState { TotalClicks = 0, ClickBonus = bonus });
        }
    }
    
    public int GetUpgrade1Cost()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        if (state == null) return DefaultUpgrade1Cost;
        return state.Upgrade1Cost <= 0 ? DefaultUpgrade1Cost : state.Upgrade1Cost;
    }

    public void SaveUpgrade1Cost(int cost)
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        if (state != null)
        {
            state.Upgrade1Cost = cost;
            _database.Update(state);
        }
        else
        {
            _database.Insert(new GameState
            {
                Upgrade1Cost = cost,
                Upgrade1Count = 0,
                Upgrade2Cost = DefaultUpgrade2Cost,
                Upgrade2Count = 0,
                TotalClicks = 0,
                ClickBonus = 1
            });
        }
    }

    public int GetUpgrade1Count()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        return state?.Upgrade1Count ?? 0;
    }

    public void SaveUpgrade1Count(int count)
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        if (state != null)
        {
            state.Upgrade1Count = count;
            _database.Update(state);
        }
    }

    public int GetUpgrade2Cost()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        if (state == null) return DefaultUpgrade2Cost;
        return state.Upgrade2Cost <= 0 ? DefaultUpgrade2Cost : state.Upgrade2Cost;
    }

    public void SaveUpgrade2Cost(int cost)
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        if (state != null)
        {
            state.Upgrade2Cost = cost;
            _database.Update(state);
        }
        else
        {
            _database.Insert(new GameState
            {
                Upgrade2Cost = cost,
                Upgrade2Count = 0,
                Upgrade1Cost = DefaultUpgrade1Cost,
                Upgrade1Count = 0,
                TotalClicks = 0,
                ClickBonus = 1
            });
        }
    }

    public int GetUpgrade2Count()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        return state?.Upgrade2Count ?? 0;
    }

    public void SaveUpgrade2Count(int count)
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        if (state != null)
        {
            state.Upgrade2Count = count;
            _database.Update(state);
        }
    }
    
    public void ResetUpgrades()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        if (state != null)
        {
            state.Upgrade1Cost = DefaultUpgrade1Cost;
            state.Upgrade1Count = 0;
            state.Upgrade2Cost = DefaultUpgrade2Cost;
            state.Upgrade2Count = 0;
            _database.Update(state);
        }
        else
        {
            _database.Insert(new GameState
            {
                TotalClicks = 0,
                ClickBonus = 1,
                Upgrade1Cost = DefaultUpgrade1Cost,
                Upgrade1Count = 0,
                Upgrade2Cost = DefaultUpgrade2Cost,
                Upgrade2Count = 0
            });
        }

        UpgradesReset?.Invoke();
    }
    
    public bool SaveFoundAccompaniment(Accompaniment? accompaniment)
    {
        if (accompaniment == null) return false;

        var state = _database.Table<GameState>().FirstOrDefault();
        if (state == null)
        {
            var list = new List<string> { accompaniment.Name ?? string.Empty };
            var joined = string.Join(", ", list);
            _database.Insert(new GameState
            {
                TotalClicks = 0,
                ClickBonus = 1 + 100, // +100 cal/click pour le premier trouvé
                Upgrade1Cost = DefaultUpgrade1Cost,
                Upgrade1Count = 0,
                Upgrade2Cost = DefaultUpgrade2Cost,
                Upgrade2Count = 0,
                FoundAccompanimentCount = 1,
                LastFoundAccompaniment = joined
            });
            return true;
        }

        // Construire liste existante
        var existing = new List<string>();
        if (!string.IsNullOrWhiteSpace(state.LastFoundAccompaniment))
        {
            existing = state.LastFoundAccompaniment
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();
        }

        var newNorm = NormalizeString(accompaniment.Name ?? string.Empty);
        var already = existing.Any(e => NormalizeString(e) == newNorm);
        if (already) return false;

        existing.Add(accompaniment.Name ?? string.Empty);
        state.FoundAccompanimentCount = existing.Count;
        state.LastFoundAccompaniment = string.Join(", ", existing);

        // Augmenter le bonus par click de 100 pour chaque nouvel accompagnement trouvé
        state.ClickBonus = (state.ClickBonus <= 0 ? 1 : state.ClickBonus) + 100;

        _database.Update(state);
        return true;
    }

    public int GetFoundAccompanimentCount()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        return state?.FoundAccompanimentCount ?? 0;
    }

    public string GetLastFoundAccompaniment()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        return state?.LastFoundAccompaniment ?? string.Empty;
    }

    public int AddBeurre()
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Beurre",
            Description = "Beurre complet salé, bon pour la santé !!!",
            Calorie = 0.1,
            Image = "Resources/Images/Accompaniment/beurre.png"
        };

        return _database.Insert(newAccompaniment);
    }

    public int AddCamembert()
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Camembert",
            Description = "Aussi puant que bon !!!",
            Calorie = 1,
            Image = "Resources/Images/Accompaniment/camembert.png"
        };

        return _database.Insert(newAccompaniment);
    }

    public int AddJambon()
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Jambon",
            Description = "Une bonne tranche bien fraiche !!!",
            Calorie = 47,
            Image = "Resources/Images/Accompaniment/jambon.png"
        };

        return _database.Insert(newAccompaniment);
    }
    
    public int AddKetchup()
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Ketchup",
            //Description = "Une bonne tranche bien fraiche !!!",
            //Calorie = 47,
            // Image = "Resources/Images/Accompaniment/ketchup.png"
        };

        return _database.Insert(newAccompaniment);
    }
    
    public int AddPiseCheval()
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Pise de cheval",
            //  = "Une bonne tranche bien fraiche !!!",
            // Calorie = 47,
            // Image = "Resources/Images/Accompaniment/jambon.png"
        };

        return _database.Insert(newAccompaniment);
    }

    public List<Accompaniment> GetAccompaniments()
    {
        var list = _database.GetAllWithChildren<Accompaniment>(recursive: true)
            .ToList();

        // Nettoyer et persister noms bizarres (espaces insécables, trailing spaces, etc.)
        foreach (var a in list)
        {
            if (a?.Name == null) continue;
            var original = a.Name;
            var cleaned = original.Replace('\u00A0', ' ').Trim();
            if (cleaned != original)
            {
                a.Name = cleaned;
                try { _database.Update(a); }
                catch { /* silencieux si update échoue */ }
            }

            Debug.WriteLine($"Accomp loaded: '{a.Name}' -> normalized '{NormalizeString(a.Name)}'");
        }

        return list.OrderBy(x => x.Name).ToList();
    }
    
    public void ResetFoundAccompaniments()
    {
        var state = _database.Table<GameState>().FirstOrDefault();
        const int accompanimentBonus = 100;

        if (state != null)
        {
            // soustraire la contribution des accompagnements trouvés sans toucher aux améliorations
            var reduction = state.FoundAccompanimentCount * accompanimentBonus;
            state.ClickBonus = Math.Max(1, state.ClickBonus - reduction);

            state.FoundAccompanimentCount = 0;
            state.LastFoundAccompaniment = string.Empty;

            _database.Update(state);
        }
        else
        {
            // créer un état par défaut si nécessaire
            _database.Insert(new GameState
            {
                TotalClicks = 0,
                ClickBonus = 1,
                Upgrade1Cost = DefaultUpgrade1Cost,
                Upgrade1Count = 0,
                Upgrade2Cost = DefaultUpgrade2Cost,
                Upgrade2Count = 0,
                FoundAccompanimentCount = 0,
                LastFoundAccompaniment = string.Empty
            });
        }
    }
}
