using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Toolbox.Update
{
    /// <summary>
    /// Information about a version
    /// </summary>
    [DebuggerDisplay("{Name} - {Version} - {Published}")]
    public class UpdateInfo
    {
        public UpdateInfo(string name, string version, DateTime published, string uri)
        {
            Name = name;
            Version = version;
            Published = published;
            Uri = uri;
        }

        public string Name { get; }
        public string Version { get; }
        public DateTime Published { get; }
        public string Uri { get; set; }

        public List<Asset> Assets { get; } = new List<Asset>();
    }
}
