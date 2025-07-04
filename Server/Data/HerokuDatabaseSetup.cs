using Npgsql;

namespace Server.Data;

public static class HerokuDatabaseSetup
{
    public static string GetHerokuConnectionString()
    {
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        
        if (string.IsNullOrEmpty(databaseUrl))
        {
            return string.Empty;
        }
        
        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');
        
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/'),
            SslMode = SslMode.Prefer,
            TrustServerCertificate = true
        };

        return builder.ToString();
    }
}