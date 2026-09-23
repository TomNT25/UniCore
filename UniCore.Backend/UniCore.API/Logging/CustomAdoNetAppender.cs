using log4net.Appender;

namespace UniCore.API.Logging
{
    public class CustomAdoNetAppender : AdoNetAppender
    {
        public static string? ConnectionStringOverride { get; set; }

        public override void ActivateOptions()
        {
            if (!string.IsNullOrEmpty(ConnectionStringOverride))
            {
                ConnectionString = ConnectionStringOverride;
            }

            base.ActivateOptions();
        }
    }
}
