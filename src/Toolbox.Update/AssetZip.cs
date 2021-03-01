using System;
using System.IO;
using System.IO.Compression;

namespace Toolbox.Update
{
    /// <summary>
    /// A zip file
    /// </summary>
    public class AssetZip : Asset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetZip"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        public AssetZip(string name, string uri) : base(name, uri)
        {            
        }

        private string ExtractFolder { get; set; }

        /// <inheritdoc/>
        public override void Downloaded(string target)
        {
            base.Downloaded(target);

            ExtractFolder = Path.Combine(Path.GetDirectoryName(target), Path.GetFileNameWithoutExtension(target));

            ZipFile.ExtractToDirectory(target, ExtractFolder);
        }

        /// <inheritdoc/>
        public override string GetInstallScript()
        {
            var script = typeof(AssetZip).GetResourceText("AssetZip.ps1");
            return 
                base.GetInstallScript() + Environment.NewLine 
                + $"$source = \"{ExtractFolder}\"" + Environment.NewLine
                + script;
        }
    }
}
