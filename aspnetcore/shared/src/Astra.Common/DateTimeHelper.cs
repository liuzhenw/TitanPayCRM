namespace Astra.Common;

public static class DateTimeHelper
{
    public static DateTimeOffset RoundMinute(this DateTimeOffset time)
    {
        return time - (time.TimeOfDay - TimeSpan.FromMinutes((int)time.TimeOfDay.TotalMinutes));
    }
    
    public static DateTimeOffset RoundHour(this DateTimeOffset time)
    {
        return time - (time.TimeOfDay - TimeSpan.FromHours((int)time.TimeOfDay.TotalHours));
    }
    
    public static DateTimeOffset RoundDay(this DateTimeOffset time)
    {
        return time - time.TimeOfDay;
    }
}