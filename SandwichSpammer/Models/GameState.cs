using SQLite;

namespace SandwichSpammer.Models;

public class GameState
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int TotalClicks { get; set; }
    
    /*
     public string UpgradeTake {get; set;}
     */
}