namespace mge.API.Models
{
    public class DatabaseSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string BuildConnectionString()
        {
            return $"Host={Host};" +
            $"Port={Port};" +
            $"Database={Database};" +
            $"Username={Username};" +
            $"Password={Password};";
        }
    }
}