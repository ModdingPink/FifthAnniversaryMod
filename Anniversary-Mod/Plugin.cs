using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using SiraUtil.Zenject;
using FifthAnniversary.Installers;
using IPALogger = IPA.Logging.Logger;
using Conf = IPA.Config.Config;
using HarmonyLib;
using System.Reflection;
using System.IO;
using UnityEngine;
using FifthAnniversary;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.IO.Compression;

namespace FifthAnniversary
{
    [Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
    public class Plugin
    {
        internal static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();



        [Init]
        public Plugin(IPALogger logger, Conf conf, Zenjector zenjector)
        {

            Config.Instance = conf.Generated<Config>();

            zenjector.UseLogger(logger);
            zenjector.Install<AppInstaller>(Location.App, Config.Instance);
            zenjector.Install<MenuInstaller>(Location.Menu);

            SongCore.Collections.AddSeperateSongFolder("BSMG's Fifth Anniversary Music Pack", AssetExtractor.LevelsPath, SongCore.Data.FolderLevelPack.NewPack, SongCore.Utilities.Utils.LoadSpriteFromResources("FifthAnniversary.Assets.BSMG_Pack_Cover.png"), false, true);

            Task.Run(() =>
            {
                SongDownloader.DownloadSongs("https://beatsaver.com/api/playlists/id/89418/0");
            });

            if (!Config.Instance.itemsDownloaded)
            {
                AssetExtractor.ExtractAssets();
                Config.Instance.itemsDownloaded = true;
            }
        }

        [OnStart]
        public void OnStart()
        {
            var harmony = new Harmony("BSMG.Anniversary");
            harmony.PatchAll(Assembly);
         
        }
    }
}
