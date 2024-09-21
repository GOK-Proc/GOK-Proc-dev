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
		public static SceneName PreviousSceneName { get; private set; }

		private static CanvasGroup _overlay;

		private static Scene _prevScene;

		private void OnEnable()
		{
			_overlay = GameObject.FindWithTag("TransitionOverlay").GetComponent<CanvasGroup>();

			SceneManager.LoadScene(SceneName.Title.ToString(), LoadSceneMode.Additive);
			_prevScene = SceneManager.GetSceneByName(SceneName.Title.ToString());
		}

		public static void TransitionToGallery()
		{
			TransitionToScene(SceneName.Gallery);
		}

		public static void TransitionToMap()
		{
			TransitionToScene(SceneName.Map);
		}

		public static void TransitionToMap(bool result)
		{
			if (result)
			{
				EpisodeFlagManager episodeFlagManager = GameObject.FindWithTag("EpisodeFlagManager").GetComponent<EpisodeFlagManager>();

				switch (CurrentEpisodeType)
				{
					case EpisodeType.Novel:
						episodeFlagManager.SetNextFlag(CurrentNovelId);
						break;
					case EpisodeType.Rhythm:
						episodeFlagManager.SetNextFlag(CurrentRhythmId);
						break;
				}
			}

			TransitionToScene(SceneName.Map);
		}

		public static void TransitionToModeSelection()
		{
			TransitionToScene(SceneName.ModeSelection);
		}

		public static void TransitionToMusicSelection()
		{
			TransitionToScene(SceneName.MusicSelection);
		}

		public static void TransitionToNovel(NovelId novelId)
		{
			if (novelId == NovelId.None) return;

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

			TransitionToScene(SceneName.Rhythm);
		}

		private static void TransitionToScene(SceneName sceneName)
		{
			Instance.StartCoroutine(TransitionToSceneCoroutine(sceneName));
		}

		private static IEnumerator TransitionToSceneCoroutine(SceneName sceneName)
		{
			BGMManager.Instance.FadeOut(duration: 0.5f);

			yield return _overlay.DOFade(endValue: 1f, duration: 0.5f).WaitForCompletion();

			PreviousSceneName = (SceneName)Enum.Parse(typeof(SceneName), _prevScene.name);
			
			AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_prevScene);
			yield return new WaitUntil(() => unloadOp.isDone);

			AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
			yield return new WaitUntil(() => loadOp.isDone);

			yield return _overlay.DOFade(endValue: 0f, duration: 0.5f).WaitForCompletion();

			_prevScene = SceneManager.GetSceneByName(sceneName.ToString());
		}
	}
}