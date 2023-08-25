using TMPro;
using UnityEngine;
namespace PubScale.SdkOne.NativeAds.Sample
{
    /// <summary>
    /// Class to Logs message to the log ui console.
    /// </summary>
    public class Logger : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI logTxt;
        [SerializeField] private Canvas UICanvas;
        private static Logger instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
                PubScaleManager.LogHandler += NativeAdManager_LogHandler; //Subcribe to Native ad manager log event
            }
            else
                Destroy(this.gameObject);
        }
        private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
        {
            UICanvas.worldCamera = Camera.main;
        }

        private void OnDestroy()
        {
            PubScaleManager.LogHandler -= NativeAdManager_LogHandler; //Unsubcribe to Native ad manager log event
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }
        private void NativeAdManager_LogHandler(string obj,PubScaleLogData logData)
        {
            string log = "";
            foreach (var item in logData.EventData)
            {
                log += item.k + "=" + item.v + "\n";
            }
            log = obj + "\n" + log;
            LogMessage(log);
        }
        /// <summary>
        /// Logs message to the log ui console.
        /// </summary>
        public static void Log(string msg)
        {
            Debug.Log(msg);
            if (instance == null)
                return;
            instance.LogMessage(msg);
        }
        public void LogMessage(string msg)
        {
            logTxt.text += msg + "\n";
        }
    }
}
