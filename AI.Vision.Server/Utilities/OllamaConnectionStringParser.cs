namespace AI.Vision.Server.Utilities
{
    public class OllamaConnectionStringParser
    {
        public static (string Endpoint, string Model) Parse(string connectionString)
        {
            var parts = connectionString.Split(';')
                .Select(p => p.Split('='))
                .ToDictionary(p => p[0], p => p[1]);

            return (parts["Endpoint"], parts["Model"]);
        }
    }
}
