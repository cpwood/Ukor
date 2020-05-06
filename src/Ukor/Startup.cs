using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rssdp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Ukor.Configuration;
using Ukor.Logging;
using Ukor.Services;

namespace Ukor
{
    public class Startup
    {
        private SsdpDevicePublisher _publisher;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appOptions = Configuration.Get<ApplicationOptions>();
            var generalOptions = Configuration.Get<GeneralOptions>();

            ValidateOptions(generalOptions, appOptions);

            services.Configure<ApplicationOptions>(Configuration);
            services.Configure<GeneralOptions>(Configuration);
            services.Configure<LocalServerOptions>(x => x.IpAddress = GetLocalIpAddress());

            services.AddSingleton<SsdpLogger>();
            services.AddSingleton<IApplicationService, ApplicationService>();
            services.AddSingleton<IKeyPressService, KeyPressService>();

            services.AddControllers()
                .AddXmlSerializerFormatters()
                .AddMvcOptions(x =>
                {
                    var formatter = (XmlSerializerOutputFormatter)
                        x.OutputFormatters.FirstOrDefault(y => y.GetType() == typeof(XmlSerializerOutputFormatter));

                    if (formatter != null)
                    {
                        formatter.WriterSettings.OmitXmlDeclaration = false;
                        formatter.WriterSettings.Indent = true;
                    }
                });
        }

        private void ValidateOptions(GeneralOptions generalOptions, ApplicationOptions appOptions)
        {
            if (appOptions.Applications.Any(x =>
                x.LaunchKeySequence != null && x.LaunchKeySequence.Length != generalOptions.KeySequenceLength))
            {
                throw new InvalidDataException(
                    $"All LaunchKeySequence arrays must have a length of {generalOptions.KeySequenceLength}.");
            }

            if (appOptions.Applications.Any(x => x.Action == Application.ActionType.CSharp && x.CSharpDetails == null))
            {
                throw new InvalidDataException("All CSharp actions must have a CSharpDetails property defined and populated.");
            }

            if (appOptions.Applications.Any(x =>
                x.Action == Application.ActionType.CommandLine && x.CommandLineDetails == null))
            {
                throw new InvalidDataException("All CommandLine actions must have a CommandLineDetails property defined and populated.");
            }

            if (appOptions.Applications.Any(x =>
                x.Action == Application.ActionType.CSharp &&
                (string.IsNullOrEmpty(x.CSharpDetails.AssemblyPath) ||
                 string.IsNullOrEmpty(x.CSharpDetails.ClassName))))
            {
                throw new InvalidDataException("All CSharp actions must have both an AssemblyPath and ClassName defined.");
            }

            if (appOptions.Applications.Any(x =>
                x.Action == Application.ActionType.CommandLine &&
                string.IsNullOrEmpty(x.CommandLineDetails.Executable)))
            {
                throw new InvalidDataException("All CommandLine actions must have an Executable defined.");
            }

            if (appOptions.Applications.Any(x => x.Id < 1000))
            {
                throw new InvalidDataException("All applicaiton Id values must be 1000 or above.");
            }

            var duplicates = appOptions.Applications.GroupBy(x => x.Id).Select(x => new {Id = x.Key, Count = x.Count()})
                .Where(x => x.Count > 1).ToArray();

            if (duplicates.Any())
            {
                var duplicated = duplicates.Select(x => x.Id).ToArray();
                throw new InvalidDataException($"The following application Id values are duplicated: {string.Join(',', duplicated)}");
            }
        }

        private string GetLocalIpAddress()
        {
            // Find the network card being used for internet-based traffic
            // and then get the IP address associated with it.
            using var client = new UdpClient();
            var ep = new IPEndPoint(IPAddress.Parse("8.8.8.8"), 53);
            client.Connect(ep);
            var local = (IPEndPoint) client.Client.LocalEndPoint;
            var ip = local.Address.ToString();
            client.Close();
            return ip;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseRewriter(new RewriteOptions()
                .AddRewrite("dial/dd\\.xml", "Device.xml", true));

            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("Device.xml");
            app.UseDefaultFiles(options);

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            ConfigureSsdp(
                app.ApplicationServices.GetService<IOptions<LocalServerOptions>>(),
                app.ApplicationServices.GetService<SsdpLogger>());
        }

        private void ConfigureSsdp(
            IOptions<LocalServerOptions> localServerOptions,
            SsdpLogger logger)
        {
            var device = new SsdpRootDevice()
            {
                CacheLifetime = TimeSpan.FromHours(1),
                Location = new Uri($"{localServerOptions.Value.RootUrl}/"),
                UrlBase = new Uri($"{localServerOptions.Value.RootUrl}/"),
                DeviceType = "urn:roku-com:device:player:1-0:1",
                DeviceTypeNamespace = "schemas-upnp-org",
                DeviceVersion = 1,
                Uuid = "29600009-5406-1005-8080-1234567890ab",
                Udn = "uuid:29600009-5406-1005-8080-1234567890ab",
                FriendlyName = "Ukor Server",
                Manufacturer = "Roku",
                ManufacturerUrl = new Uri("http://www.roku.com/"),
                ModelDescription = "Roku Streaming Player Network Media",
                ModelName = "Ukor Server",
                ModelNumber = "3810EU",
                ModelUrl = new Uri("http://www.roku.com/"),
                SerialNumber = "YH009E000001",
                Usn = "uuid:roku:ecp:YH009E000001",
                NotificationType = "roku:ecp"
            };

            device.CustomResponseHeaders.Add(new CustomHttpHeader("Server", "Roku/9.2.0, UPnP/1.0"));
            device.CustomResponseHeaders.Add(new CustomHttpHeader("device-group.roku.com", "1E3DE502613555ACA315"));
            device.CustomResponseHeaders.Add(new CustomHttpHeader("WAKEUP", "MAC=ac:ae:01:02:03:04, Timeout=10"));

            var ecpService = new SsdpService
            {
                ServiceType = "ecp",
                ServiceTypeNamespace = "roku-com",
                ServiceVersion = 1,
                Uuid = "ecp1-0",
                ScpdUrl = new Uri("ecp_SCPD.xml", UriKind.Relative),
                ControlUrl = new Uri("roku:0")
            };

            device.AddService(ecpService);

            var dialService = new SsdpService
            {
                ServiceType = "dial",
                ServiceTypeNamespace = "dial-multiscreen-org",
                ServiceVersion = 1,
                Uuid = "dial1-0",
                ScpdUrl = new Uri("dial_SCPD.xml", UriKind.Relative),
                ControlUrl = new Uri("roku:1")
            };

            device.AddService(dialService);

            _publisher =
                new SsdpDevicePublisher {StandardsMode = SsdpStandardsMode.Relaxed, Log = logger};

            _publisher.AddDevice(device);
            _publisher.NotificationBroadcastInterval = TimeSpan.FromMinutes(10);
        }
    }
}
