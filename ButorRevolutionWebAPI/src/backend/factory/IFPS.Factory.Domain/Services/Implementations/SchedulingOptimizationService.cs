using IFPS.Factory.Domain.Exceptions;
using IFPS.Factory.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Serilog;

namespace IFPS.Factory.Domain.Services.Implementations
{
    public class SchedulingOptimizationService : ISchedulingOptimizationService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public SchedulingOptimizationService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public string RunLayoutPlanning(string inputFilePath, string layoutDir, string strategyType)
        {          
            var exePath = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", "Optimizer", "exe");
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exePath = Path.Combine(exePath, "LayoutPlanning.exe");
            }
            else
            {
                exePath = Path.Combine(exePath, "lp.o");
            }
            var settingsPath = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", "Optimizer", "settings", "lp_settings.json");
            var outputPath = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", "Optimizer", "output");

            var startInfo = new ProcessStartInfo() { UseShellExecute = false };
            startInfo.FileName = exePath;
            startInfo.Arguments = inputFilePath + " " + strategyType + " " + settingsPath + " " + outputPath + " " + layoutDir;
         
            Log.Information("LayoutPlanning startInfo.FileName: {fname}", startInfo.FileName);
            Log.Information("LayoutPlanning startInfo.Arguments: {args}", startInfo.Arguments);

            int exitCode = -1;
            using (Process p = Process.Start(startInfo))
            {
                p.WaitForExit();
                exitCode = p.ExitCode;
            }
            
            switch (exitCode)
            {
                case 1:
                    var directory = new DirectoryInfo(outputPath);
                    var file = GetNewestFile(directory);
                    return Path.Combine(outputPath, file.Name);

                case 2:                        
                    throw new IFPSDomainException("LayoutPlan error: caught exception, check logfile for more info!");

                case 3:                        
                    throw new IFPSDomainException("LayoutPlan error: unknown internal error!");

                case 4:                       
                    throw new IFPSDomainException("LayoutPlan error: encountered invalid json file!");

                case 5:                        
                    throw new IFPSDomainException("LayoutPlan error: invalid search strategy!");

                default:                        
                    throw new IFPSDomainException("LayoutPlan error: invalid exit code!");
            }
        }

        public string RunJobScheduling(string inputFilePath, string scheduleDir, string cuttingsJsonFilePath, int shiftNumber, int shiftLength, string scheduleMode)
        {
            var exePath = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", "Optimizer", "exe");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                exePath = Path.Combine(exePath, "JobScheduling.exe");
            }
            else
            {
                exePath = Path.Combine(exePath, "js.o");
            }
            var settingsPath = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", "Optimizer", "settings", "js_settings.json");
            var outputPath = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", "Optimizer", "output");

            var startInfo = new ProcessStartInfo() { };
            startInfo.FileName = exePath;
            startInfo.Arguments = inputFilePath + " " + cuttingsJsonFilePath + " " + settingsPath + " " + outputPath + " " + shiftNumber + " " + shiftLength + " " + scheduleMode + " " + scheduleDir;

            Log.Information("JobScheduling startInfo.FileName: {fname}", startInfo.FileName);
            Log.Information("JobScheduling startInfo.Arguments: {args}", startInfo.Arguments);

            int exitCode = -1;
            using (Process p = Process.Start(startInfo))
            {
                p.WaitForExit();
                exitCode = p.ExitCode;
            }

            switch (exitCode)
            {
                case 0:
                    var directory = new DirectoryInfo(outputPath);
                    var file = GetNewestResultFile(directory);
                    return Path.Combine(outputPath, file.Name);

                case 1:                        
                    throw new IFPSDomainException("JobScheduling error: encountered invalid json file!");

                case 2:                    
                    throw new IFPSDomainException("JobScheduling error: invalid command line argument!");

                case 3:             
                    throw new IFPSDomainException("JobScheduling error: invalid configuration value, check settings.json!");

                case 4:       
                    throw new IFPSDomainException("JobScheduling error: error encountered in OR-Tools solver!");

                case 5:  
                    throw new IFPSDomainException("JobScheduling error: caught exception, check logfile for more info!");

                case 6:           
                    throw new IFPSDomainException("JobScheduling error: unknown internal error!");

                default:
                    throw new IFPSDomainException("JobScheduling error: invalid exit code!");
            }            
        }

        private FileInfo GetNewestFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .OrderByDescending(ent => (ent == null ? DateTime.MinValue : ent.LastWriteTime))
                .First();
        }

        private FileInfo GetNewestResultFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .Where(ent => ent.Name == "result.json")
                .OrderByDescending(ent => (ent == null ? DateTime.MinValue : ent.LastWriteTime))
                .First();
        }
    }
}
