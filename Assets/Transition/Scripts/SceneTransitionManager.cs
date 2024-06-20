using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTransitionManager : SingletonMonoBehaviour<SceneTransitionManager>
{
	public static NovelId CurrentNovelId { get; private set; }
	public static RhythmId CurrentRhythmId { get; private set; }
	public static bool IsVs { get; private set; }

	public static void TransitionToNovel(NovelId novelId)
	{
		if(novelId == NovelId.None) return;

		CurrentNovelId = novelId;

		TransitionToScene(SceneName.Novel);
	}

	public static void TransitionToRhythm(RhythmId rhythmId, bool isVs = false)
	{
		if(rhythmId == RhythmId.None) return;
	
		CurrentRhythmId = rhythmId;
		IsVs = isVs;
		
		TransitionToScene(SceneName.Rhythm);
	}

	// TODO: 各シーンへの遷移関数を定義したらprivateに
	public static void TransitionToScene(SceneName sceneName)
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

		CanvasGroup canvasGroup = GameObject.FindWithTag("TransitionOverlay").GetComponent<CanvasGroup>();

		yield return canvasGroup.DOFade(endValue: 1f, duration: 0.5f).WaitForCompletion();

		AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(prevScene);
		yield return new WaitUntil(() => unloadOp.isDone);

		AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Additive);
		yield return new WaitUntil(() => loadOp.isDone);

		yield return canvasGroup.DOFade(endValue: 0f, duration: 0.5f).WaitForCompletion();
	}
}
