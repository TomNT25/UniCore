namespace UniCore.Helper.Options
{
    public class EmailOptions
    {
        public const string SectionName = "Email";

        public bool Enabled { get; set; }
        public string Provider { get; set; } = "HttpSimulation";
        public string SimulationBaseUrl { get; set; } = "http://127.0.0.1:5289";
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public string FromDisplayName { get; set; } = "UniCore Announcements";
    }
}
