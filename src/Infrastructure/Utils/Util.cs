namespace ClubApi.Infrastructure.Utils;

public class Util
{
    public static DateTime GetDate()
    {
        return DateTime.UtcNow.AddHours(-5);
    }
}