namespace UniCore.Helper.Constant
{
    public static class AnnouncementConstants
    {
        public static class Status
        {
            public const string Upcoming = "UPCOMING";
            public const string Active = "ACTIVE";
            public const string Expired = "EXPIRED";
        }

        public static class Type
        {
            public const string Normal = "NORMAL";
            public const string Important = "IMPORTANT";
            public const string Urgent = "URGENT";

            public static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
            {
                Normal, Important, Urgent
            };

            public static bool RequiresSideEffects(string type) =>
                string.Equals(type, Important, StringComparison.OrdinalIgnoreCase)
                || string.Equals(type, Urgent, StringComparison.OrdinalIgnoreCase);
        }

        public static class Scope
        {
            public const string Public = "PUBLIC";
            public const string Students = "STUDENTS"; // all active verified students
            public const string Department = "DEPARTMENT";
            public const string Class = "CLASS";
            public const string Course = "COURSE";
            public const string SpecificStudents = "SPECIFIC_STUDENTS";

            public static readonly HashSet<string> Supported = new(StringComparer.OrdinalIgnoreCase)
            {
                Public, Students, Department, Class, Course, SpecificStudents
            };
        }

        public static class EmailLogStatus
        {
            public const string Pending = "PENDING";
            public const string Sent = "SENT";
            public const string Failed = "FAILED";
        }

        public static class Whitelist
        {
            public const string Email = "EMAIL";
            public const string Domain = "DOMAIN";
            public const string Active = "ACTIVE";
        }
    }
}
