using System.Globalization;
using CsvHelper;

namespace Chirp.CSVDBService;

public sealed class CheepDatabase : IDatabaseRepository<Cheep>
{
    public string DatabasePath { get; set; } = "../../data/chirp_cli_db.csv";

    private static readonly CultureInfo CultureInfo = new("en-DE");
    private static readonly Lazy<CheepDatabase> lazy = new(() => new CheepDatabase());

    public static CheepDatabase Instance => lazy.Value;

    /// <summary>
    /// Reads Cheeps from the CSV database, with an optional limit on the number of Cheeps returned.
    /// </summary>
    /// <param name="limit">Optional parameter to specify the maximum number of Cheeps to return.</param>
    /// <returns>An enumerable collection of Cheeps.</returns>
    public IEnumerable<Cheep> Read(int? limit = null)
    {
        using StreamReader reader = new(DatabasePath);
        using CsvReader csvReader = new(reader, CultureInfo);
        var cheeps = csvReader.GetRecords<Cheep>();
        return limit.HasValue ? cheeps.TakeLast(limit.Value).ToList() : cheeps.ToList();
    }

    public void Store(Cheep record)
    {
        using (var stream = File.Open(DatabasePath, FileMode.Append))
        using (StreamWriter writer = new(stream))
        using (CsvWriter csv = new(writer, CultureInfo))
        {
            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }

    public void Reset()
    {
        // ensure the parent directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath));

        // clear the file
        File.WriteAllText(DatabasePath, string.Empty);

        using var stream = File.Open(DatabasePath, FileMode.Append);
        using StreamWriter writer = new(stream);
        using var csvWriter = new CsvWriter(writer, CultureInfo);
        {
            csvWriter.WriteHeader<Cheep>();
            csvWriter.NextRecord();
        }
    }
    
    public void EnsureCreated()
    {
        if (!File.Exists(DatabasePath))
        {
            Reset();
        }
    }
}