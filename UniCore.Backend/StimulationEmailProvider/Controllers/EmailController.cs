using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using StimulationEmailProvider.Services;

namespace StimulationEmailProvider.Controllers;

[ApiController]
[Route("email")]
public class EmailController : ControllerBase
{
    private readonly InMemoryEmailStore _store;

    public EmailController(InMemoryEmailStore store)
    {
        _store = store;
    }

    public sealed class SendEmailRequest
    {
        [JsonPropertyName("to")]
        public string To { get; set; } = string.Empty;

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;

        [JsonPropertyName("fromAddress")]
        public string? FromAddress { get; set; }

        [JsonPropertyName("fromDisplayName")]
        public string? FromDisplayName { get; set; }
    }

    public sealed class SendEmailResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = "accepted";
    }

    public sealed class MessageListResponse
    {
        public List<SimulatedEmailMessage> Data { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    [HttpPost("send")]
    public ActionResult<SendEmailResponse> Send([FromBody] SendEmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.To))
        {
            return BadRequest(new { error = "To is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Subject))
        {
            return BadRequest(new { error = "Subject is required." });
        }

        var message = new SimulatedEmailMessage
        {
            To = request.To.Trim(),
            Subject = request.Subject,
            Body = request.Body ?? string.Empty,
            FromAddress = request.FromAddress?.Trim() ?? "noreply@unicore.local",
            FromDisplayName = request.FromDisplayName?.Trim() ?? "UniCore"
        };

        _store.Add(message);

        return Ok(new SendEmailResponse { Id = message.Id, Status = "accepted" });
    }

    [HttpGet("messages")]
    public ActionResult<MessageListResponse> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        pageSize = Math.Clamp(pageSize, 1, 100);
        page = Math.Max(1, page);

        var items = _store.List(page, pageSize, out var total);
        return Ok(new MessageListResponse
        {
            Data = items.ToList(),
            Total = total,
            Page = page,
            PageSize = pageSize
        });
    }

    [HttpGet("messages/{id}")]
    public ActionResult<SimulatedEmailMessage> GetById(string id)
    {
        var message = _store.GetById(id);
        if (message == null)
        {
            return NotFound();
        }

        return Ok(message);
    }

    [HttpDelete("messages")]
    public IActionResult ClearInbox()
    {
        _store.ClearAll();
        return NoContent();
    }
}
