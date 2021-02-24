using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Toolbox.Update
{
    public static class ObjectExtension
    {
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
