using System.Linq;
using System.Text.Json;
using System.IO;
using System;

namespace Toolbox.Update
{
    /// <summary>
    /// Information about an update from github releases
    /// </summary>
    public class GitHubUpdateInfo : UpdateInfo
    {
        public GitHubUpdateInfo(JsonElement element)
            :base(
                 element.GetProperty("name").GetString(),
                 element.GetProperty("tag_name").GetString(),
                 element.GetProperty("published_at").GetDateTime(),
                 element.GetProperty("html_url").GetString()
                 )
        {
            var assetsElement = element.GetProperty("assets");
            var assetElements = new JsonElement[assetsElement.GetArrayLength()];

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
                            throw new NotImplementedException($"Asset '{name}' can not be processes.");
                    }
                }
            }            
        }
    }
}
