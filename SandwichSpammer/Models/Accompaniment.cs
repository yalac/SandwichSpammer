using SQLite;

namespace SandwichSpammer.Models;

public class Accompaniment
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } 
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double Calorie { get; set; }
    public string Image { get; set; }
}