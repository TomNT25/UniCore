namespace UniCore.Helper.Constant
{
    public static class AnnouncementTargetConstants
    {
        public const int DefaultLimit = 10;
        public const int MaxLimit = 50;

        public static class Domain
        {
            public const string Course = "course";
            public const string Class = "class";
            public const string Students = "students";
            public const string Department = "department";

            public static readonly HashSet<string> Supported = new(StringComparer.OrdinalIgnoreCase)
            {
                Course, Class, Students, Department
            };
        }
    }
}
