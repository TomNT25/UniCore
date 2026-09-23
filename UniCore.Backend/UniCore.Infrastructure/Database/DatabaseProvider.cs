namespace UniCore.Infrastructure.Database
{
    public class DatabaseProvider
    {
        public string Name { get; }
        public string ConnectionString { get; }

        public DatabaseProvider(string name, string connectionString)
        {
            Name = name;
            ConnectionString = connectionString;
        }
    }
}
