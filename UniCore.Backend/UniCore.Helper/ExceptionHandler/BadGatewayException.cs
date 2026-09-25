namespace UniCore.Helper.ExceptionHandler
{
    public class BadGatewayException : Exception
    {
        public List<string> Errors { get; } = new();

        public BadGatewayException(string message) : base(message)
        {
            Errors.Add(message);
        }

        public BadGatewayException(string message, IEnumerable<string> errors) : base(message)
        {
            Errors.AddRange(errors);
        }
    }
}
