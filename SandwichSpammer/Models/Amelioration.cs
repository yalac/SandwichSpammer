using SQLite;

namespace SandwichSpammer.Models;

public class Amelioration
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? Name { get; set; }
    public int InitialCost { get; set; }
    public int NbAmelioration { get; set; }
    
}