using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Settings;
using UnityEngine;
using Zenject;
using Colour = UnityEngine.Color;

namespace FifthAnniversary.UI
{
	public class DownloadSettings : MonoBehaviour, IInitializable, INotifyPropertyChanged
	{
		private Config? config;
		private GameplaySetupViewController? gameplaySetupViewController;
		public event PropertyChangedEventHandler PropertyChanged = null!;

        [UIAction("download")]
        private void DownloadButtonClicked()
        {
            Debug.Log("a");
            Task.Run(() =>
            {
                SongDownloader.DownloadSongs("https://beatsaver.com/api/playlists/id/89418/0");
            });
        }

        [UIAction("extract")]
        private void ExtractButtonClicked()
        {
            AssetExtractor.ExtractAssets();
        }

        public void Initialize()
		{
            BSMLSettings.instance.AddSettingsMenu("Fifth Anniversary", "FifthAnniversary.UI.DownloadSettings.bsml", this);
		}
	}
}
