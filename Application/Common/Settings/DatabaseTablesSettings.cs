namespace Application.Common.Settings;

public class DatabaseTablesSettings
{
    public string LogsTable { get; set; } = "Logs";
    public string CacheTable { get; set; } = "Cache";
    public string CacheSchema { get; set; } = "dbo";
}
