using System.Globalization;

using CsvHelper;

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private string DatabasePath;
    private static readonly CultureInfo CultureInfo = new("en-DE");

    public CSVDatabase(string databasePath)
    {
        DatabasePath = databasePath;
    }

    /// <summary>
    /// Reads records from the CSV database, with an optional limit on the number of records returned.
    /// </summary>
    /// <param name="limit">Optional parameter to specify the maximum number of records to return.</param>
    /// <returns>An enumerable collection of records of type T.</returns>
    public IEnumerable<T> Read(int? limit = null)
    {
        using StreamReader reader = new(DatabasePath);
        using CsvReader csv = new(reader, CultureInfo);
        IEnumerable<T> records = csv.GetRecords<T>();
        return limit.HasValue ? records.TakeLast(limit.Value).ToList() : records.ToList();
    }

    public void Store(T record)
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