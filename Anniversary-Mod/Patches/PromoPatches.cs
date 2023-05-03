using HarmonyLib;
using IPA.Utilities;
using SongCore.OverrideClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static SelectLevelCategoryViewController;

namespace FifthAnniversary.Patches
{
    internal class PromoPatches
    {

        [HarmonyPatch(typeof(LevelFilteringNavigationController), nameof(LevelFilteringNavigationController.ShowPacksInSecondChildController))]
        static class PackPreselect
        {
            static void Prefix(ref string ____levelPackIdToBeSelectedAfterPresent)
            {
                if (____levelPackIdToBeSelectedAfterPresent != null || !PackPromoButtonWasPressed.buttonWasPressed)
                    return;

                ____levelPackIdToBeSelectedAfterPresent = "BSMG's Fifth Anniversary Music Pack";
            }
        }


        [HarmonyPatch(typeof(LevelSelectionFlowCoordinator), "DidActivate")]
        static class LevelSelectionFlowCoordinator_DidActivate
        {
            //https://github.com/kinsi55/BeatSaber_BetterSongList/blob/master/HarmonyPatches/RestoreLevelSelection.cs
            static readonly ConstructorInfo thingy = AccessTools.FirstConstructor(typeof(LevelSelectionFlowCoordinator.State), x => x.GetParameters().Length == 4);
          
            static void Prefix(ref LevelSelectionFlowCoordinator.State ____startState, bool addedToHierarchy)
            {
                if (!addedToHierarchy || !PackPromoButtonWasPressed.buttonWasPressed)
                    return;
                PackPromoButtonWasPressed.buttonWasPressed = false;
                IBeatmapLevelPack? pack = SongCore.Loader.CustomBeatmapLevelPackCollectionSO.beatmapLevelPacks.FirstOrDefault(packs => packs.packName == "BSMG's Fifth Anniversary Music Pack");
                if (pack != null) ____startState = (LevelSelectionFlowCoordinator.State)thingy.Invoke(new object[] { LevelCategory.CustomSongs, pack, null, null });
            }
        }


        [HarmonyPatch(typeof(MainMenuViewController))]
        [HarmonyPatch("PackPromoButtonWasPressed")]
        internal class PackPromoButtonWasPressed
        {
            public static bool buttonWasPressed = false;
            static bool Prefix(MainMenuViewController __instance, ref DlcPromoPanelDataSO.MusicPackPromoInfo ____musicPackPromoBanner)
            {
                if (!SongCore.Loader.AreSongsLoaded) {
                    return false;
                }
                buttonWasPressed = true;
                __instance.GetField<Action<IBeatmapLevelPack, IPreviewBeatmapLevel>, MainMenuViewController>("musicPackPromoButtonWasPressedEvent")(null, null);
                return false;
            }
        }


        [HarmonyPatch(typeof(MainMenuViewController))]
        [HarmonyPatch("DidActivate")]
        internal class DidActivate
        {
            public static bool buttonWasPressed = false;
            static void Postfix(MainMenuViewController __instance)
            {
                if(SongDownloader.bar == null)
                {
                    SongDownloader.bar = DownloadingText.Create();
                }
            }
        }


        [HarmonyPatch(typeof(DlcPromoPanelModel))]
        [HarmonyPatch("GetPackDataForMainMenuPromoBanner")]
        internal class DlcPromoPanelModelPack
        {
            static bool Prefix(DlcPromoPanelModel __instance, ref DlcPromoPanelDataSO.MusicPackPromoInfo __result, ref bool owned)
            {

                

                owned = false;

                DlcPromoPanelDataSO _dlcPromoPanelData = __instance.GetField<DlcPromoPanelDataSO, DlcPromoPanelModel>("_dlcPromoPanelData");
                PlayerDataModel _playerDataModel = __instance.GetField<PlayerDataModel, DlcPromoPanelModel>("_playerDataModel");
                string packID = _dlcPromoPanelData.defaultMusicPackPromo.previewBeatmapLevelPack.packID;
                if (_playerDataModel.playerData.currentDlcPromoId != packID)
                {
                    _playerDataModel.playerData.SetNewDlcPromo(packID);
                }
                DlcPromoPanelDataSO.MusicPackPromoInfo pack = new DlcPromoPanelDataSO.MusicPackPromoInfo();
                pack.SetField<DlcPromoPanelDataSO.MusicPackPromoInfo, Sprite>("_bannerImage", SongCore.Utilities.Utils.LoadSpriteFromResources("FifthAnniversary.Assets.PromoImage.png"));
                pack.SetField<DlcPromoPanelDataSO.MusicPackPromoInfo, PreviewBeatmapLevelPackSO>("_beatmapLevelPack", (PreviewBeatmapLevelPackSO)_dlcPromoPanelData.defaultMusicPackPromo.previewBeatmapLevelPack);
                __result = pack;
                return false;
            }
        }

      


    }
}
