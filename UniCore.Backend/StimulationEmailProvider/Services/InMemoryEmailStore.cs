using System.Collections.Concurrent;

namespace StimulationEmailProvider.Services;

public sealed class InMemoryEmailStore
{
    private readonly ConcurrentDictionary<string, SimulatedEmailMessage> _messages = new();

    public SimulatedEmailMessage Add(SimulatedEmailMessage message)
    {
        _messages[message.Id] = message;
        return message;
    }

    public IReadOnlyList<SimulatedEmailMessage> List(int page, int pageSize, out int total)
    {
        var ordered = _messages.Values.OrderByDescending(m => m.CreatedAtUtc).ToList();
        total = ordered.Count;
        var skip = Math.Max(0, (page - 1) * pageSize);
        return ordered.Skip(skip).Take(pageSize).ToList();
    }

    public SimulatedEmailMessage? GetById(string id) =>
        _messages.TryGetValue(id, out var msg) ? msg : null;

    public void ClearAll() => _messages.Clear();
}

public sealed class SimulatedEmailMessage
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public string To { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public string FromAddress { get; init; } = string.Empty;
    public string FromDisplayName { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
}
