using UnityEngine;

namespace Assets.Scripts.SaveLoadSystem
{
    public class LocalStorage
    {
        private const string PROGRESS_KEY = "Progress";
        public static void SaveProgress(ProgressAsset progress)
        {
            string progressJson = JsonUtility.ToJson(progress);
            PlayerPrefs.SetString(PROGRESS_KEY, progressJson);
            PlayerPrefs.Save();
        }

        public static ProgressAsset GetProgress()
        {
            string progressJson = PlayerPrefs.GetString(PROGRESS_KEY);
            if (progressJson == null)
                return null;
            var progress = JsonUtility.FromJson<ProgressAsset>(progressJson);
            return progress;
        }
    }
}
