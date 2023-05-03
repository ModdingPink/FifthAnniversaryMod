using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FifthAnniversary
{
    internal class AssetExtractor
    {
        public static string LevelsPath = Path.Combine(Application.dataPath, "CustomFifthAnniversaryLevels");
        public static string SabersPath = "CustomSabers";
        public static void ExtractAssets() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Directory.CreateDirectory(SabersPath);

            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith(".saber"))
                {
                    string outputDirectory = Path.Combine(SabersPath, Path.GetFileName(resourceName).Replace("FifthAnniversary.Assets.Sabers.", ""));
                    using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
                    using (FileStream fileStream = new FileStream(outputDirectory, FileMode.Create))
                    {
                        resourceStream.CopyTo(fileStream);
                    }
                }
            }
        }

    }
}
