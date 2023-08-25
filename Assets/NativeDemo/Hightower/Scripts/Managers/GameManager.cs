using System;
using System.Collections;
using UnityEngine;
namespace PubScale.SdkOne.NativeAds.Hightower
{
	public enum GameState { Running, Paused }
	public class GameManager : MonoBehaviour
	{

		#region Decalaration
		public static GameManager instance;

		[Header("Game Settings")]
		[Space]
		public GameSettings gameSettings;
		[Header("Script References")]
		[Space]
		[SerializeField] private PlayerController player;
		[SerializeField] private CameraController cameraFollow;
		[SerializeField] private LevelGenerator levelManager;
		[SerializeField] private UIManager ui;
		private int currentScore;
		private int bestScore;
		private int NextStartScore = 5;
		private int NextEndScore = 5;
		private int previousScore;
		private int previousMove;
        private bool canShowBallonAd;

        public static event Action<Transform> ShowAdNow;
		public static event Action HideAdNow;

        public static event Action<bool,Transform> DroneState;
        #endregion

        #region Intialisation
        private void Awake()
		{
			Application.targetFrameRate = 60;
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy(gameObject);

			bestScore = PlayerPrefs.GetInt("BestScore", 0);
			Input.multiTouchEnabled = false;
			PlayerController.OnFloor += PlayerController_OnFloor;
			CalculateNextScore();
			previousScore = 0;

		}
		void CalculateNextScore()
		{
			NextStartScore = UnityEngine.Random.Range(currentScore + 3, currentScore + 10);
			NextEndScore = UnityEngine.Random.Range(NextStartScore + 11, NextStartScore + 16);
		}
		private void PlayerController_OnFloor(FloorHandler floor)
		{
			int move=floor.GetXStatus();
			currentScore = currentScore + 1;
            if (currentScore == 4)
                canShowBallonAd = true;
            if (canShowBallonAd && currentScore % 3 == 0)
            {
                canShowBallonAd = false;
                DroneState?.Invoke(true,floor.transform);
            }
            int min = currentScore - previousScore;
			
            previousScore = currentScore;
			if (currentScore > 20)
				levelManager.levelSettings.canHaveObstacle = true;
			if (currentScore == NextStartScore)
			{
				levelManager.SpawnHorizontal(true);
			}
			else if (currentScore == NextEndScore)
			{
				levelManager.SpawnHorizontal(false);
				CalculateNextScore();
			}
	
			if (move >= 0 && previousMove != move)
			{
				previousMove = move;
				if (move == 1)
				{
                    DroneState?.Invoke(false,floor.transform);
                    ShowAdNow?.Invoke(player.transform);
					cameraFollow.LockCam(true, Vector3.zero);
				}
				else
				{
					canShowBallonAd = true;
					HideAdNow?.Invoke();
					cameraFollow.LockCam(false, levelManager.GetPos());
				}
			}
            ui.UpdateScore(currentScore);
		}

		private void Start()
		{
			InitGame();

		}
		void InitGame()
		{
			PoolingSystem.instance.InitPools(gameSettings.ObjectPools);
			AudioManager.instance.InitAudio(gameSettings.gameClips, gameSettings.bgMusic);
			ui.InitUI(0);
			levelManager.InitGenerator(player.transform);
			cameraFollow.InitCam(player.transform);
		}
		#endregion

		#region Events
		public void StartGame()
		{
			currentScore = 0;
			ui.StartLevel();
			player.InitPlayer();
		
			ui.UpdateScore(currentScore);
		}
		public void LevelComplete(float delay = 0)
		{
			AudioManager.instance.Play("gameComplete");
			bool best = false;

			if (currentScore > bestScore)
			{
				best = true;
				bestScore = currentScore;
				LogScores(bestScore, 50, 100);
				PlayerPrefs.SetInt("BestScore", currentScore);
			}
			levelManager.StopGenerating();
			StartCoroutine(DelayDie(best, delay));
			cameraFollow.DisableBounds();
		}
		void LogScores(int bestScore, int range = 100, int Multiplier = 10)
		{
			string score = "";
			if (bestScore <= 0)
				return;

			for (int i = 0; i <= range * Multiplier; i += range)
			{
				if (bestScore >= i && bestScore < i + range)
				{
					score = i + "-" + (i + range);
					break;
				}
			}
			if (bestScore > range * Multiplier)
				score = (range * Multiplier) + "+";
		}
		IEnumerator DelayDie(bool best, float delay)
		{
			yield return new WaitForSeconds(delay);
			ui.ShowLevelComplete(bestScore, best);
		}
		public void ContinueGame()
		{
            ui.ShowHideAd?.Invoke(false);
            ui.HideLevelComplete();
			cameraFollow.ResetCam();
			player.Revive();
			levelManager.RestartGenerator();
			cameraFollow.DelayEnableBounds();
		}
		private void OnDestroy()
		{
			PlayerController.OnFloor -= PlayerController_OnFloor;
		}
		#endregion
		public void ShowReward()
		{
			ui.ShowHideAd?.Invoke(true);
			ui.ContinueButton(false);
			//AdManager.instance.GiveRewardedAd((and) =>
			//{
			//	if (and)
			//	{
                  
			//	}
			//});
                    ContinueGame();
		}
	}
}
