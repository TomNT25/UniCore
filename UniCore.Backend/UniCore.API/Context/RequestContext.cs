namespace UniCore.API.Context
{
    public class RequestContext : IRequestContext
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
