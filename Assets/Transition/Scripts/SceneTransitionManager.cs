using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTransitionManager : SingletonMonoBehaviour<SceneTransitionManager>
{

	public static Coroutine TransitionToScene(string sceneName)
	{
		return Instance.StartCoroutine(TransitionToSceneCoroutine(sceneName));
	}

	private static IEnumerator TransitionToSceneCoroutine(string sceneName)
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

		AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		yield return new WaitUntil(() => loadOp.isDone);

		yield return canvasGroup.DOFade(endValue: 0f, duration: 0.5f).WaitForCompletion();
	}
}
