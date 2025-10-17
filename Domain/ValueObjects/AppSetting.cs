namespace Domain.ValueObjects;

public class AppSetting
{
    public Connection? Connections { get; set; }
}
public class Connection
{
    public string SqlServerConnectionString { get; set; } = string.Empty;
}