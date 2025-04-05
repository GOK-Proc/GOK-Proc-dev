using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Map;
using Rhythm;
using KanKikuchi.AudioManager;

namespace Transition
{
	public class SceneTransitionManager : SingletonMonoBehaviour<SceneTransitionManager>
	{
		public static EpisodeType CurrentEpisodeType { get; private set; }
		public static NovelId CurrentNovelId { get; private set; }
		public static RhythmId CurrentRhythmId { get; private set; }
		public static Difficulty CurrentDifficulty { get; private set; }
		public static bool CurrentIsVs { get; private set; }
		public static TutorialId CurrentTutorialId { get; private set; }
		public static SceneName RecentSceneName { get; private set; }

		private static CanvasGroup _overlay;

		private static Scene _prevScene;

		private static EpisodeFlagManager _episodeFlagManager;
		private static SettingsManager _settingsManager;

		private void OnEnable()
		{
			_episodeFlagManager = GameObject.FindWithTag("EpisodeFlagManager").GetComponent<EpisodeFlagManager>();
			_settingsManager = GameObject.FindWithTag("SettingsManager").GetComponent<SettingsManager>();

			_overlay = GameObject.FindWithTag("TransitionOverlay").GetComponent<CanvasGroup>();

			SceneManager.LoadScene(SceneName.Title.ToString(), LoadSceneMode.Additive);
			_prevScene = SceneManager.GetSceneByName(SceneName.Title.ToString());
		}

		public static void TransitionToTitle()
		{
			CurrentEpisodeType = EpisodeType.None;
			CurrentNovelId = NovelId.None;
			CurrentRhythmId = RhythmId.None;

			TransitionToScene(SceneName.Title);
		}

		public static void TransitionToSettings()
		{
			TransitionToScene(SceneName.Settings);
		}

		public static void TransitionToGallery()
		{
			TransitionToScene(SceneName.Gallery);
		}

		public static void TransitionToMap()
		{
			_settingsManager.SetDefaultSeVolume();
		
			TransitionToScene(SceneName.Map);
		}

		public static void TransitionToMap(bool result)
		{
			if (result)
			{
				switch (CurrentEpisodeType)
				{
					case EpisodeType.Novel:
						_episodeFlagManager.SetNextFlag(CurrentNovelId);
						break;
					case EpisodeType.Rhythm:
						_episodeFlagManager.SetNextFlag(CurrentRhythmId);
						break;
					default:
						return;
				}
			}

			TransitionToScene(SceneName.Map);
		}

		public static void TransitionToModeSelection()
		{
			CurrentEpisodeType = EpisodeType.None;
			CurrentNovelId = NovelId.None;
			CurrentRhythmId = RhythmId.None;
		
			TransitionToScene(SceneName.ModeSelection);
		}

		public static void TransitionToMusicSelection()
		{
			TransitionToScene(SceneName.MusicSelection);
		}

		public static void TransitionToNovel(NovelId novelId)
		{
			if (novelId == NovelId.None) return;

			_settingsManager.SetNovelSeVolume();

			CurrentEpisodeType = EpisodeType.Novel;
			CurrentNovelId = novelId;

			TransitionToScene(SceneName.Novel);
		}

		public static void TransitionToRhythm(RhythmId rhythmId, Difficulty difficulty, bool isVs = false)
		{
			if (rhythmId == RhythmId.None) return;

			CurrentEpisodeType = EpisodeType.Rhythm;
			CurrentRhythmId = rhythmId;
			CurrentDifficulty = difficulty;
			CurrentIsVs = isVs;
			CurrentTutorialId = TutorialId.None;

			TransitionToScene(SceneName.Rhythm);
		}

        public static void TransitionToBattleTutorial(RhythmId rhythmId, Difficulty difficulty)
        {
            if (rhythmId == RhythmId.None) return;

            CurrentEpisodeType = EpisodeType.Rhythm;
            CurrentRhythmId = rhythmId;
            CurrentDifficulty = difficulty;
            CurrentIsVs = true;
			CurrentTutorialId = TutorialId.Battle;

            TransitionToScene(SceneName.Rhythm);
        }

        public static void TransitionToRhythmTutorial()
        {
            CurrentEpisodeType = EpisodeType.Rhythm;
            CurrentRhythmId = RhythmId.Chapter1_2;
            CurrentIsVs = true;
            CurrentTutorialId = TutorialId.Rhythm;

            TransitionToScene(SceneName.Rhythm);
        }

		public static void TransitionToAdjustment()
		{
			TransitionToScene(SceneName.Adjustment);
		}

		public static void TransitionToCredit()
		{
			_settingsManager.SetDefaultSeVolume();

			TransitionToScene(SceneName.Credit);
		}

        private static void TransitionToScene(SceneName sceneName)
		{
			Instance.StartCoroutine(TransitionToSceneCoroutine(sceneName));
		}

		private static IEnumerator TransitionToSceneCoroutine(SceneName sceneName)
		{
			BGMManager.Instance.FadeOut(duration: 0.5f);

			yield return _overlay.DOFade(endValue: 1f, duration: 0.5f).WaitForCompletion();

			RecentSceneName = (SceneName)Enum.Parse(typeof(SceneName), _prevScene.name);
			
			AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_prevScene);
			yield return new WaitUntil(() => unloadOp.isDone);

			AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
			yield return new WaitUntil(() => loadOp.isDone);

			_prevScene = SceneManager.GetSceneByName(sceneName.ToString());

			yield return _overlay.DOFade(endValue: 0f, duration: 0.5f).WaitForCompletion();
		}
	}
}