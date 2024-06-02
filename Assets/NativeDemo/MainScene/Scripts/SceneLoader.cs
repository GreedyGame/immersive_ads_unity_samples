using UnityEngine.SceneManagement;
using UnityEngine;

namespace PubScale.SdkOne.NativeAds.Sample
{
    /// <summary>
    /// Class to load scene on button press
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {

        bool isClicked = false;


        /// <summary>
        /// Loads the scene according to the scene name provided
        /// </summary>
        public void LoadScene(string sceneName)
        {
            if (isClicked == false)
            {
                isClicked = true;
                Time.timeScale = 1;

                if (Fader.instance != null)
                    Fader.LoadScene(sceneName);
                else
                    SceneManager.LoadScene(name);
            }

        }


        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
