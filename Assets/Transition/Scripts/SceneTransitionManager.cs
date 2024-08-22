using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Map;
using Rhythm;

namespace Transition
{
	public class SceneTransitionManager : SingletonMonoBehaviour<SceneTransitionManager>
	{
		public static EpisodeType CurrentEpisodedType { get; private set; }
		public static NovelId CurrentNovelId { get; private set; }
		public static RhythmId CurrentRhythmId { get; private set; }
		public static Difficulty CurrentDifficulty { get; private set; }
		public static bool CurrentIsVs { get; private set; }

		private static CanvasGroup _overlay;

		private void OnEnable()
		{
			_overlay = GameObject.FindWithTag("TransitionOverlay").GetComponent<CanvasGroup>();

			SceneManager.LoadScene(SceneName.Title.ToString(), LoadSceneMode.Additive);
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
			EpisodeFlagManager episodeFlagManager = GameObject.FindWithTag("EpisodeFlagManager").GetComponent<EpisodeFlagManager>();
		
			switch(CurrentEpisodedType)
			{
				case EpisodeType.Novel:
					episodeFlagManager.SetFlag(CurrentNovelId, result);
					break;
				case EpisodeType.Rhythm:
					episodeFlagManager.SetFlag(CurrentRhythmId, result);
					break;
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

			CurrentEpisodedType = EpisodeType.Novel;
			CurrentNovelId = novelId;

			TransitionToScene(SceneName.Novel);
		}

		public static void TransitionToRhythm(RhythmId rhythmId, Difficulty difficulty, bool isVs = false)
		{
			if (rhythmId == RhythmId.None) return;

			CurrentEpisodedType = EpisodeType.Rhythm;
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
			Scene prevScene = default;
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				if (SceneManager.GetSceneAt(i).name != "Transition")
				{
					prevScene = SceneManager.GetSceneAt(i);
					break;
				}
			}

			yield return _overlay.DOFade(endValue: 1f, duration: 0.5f).WaitForCompletion();

			AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(prevScene);
			yield return new WaitUntil(() => unloadOp.isDone);

			AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
			yield return new WaitUntil(() => loadOp.isDone);

			yield return _overlay.DOFade(endValue: 0f, duration: 0.5f).WaitForCompletion();
		}
	}
}