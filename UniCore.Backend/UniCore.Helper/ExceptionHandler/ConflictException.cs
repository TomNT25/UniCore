namespace UniCore.Helper.ExceptionHandler
{
    public class ConflictException : Exception
    {
        public List<string> Errors { get; } = new();

        public ConflictException(string message) : base(message)
        {
            Errors.Add(message);
        }

        public ConflictException(string message, IEnumerable<string> errors) : base(message)
        {
            Errors.AddRange(errors);
        }
    }
}
