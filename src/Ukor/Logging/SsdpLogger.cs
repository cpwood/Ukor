using Microsoft.Extensions.Logging;
using Rssdp;
using Rssdp.Infrastructure;
using System;
using System.Reflection;

namespace Ukor.Logging
{
    public class SsdpLogger : ISsdpLogger
    {
        private readonly ILogger<SsdpDevicePublisher> _logger;

        public SsdpLogger(ILogger<SsdpDevicePublisher> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogVerbose(string message)
        {
            _logger.LogTrace(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        public void ApplyTo(SsdpDevicePublisher publisher)
        {
            var type = typeof(SsdpDevicePublisherBase);
            var field = type.GetField("_Log", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
                throw new NullReferenceException("Cannot find _Log private field in SsdpDevicePublisher class.");

            field.SetValue(publisher, this);
        }
    }
}
