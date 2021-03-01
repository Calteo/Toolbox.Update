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
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateInfo"/> class.
        /// </summary>
        /// <param name="name">Name of the version</param>
        /// <param name="version">Version tag</param>
        /// <param name="description">Description of that version.</param>
        /// <param name="published">Timestamp, when this version was published.</param>
        /// <param name="uri">Where this version can be viewed.</param>
        public UpdateInfo(string name, string version, string description, DateTime published, string uri)
        {
            Name = name;
            Version = version;
            Description = description;
            Published = published;
            Uri = uri;
        }

        /// <summary>
        /// Gets the name of the version.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the version tag.
        /// </summary>
        public string Version { get; }
        /// <summary>
        /// Gets the time when the version was published.
        /// </summary>
        public DateTime Published { get; }
        /// <summary>
        /// Gets the description of the version.
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Gets the location where the version is found.
        /// </summary>
        public string Uri { get;  }
        
        /// <summary>
        /// Gets the list of <see cref="Asset"/>s in this version.
        /// </summary>
        public List<Asset> Assets { get; } = new List<Asset>();
    }
}