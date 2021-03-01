using System.Diagnostics;

namespace Toolbox.Update
{
    /// <summary>
    /// An asset that is part of a <see cref="UpdateInfo"/>.
    /// </summary>
    [DebuggerDisplay("{Name} - {Uri}")]
    public class Asset
    {
        static Asset()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        public Asset(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        /// <summary>
        /// Gets the name of the asset.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the location of the asset.
        /// </summary>
        public string Uri { get; }
        /// <summary>
        /// Gets or set, if this asset is processed during the installation.
        /// </summary>
        /// <see cref="Updater{T}.Install(T)"/>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Exceuted after the asset has been downloaded.
        /// </summary>
        /// <param name="target"></param>
        public virtual void Downloaded(string target)
        {
        }

        /// <summary>
        /// Creates the powershell script to install this asset.
        /// </summary>
        /// <returns></returns>
        public virtual string GetInstallScript()
        {
            return $"$name = \"{Name}\"";
        }
    }
}