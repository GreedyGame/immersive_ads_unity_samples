using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace PubScale.SdkOne.NativeAds.Sample
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Image faderImg;
        public static Fader instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            }
        }
        public static void LoadScene(string name)
        {
            if(instance != null)
            {
                instance.faderImg.gameObject.SetActive(true);
                instance.faderImg.transform.DOScale(4.5f, 0.5f).From(0).SetEase(Ease.InSine).SetUpdate(true).OnComplete(()=> {
                    SceneManager.LoadScene(name);
                });
           }
        }
        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if(faderImg != null)
            {
                faderImg.gameObject.SetActive(true);
                faderImg.transform.DOScale(0, 0.5f).From(4.5f).SetEase(Ease.OutFlash).SetDelay(0.5f).SetUpdate(true).OnComplete(() => {
                    faderImg.gameObject.SetActive(false);
                }); 
            }
        }
    }
}