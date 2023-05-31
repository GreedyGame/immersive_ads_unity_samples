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

		public static event Action<Transform> ShowAdNow;
		public static event Action HideAdNow;
		#endregion

		#region Intialisation
		private void Awake()
		{
			Application.targetFrameRate = 60;
#if UNITY_WEBGL && !UNITY_EDITOR
		WebUtils.LogEvent("GameStart" + Application.productName);
#endif
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
		private void PlayerController_OnFloor(int move = -1)
		{
			currentScore = currentScore + 1;
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
					ShowAdNow?.Invoke(player.transform);
					cameraFollow.LockCam(true, Vector3.zero);
				}
				else
				{
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
#if UNITY_WEBGL && !UNITY_EDITOR
		WebUtils.LogEvent("ContinueUsed_" + Application.productName);
#endif
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
			ui.ContinueButton(false);
			ContinueGame();
		}
	}
}
