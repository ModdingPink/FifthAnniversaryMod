using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace FifthAnniversary
{
    public class Config
    {
        public static Config? Instance { get; set; }

        public virtual bool itemsDownloaded { get; set; } = false;

    }
}