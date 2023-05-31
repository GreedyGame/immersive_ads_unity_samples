using UnityEngine.SceneManagement;
using UnityEngine;

namespace PubScale.SdkOne.NativeAds.Sample
{
    /// <summary>
    /// Class to load scene on button press
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// Loads the scene according to the scene name provided
        /// </summary>
        public void LoadScene(string sceneName)
        {
            Time.timeScale = 1;
            Fader.LoadScene(sceneName);

        }
    }
}
