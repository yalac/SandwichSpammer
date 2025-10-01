using SandwichSpammer.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace SandwichSpammer.Database;

public partial class DatabaseService
{
    private SQLiteConnection _database;
    public const string DatabaseFilename = "SpammerDB.db3";
    public const SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLiteOpenFlags.SharedCache;
    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    
    
    public DatabaseService()
    {
        _database = new SQLiteConnection(DatabasePath, Flags);
        _database.CreateTable<Accompaniment>();
        _database.CreateTable<Amelioration>();
        //Création des tables
    }
    
    public List<Accompaniment> GetAccompaniments()
    {
        return _database.GetAllWithChildren<Accompaniment>(recursive: true)
            .OrderBy(x=>x.Name)
            .ToList();
    }
    
    // ajouter des accompagnement
    public int AddAccompaniment(string name, string description, double calorie, string image)
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Beurre",
            Description = "Beurre complet salé, bon pour la santé !!!",
            Calorie = 0.1,
            Image = "Ressources/Images/Accompaniment/Beurre.png"
        };

        return _database.Insert(newAccompaniment);
    }
}