using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace Toolbox.Update
{
    /// <summary>
    /// An updater that pulls its sources from github releases
    /// </summary>
    public class GitHubUpdater : Updater<GitHubUpdateInfo>
    {
        public GitHubUpdater(string owner, string repository)
        {
            Owner = owner;
            Repository = repository;
        }

        /// <summary>
        /// The owner of the project
        /// </summary>
        public string Owner { get; }

        /// <summary>
        /// The name of the repository
        /// </summary>
        public string Repository { get; }

        protected override Uri VersionsUri => new Uri($"https://api.github.com/repos/{Owner}/{Repository}/releases");

        protected override void PrepareRequest(HttpWebRequest request)
        {
            base.PrepareRequest(request);

            request.Accept = "application/vnd.github.v3+json";
        }

        protected override IEnumerable<UpdateInfo> ParseVersions(HttpWebResponse response) 
        {
            var document = JsonDocument.Parse(response.GetResponseStream());
            using (var enumerator = document.RootElement.EnumerateArray())
            {
                while (enumerator.MoveNext())
                {
                    yield return new GitHubUpdateInfo(enumerator.Current);
                }
            }            
        }
    }
}
