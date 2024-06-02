using UnityEngine.SceneManagement;
using UnityEngine;

namespace PubScale.SdkOne.NativeAds.Sample
{
public class Scene3DExit : MonoBehaviour
{
    public GameObject SceneObjs;
    public GameObject RunnerBoy;

        bool isClicked = false;


        /// <summary>
        /// Loads the scene according to the scene name provided
        /// </summary>
        public void LoadScene(string sceneName)
        {
            if(isClicked == false)
            {
                RunnerBoy.SetActive(false);
                SceneObjs.SetActive(false);
                isClicked = true;
                Time.timeScale = 1;
        
                 SceneManager.LoadScene(sceneName);
            }

        }


        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainScene");
        }

}
}