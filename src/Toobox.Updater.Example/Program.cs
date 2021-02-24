using System;
using System.Linq;
using Toolbox.Update;

namespace Toobox.Updater.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var updater = new GitHubUpdater("Calteo", "Image.Import");
            var latest = updater.GetLatestVersion();

            var versions = updater.GetVersions().ToArray();

            updater.Install(latest);
        }
    }
}
