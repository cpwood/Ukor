using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Ukor.Configuration;

namespace Ukor.Services
{
    public class KeyPressService : IKeyPressService
    {
        private readonly IOptions<GeneralOptions> _generalOptions;
        private readonly IOptions<ApplicationOptions> _applicationOptions;
        private readonly IApplicationService _appService;
        private readonly ILogger<KeyPressService> _logger;
        private readonly ConcurrentQueue<Application.KeyPress> _presses = new ConcurrentQueue<Application.KeyPress>();

        public KeyPressService(
            IOptions<GeneralOptions> generalOptions,
            IOptions<ApplicationOptions> applicationOptions,
            IApplicationService appService,
            ILogger<KeyPressService> logger)
        {
            _generalOptions = generalOptions;
            _applicationOptions = applicationOptions;
            _appService = appService;
            _logger = logger;
        }

        public async Task HandleKeyPress(Application.KeyPress keyPress)
        {
            _logger.LogInformation($"Received KeyPress {keyPress} at {DateTime.Now.ToLongTimeString()}");

            _presses.Enqueue(keyPress);

            while (_presses.Count > _generalOptions.Value.KeySequenceLength)
            {
                _presses.TryDequeue(out _);
            }

            var matched = _applicationOptions.Value.Applications.FirstOrDefault(x =>
                x.LaunchKeySequence != null && x.LaunchKeySequence.SequenceEqual(_presses));

            if (matched != null)
            {
                _logger.LogInformation($"Launching '{matched.Name}' application..");
                _presses.Clear();
                await _appService.DoActionAsync(matched);
            }
        }
    }
}
