﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Diagnostics;

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

        /// <summary>
        /// Installs the selected version.
        /// </summary>
        /// <param name="version"></param>
        public void Install(T version)
        {
            var folder = Path.Combine(Path.GetTempPath(), "updater." + Guid.NewGuid().ToString("D"));
            Trace.WriteLine($"create folder '{folder}'", "Updater.Install");
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
                Trace.WriteLine($"download '{asset.Uri}'", "Updater.Install");
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

            Trace.WriteLine($"write '{update}'", "Updater.Install");

            using (var writer = new StreamWriter(update))
            {
                writer.Write(updateScript);
            }

            Trace.WriteLine($"start powershell", "Updater.Install");
            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = update,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,                
            });
        }

        /// <summary>
        /// Gets the location where versions can be quiered.
        /// </summary>
        protected abstract Uri VersionsUri { get; }

        /// <summary>
        /// Prepares a <see cref="HttpWebRequest"/> for the version storage.
        /// </summary>
        /// <param name="request"></param>
        protected virtual void PrepareRequest(HttpWebRequest request)
        {
            request.UserAgent = $"Calteo.Toolbox.Updater - {GetType().Assembly.GetName().Version}";
        }

        /// <summary>
        /// Parses the response from the <see cref="VersionsUri"/>.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected abstract IEnumerable<UpdateInfo> ParseVersions(HttpWebResponse response);
    }
}
