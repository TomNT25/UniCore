using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UniCore.Application.Contract.Service.v1;
using UniCore.Helper.Options;

namespace UniCore.Infrastructure.Service
{
    /// <summary>
    /// Dev/LAN email simulation — POST to StimulationEmailProvider (same VM as API, port 5289).
    /// </summary>
    public class HttpSimulationEmailSender : IEmailSender
    {
        private readonly HttpClient _httpClient;
        private readonly EmailOptions _options;
        private readonly ILogger<HttpSimulationEmailSender> _logger;

        public HttpSimulationEmailSender(
            HttpClient httpClient,
            IOptions<EmailOptions> options,
            ILogger<HttpSimulationEmailSender> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task SendAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
        {
            if (!_options.Enabled)
            {
                throw new InvalidOperationException("Email sending is disabled. Set Email:Enabled=true.");
            }

            var payload = new
            {
                to = toEmail,
                subject,
                body,
                fromAddress = _options.FromAddress,
                fromDisplayName = _options.FromDisplayName
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("email/send", payload, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError(
                        "Simulation email failed HTTP {Status} to {Email}: {Body}",
                        (int)response.StatusCode, toEmail, errorBody);
                    throw new InvalidOperationException($"Simulation email provider returned {(int)response.StatusCode}.");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Cannot reach simulation email provider for {Email}", toEmail);
                throw new InvalidOperationException(
                    "Cannot reach simulation email provider. Start StimulationEmailProvider on Email:SimulationBaseUrl.",
                    ex);
            }
        }
    }
}
