using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ukor.Configuration;

namespace Ukor.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ILogger<ApplicationService> _logger;

        public ApplicationService(ILogger<ApplicationService> logger)
        {
            _logger = logger;

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                // check for assemblies already loaded
                var dependency = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.FullName == args.Name);

                if (dependency != null)
                    return dependency;

                // Try to load by filename - split out the filename of the full assembly name
                // and append the base path of the original assembly (ie. look in the same dir)
                var filename = args.Name.Split(',')[0] + ".dll".ToLower();
                var folder = new FileInfo(args.RequestingAssembly.Location).DirectoryName;

                var asmFile = Path.Combine(folder, filename);

                try
                {
                    return Assembly.LoadFrom(asmFile);
                }
                catch (Exception)
                {
                    return null;
                }
            };
        }

        public async Task DoActionAsync(Application application)
        {
            switch (application.Action)
            {
                case Application.ActionType.None:
                    return;
                case Application.ActionType.CSharp:
                    _logger.LogInformation($"Launching '{application.Name}' application..");

                    if (application.ActionClass == null)
                    {
                        var assembly = Assembly.LoadFile(application.CSharpDetails.AssemblyPath);
                        var type = assembly.GetTypes().FirstOrDefault(x => x.Name == application.CSharpDetails.ClassName);

                        if (type != null)
                            application.ActionClass = (ICSharpAction) Activator.CreateInstance(type);
                    }

                    if (application.ActionClass != null)
                        await application.ActionClass.DoActionAsync(application);
                    break;
                case Application.ActionType.CommandLine:
                    _logger.LogInformation($"Launching '{application.Name}' application..");

                    var i = new ProcessStartInfo(application.CommandLineDetails.Executable,
                        application.CommandLineDetails.Arguments)
                    {
                        WorkingDirectory = application.CommandLineDetails.WorkingFolder
                    };

                    var p = Process.Start(i);

                    if (application.CommandLineDetails.WaitForExit)
                        p?.WaitForExit();

                    return;
            }
        }
    }
}
