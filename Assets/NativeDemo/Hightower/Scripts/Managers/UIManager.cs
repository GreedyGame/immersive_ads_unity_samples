using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.Collections;
using PubScale.SdkOne;

namespace PubScale.SdkOne.NativeAds.Hightower
{
    public class UIManager : MonoBehaviour
    {
        #region Decalaration
        public static event Action<bool> GamePaused;
        public TextMeshProUGUI timerTxt;
        [SerializeField] private CanvasGroup gameCanvas;
        [SerializeField] private CanvasGroup gameOverUI;
        [SerializeField] private CanvasGroup pauseUI;
        [SerializeField] private CanvasGroup tutorialUI;
        [SerializeField] private CanvasGroup countdownUI;
        [SerializeField] private Button pauseBtn;
        [SerializeField] private TextMeshProUGUI scoreTxt;
        [SerializeField] private TextMeshProUGUI bestScoreTxt;
        [SerializeField] private TextMeshProUGUI countDownTxt;
        [SerializeField] private TextMeshProUGUI gameOvertotalScoreTxt;
        [SerializeField] private TextMeshProUGUI gameOverbestScoreTxt;
        [SerializeField] private GameObject newBest;
        [SerializeField] private GameObject continueButton;
        [SerializeField] private Image audioToggle;
        [SerializeField] private Sprite mute, umute;
        [SerializeField] private NativeAdHolder ads;
        private Coroutine countDownRoutine;
        private bool pauseState = false;
        private bool canMute = false;
        #endregion

        #region Intialisation
        private void Awake()
        {
            bestScoreTxt.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
            gameCanvas.gameObject.SetActive(false);
            AudioListener.volume = PlayerPrefs.GetInt("mute", 0) == 1 ? 0 : 1;
            AudioManager.isMute = PlayerPrefs.GetInt("mute", 0) == 1;
            audioToggle.sprite = AudioListener.volume == 0 ? mute : umute;
            canMute = true;
        }
        public void InitUI(int score)
        {
            tutorialUI.blocksRaycasts = true;
            tutorialUI.gameObject.SetActive(true);
            tutorialUI.DOFade(1, 1).From(0);
            gameCanvas.gameObject.SetActive(false);
            scoreTxt.text = score.ToString();
        }
        #endregion
        #region Events
        public void StartLevel()
        {
            tutorialUI.blocksRaycasts = false;
            tutorialUI.DOFade(0, 1).From(1).OnComplete(() => { tutorialUI.gameObject.SetActive(false); });
            gameCanvas.gameObject.SetActive(true);
            gameCanvas.DOFade(1, 1).From(0);
        }
        public void UpdateScore(int score)
        {
            scoreTxt.text = score.ToString();
        }
        public void RefreshAd()
        {
            ads.FetchAd();
        }
        public void MuteToggle()
        {
            if (!canMute)
                return;
            AudioListener.volume = 1 - AudioListener.volume;
            AudioManager.instance.Play("button");
            PlayerPrefs.SetInt("mute", AudioListener.volume == 0 ? 1 : 0);
            audioToggle.sprite = AudioListener.volume == 0 ? mute : umute;
        }
        public void RestartLevel()
        {
            AudioManager.instance.Play("button");
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
#if UNITY_WEBGL && !UNITY_EDITOR
        WebUtils.ShowInterstitialAd();
#endif
        }
        public void Pause()
        {
            AudioManager.instance.Play("button");
            pauseState = !pauseState;
            if (pauseState)
            {
                GamePaused?.Invoke(true);
                pauseUI.blocksRaycasts = true;
                pauseBtn.interactable = false;
                pauseUI.interactable = false;
                AudioManager.instance.Paused(true);
                Time.timeScale = 0;
                DGAnimationController.AnimateBgPanel(pauseUI.gameObject, true, Complete: () => { pauseUI.interactable = true; });
            }
            else
            {
                if (countDownRoutine == null)
                    countDownRoutine = StartCoroutine(CountDown());
                AudioManager.instance.Paused(false);

            }
        }
        IEnumerator CountDown()
        {
            pauseUI.blocksRaycasts = false;
            pauseUI.interactable = false;
            DGAnimationController.AnimateBgPanel(pauseUI.gameObject, false, Complete: () =>
            {
                pauseBtn.interactable = true;
            });
            countdownUI.gameObject.SetActive(true);
            countdownUI.DOFade(1, 0.3f).From(0).SetUpdate(true);
            int timer = 3;
            while (timer > 0)
            {
                countDownTxt.text = "Starts in\n" + timer;
                //print("Starts in" + timer);
                yield return new WaitForSecondsRealtime(1);
                timer--;
            }
            countdownUI.gameObject.SetActive(false);
            GamePaused?.Invoke(false);
            Time.timeScale = 1;
            countDownRoutine = null;
        }
        public void ShowLevelComplete(int best, bool newScore)
        {
            newBest.SetActive(newScore);
            gameOvertotalScoreTxt.text = scoreTxt.text;
            gameOverbestScoreTxt.text = "Best: " + best.ToString();
            DGAnimationController.AnimateBgPanel(gameOverUI.gameObject, true);
        }
        public void HideLevelComplete()
        {
            continueButton.SetActive(false);
            DGAnimationController.AnimateBgPanel(gameOverUI.gameObject, false);
        }
        public void ContinueButton(bool ans)
        {
            continueButton.gameObject.SetActive(ans);
        }
        #endregion

        #region UtilityFunctions
        private void OnApplicationPause(bool pause)
        {
            //if (pause)
            //    ForcePause();
        }
        private void OnApplicationFocus(bool focus)
        {
            //if (!focus)
            //    ForcePause();
        }
        public void ForcePause()
        {
            //if (countDownRoutine != null)
            //{
            //    countdownUI.gameObject.SetActive(false);
            //    pauseUI.blocksRaycasts = true;
            //    pauseBtn.interactable = false;
            //    pauseState = true;
            //    pauseUI.interactable = true;
            //    DGAnimationController.EnablePanel(pauseUI.gameObject, true);
            //    StopCoroutine(countDownRoutine);
            //    countDownRoutine = null;
            //}
            //if (Time.timeScale == 0 || gameOverUI.gameObject.activeInHierarchy || tutorialUI.blocksRaycasts)
            //    return;
            //pauseUI.blocksRaycasts = true;
            //pauseUI.interactable = true;
            //pauseBtn.interactable = false;
            //pauseState = true;
            //Time.timeScale = 0;
            //DGAnimationController.KillAnimation(pauseUI.gameObject);
            //DGAnimationController.EnablePanel(pauseUI.gameObject, true);
            //GamePaused?.Invoke(true);
        }
        public void ForceUnPause()
        {
            //pauseUI.gameObject.SetActive(false);
            //if (countDownRoutine == null)
            //{
            //    Time.timeScale = 0;
            //    StartCoroutine(CountDown());
            //}
        }
        #endregion
    }
}