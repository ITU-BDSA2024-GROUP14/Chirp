using System.Globalization;

using CsvHelper;

namespace SimpleDB;

public sealed class CheepDatabase : IDatabaseRepository<Cheep>
{
    private static CheepDatabase? instance;
    private static readonly string DatabasePath = "../../data/chirp_cli_db.csv";
    private static readonly CultureInfo CultureInfo = new("en-DE");
    private static readonly Lazy<CheepDatabase> lazy = new Lazy<CheepDataBase>(() => new CheepDataBase());
    
    
    public static CheepDatabase Instance
    {
        get
        {
            return lazy.Value;
        }
    }

    /// <summary>
    /// Reads Cheeps from the CSV database, with an optional limit on the number of Cheeps returned.
    /// </summary>
    /// <param name="limit">Optional parameter to specify the maximum number of Cheeps to return.</param>
    /// <returns>An enumerable collection of Cheeps.</returns>
    public IEnumerable<Cheep> Read(int? limit = null)
    {
        using StreamReader reader = new(DatabasePath);
        using CsvReader csv = new(reader, CultureInfo);
        IEnumerable<Cheep> records = csv.GetRecords<Cheep>();
        return limit.HasValue ? records.TakeLast(limit.Value).ToList() : records.ToList();
    }

    public void Store(Cheep record)
    {
        using (FileStream stream = File.Open(DatabasePath, FileMode.Append))
        using (StreamWriter writer = new(stream))
        using (CsvWriter csv = new(writer, CultureInfo))
        {
            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }
}