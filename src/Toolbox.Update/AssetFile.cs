namespace Toolbox.Update
{
    public class AssetFile : Asset
    {
        public AssetFile(string name, string uri) : base(name, uri)
        {
        }

        public override void Downloaded(string target)
        {
            base.Downloaded(target);
        }

        public override string GetInstallScript()
        {
            return base.GetInstallScript();
        }
    }
}
