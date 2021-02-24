using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Toolbox.Update
{
    /// <summary>
    /// A general updater 
    /// </summary>
    public abstract class Updater<T> where T : UpdateInfo
    {       
        /// <summary>
        /// Returns the latest version
        /// </summary>
        /// <returns><c>null</c>, if no version exists.</returns>
        public T GetLatestVersion()
        {
            var request = WebRequest.CreateHttp(VersionsUri);

            PrepareRequest(request);

            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK) return null;

            return ParseVersions(response).OfType<T>().OrderByDescending(i => i.Published).FirstOrDefault();
        }

        /// <summary>
        /// Returns the latest version
        /// </summary>
        /// <returns><c>null</c>, if no version exists.</returns>
        public IEnumerable<T> GetVersions()
        {
            var request = WebRequest.CreateHttp(VersionsUri);

            PrepareRequest(request);

            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK) return null;

            return ParseVersions(response).OfType<T>().OrderByDescending(i => i.Published);
        }

        public void Install(T version)
        {
            var folder = Path.Combine(Path.GetTempPath(), "updater." + Guid.NewGuid().ToString("D"));
            Directory.CreateDirectory(folder);

            var updateScript = typeof(Updater<>).GetResourceText("update.ps1");

            var process = Process.GetCurrentProcess();

            var variables = new StringBuilder();
            variables.AppendLine($"# variables");
            variables.AppendLine($"$process = {process.Id}");
            variables.AppendLine($"$target = \"{AppDomain.CurrentDomain.BaseDirectory}\"");
            variables.AppendLine($"$executable = \"{process.MainModule.FileName}\"");
            variables.AppendLine($"$arguments = @({string.Join(",", Environment.GetCommandLineArgs().Skip(1).Select(a => $"\"{a}\""))})");

            updateScript = updateScript.Replace("# @variables", variables.ToString());

            var installers = new StringBuilder();

            foreach (var asset in version.Assets.Where(a => a.Enabled))
            {
                var request = WebRequest.CreateHttp(asset.Uri);
                PrepareRequest(request);
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Downloading {request.RequestUri} failed with {response.StatusCode}.");

                var target = Path.Combine(folder, asset.Name);
                using (var stream = new FileStream(target, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    response.GetResponseStream().CopyTo(stream);
                }
                asset.Downloaded(target);

                installers.AppendLine($"# install {asset.Name} with {asset.GetType().FullName}");
                installers.AppendLine(asset.GetInstallScript());                
            }

            updateScript = updateScript.Replace("# @installers", installers.ToString());

            var update = Path.Combine(folder, "update.ps1");
            using (var writer = new StreamWriter(update))
            {
                writer.Write(updateScript);
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = update,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,                
            });
        }

        protected abstract Uri VersionsUri { get; }

        protected virtual void PrepareRequest(HttpWebRequest request)
        {
            request.UserAgent = $"Calteo.Toolbox.Updater - {GetType().Assembly.GetName().Version}";
        }

        protected abstract IEnumerable<UpdateInfo> ParseVersions(HttpWebResponse response);
    }
}
