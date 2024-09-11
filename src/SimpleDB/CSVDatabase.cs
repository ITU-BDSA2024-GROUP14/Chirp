using System.Globalization;

using CsvHelper;

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    string DatabasePath;
    private static readonly CultureInfo CultureInfo = new CultureInfo("en-DE");
    public CSVDatabase(string databasePath)
    {
        this.DatabasePath = databasePath;
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        using (var reader = new StreamReader(DatabasePath))
        using (var csv = new CsvReader(reader, CultureInfo))
        {
            var records = csv.GetRecords<T>();
            return records;
        }
    }

    public void Store(T record)
    {
        using (var stream = File.Open(DatabasePath, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, CultureInfo))
        {
            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }
}
