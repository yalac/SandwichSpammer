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
        //Création des tables
    }
    
    // ajouter des accompagnement
    public int AddBeurre(string name, string description, double calorie, int initialCost, int nbAmelioration, string image)
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Beurre",
            Description = "Beurre complet salé, bon pour la santé !!!",
            Calorie = 0.1,
            InitialCost = 15,
            NbAmelioration = 0,
            Image = "Resources/Images/Accompaniment/beurre.png"
        };

        return _database.Insert(newAccompaniment);
    }
    
    public int AddCamembert(string name, string description, double calorie, string image)
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Camembert",
            Description = "Aussi puant que bon !!!",
            Calorie = 1,
            InitialCost = 100,
            NbAmelioration = 0,
            Image = "Resources/Images/Accompaniment/camembert.png"
        };

        return _database.Insert(newAccompaniment);
    }
    
    public int AddEmmental(string name, string description, double calorie, string image)
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Emmental",
            Description = "Le classique, l'original !!!",
            Calorie = 8,
            InitialCost = 1100,
            NbAmelioration = 0,
            Image = "Resources/Images/Accompaniment/emmental.png"
        };

        return _database.Insert(newAccompaniment);
    }
    
    public int AddJambon(string name, string description, double calorie, string image)
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Jambon",
            Description = "Une bonne tranche bien fraiche !!!",
            Calorie = 47,
            InitialCost = 12000,
            NbAmelioration = 0,
            Image = "Resources/Images/Accompaniment/jambon.png"
        };

        return _database.Insert(newAccompaniment);
    }
    
    public int AddKetchup(string name, string description, double calorie, string image)
    {
        var newAccompaniment = new Accompaniment
        {
            Name = "Ketchup",
            Description = "Un assaisonnement haut en couleur (surtout en rouge) !!!",
            Calorie = 260,
            InitialCost = 130000,
            NbAmelioration = 0,
            Image = "Resources/Images/Accompaniment/ketchup.png"
        };

        return _database.Insert(newAccompaniment);
    }
    
    public List<Accompaniment> GetAccompaniments()
    {
        return _database.GetAllWithChildren<Accompaniment>(recursive:true)
            .OrderBy(x=>x.Name)
            .ToList();
    }
}