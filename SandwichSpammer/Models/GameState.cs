using SQLite;

namespace SandwichSpammer.Models;

public class GameState
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int TotalClicks { get; set; }
    public int ClickBonus { get; set; }
    public int InitialCost { get; set; }
    public int NbAmelioration { get; set; }
    
    
    // Première amélioration 
    public int Upgrade1Cost { get; set; }
    public int Upgrade1Count { get; set; }

    
    // Seconde amélioration
    public int Upgrade2Cost { get; set; }
    public int Upgrade2Count { get; set; }
    
    // Accompagnements trouvés
    public int FoundAccompanimentCount { get; set; }
    public string? LastFoundAccompaniment { get; set; }
}