using System.Text.Json;
using System.IO;

namespace Toolbox.Update
{
    /// <summary>
    /// Information about an update from github releases
    /// </summary>
    public class GitHubUpdateInfo : UpdateInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubUpdateInfo"/> class.
        /// </summary>
        /// <param name="element"></param>
        public GitHubUpdateInfo(JsonElement element)
            :base(
                 element.GetProperty("name").GetString(),
                 element.GetProperty("tag_name").GetString(),
                 element.GetProperty("body").GetString(),
                 element.GetProperty("published_at").GetDateTime(),
                 element.GetProperty("html_url").GetString()
                 )
        {
            var assetsElement = element.GetProperty("assets");
            
            using (var enumerator = assetsElement.EnumerateArray())
            {
                while (enumerator.MoveNext())
                {
                    var assetElement = enumerator.Current;
                    var name = assetElement.GetProperty("name").GetString();
                    var uri = assetElement.GetProperty("browser_download_url").GetString();

                    var extension = Path.GetExtension(name).ToLower();
                    switch (extension)
                    {
                        case ".zip":
                            Assets.Add(new AssetZip(name, uri));
                            break;
                        default:
                            Assets.Add(new AssetFile(name, uri));
                            break;
                    }
                }
            }            
        }
    }
}
