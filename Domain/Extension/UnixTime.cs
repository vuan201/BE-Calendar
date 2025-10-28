namespace Domain.Extension;

public static class UnixTime
{
    public static long ToUnixTime(this DateTime dateTime)
    {
        var utc = dateTime.ToUniversalTime();
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)(utc - epoch).TotalSeconds;
    }
    public static DateTime ToDateTime(this long unixTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddSeconds(unixTime);
    }
}
