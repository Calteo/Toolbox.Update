using System;
using System.IO;

namespace Toolbox.Update
{
    /// <summary>
    /// Extensions to <see cref="object"/>.
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Gets an embedded resource as <see cref="string"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetResourceText(this Type type, string name)
        {
            var assembly = type.Assembly;
            using (var stream = assembly.GetManifestResourceStream(type, name))
            {
                if (stream == null) return null;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
