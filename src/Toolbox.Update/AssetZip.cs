using System;
using System.IO;
using System.IO.Compression;

namespace Toolbox.Update
{
    public class AssetZip : Asset
    {
        public AssetZip(string name, string uri) : base(name, uri)
        {            
        }

        private string ExtractFolder { get; set; }

        public override void Downloaded(string target)
        {
            base.Downloaded(target);

            ExtractFolder = Path.Combine(Path.GetDirectoryName(target), Path.GetFileNameWithoutExtension(target));

            ZipFile.ExtractToDirectory(target, ExtractFolder);
        }

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
