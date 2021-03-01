namespace Toolbox.Update
{
    /// <summary>
    /// Generic file asset
    /// </summary>
    public class AssetFile : Asset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetFile"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        public AssetFile(string name, string uri) : base(name, uri)
        {
        }

        /// <inheritdoc/>
        public override void Downloaded(string target)
        {
            base.Downloaded(target);
        }

        /// <inheritdoc/>
        public override string GetInstallScript()
        {
            return base.GetInstallScript();
        }
    }
}
