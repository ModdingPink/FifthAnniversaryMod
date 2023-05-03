using Newtonsoft.Json.Linq;
using SongCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FifthAnniversary
{
    internal class SongDownloader
    {
        public static DownloadingText bar;
        static bool downloadInProgress = false;
        static bool barHasntExistedYet = true;

        public static async Task DownloadSongs(string URL)
        {
            if (downloadInProgress) return;
            downloadInProgress = true;
            if (bar != null) bar.enabled = true;
            try
            {
                if (bar != null) {
                    bar.progress = 0;
                    barHasntExistedYet = false;
                    bar.StartEvent();
                }

                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("BSMG/FifthAnniversary");
                var response = await client.GetAsync(URL);
                var content = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(content);

                List<string> list = new List<string>();
                float downloadNum = 1;
                
                foreach (var item in o["maps"])
                {
                    await BeatmapDownload(item["map"]["versions"][0]["downloadURL"].Value<string>(), item["map"]["id"].Value<string>());
                    if (bar != null)
                    {
                        if (barHasntExistedYet)
                        {
                            barHasntExistedYet = false;
                            bar.StartEvent();
                        }
                        bar.progress = (float)downloadNum / 10;
                        downloadNum++;
                    }
                }
                Config.Instance.itemsDownloaded = true;
                Loader.Instance.RefreshSongs(false);
            }
            catch
            {
                Config.Instance.itemsDownloaded = false;
            }
            if (bar != null) bar.EndEvent();
            downloadInProgress = false;
        }
        public static async Task BeatmapDownload(string URL, string songName)
        {
            using (WebClient webClient = new WebClient())
            {
                string zip = songName + ".zip";
                try
                {
                    var file = Path.Combine(AssetExtractor.LevelsPath, songName);
                    if (!Directory.Exists(file))
                    {
                        webClient.DownloadFile(URL, zip);
                        ZipFile.ExtractToDirectory(zip, file);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    System.IO.File.Delete(zip);
                }
            }
        }
    }

}
