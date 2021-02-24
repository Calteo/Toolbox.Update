using System;
using System.Collections.Generic;
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

        public Asset(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public string Name { get; }
        public string Uri { get; }
        public bool Enabled { get; set; } = true;

        public virtual void Downloaded(string target)
        {
        }

        public virtual string GetInstallScript()
        {
            return $"$name = \"{Name}\"";
        }
    }
}
