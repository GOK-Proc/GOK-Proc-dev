using UnityEngine;
using Transition;
using KanKikuchi.AudioManager;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Title
{
	public class TitleManager : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _terminateCanvasGroup;
		[SerializeField] private Selectable _yesButton;
		[SerializeField] private EventTrigger _eventTrigger;

		private void Start()
		{
			BGMManager.Instance.Play(BGMPath.MAIN_THEME);
		}

		public void ToModeSelection()
		{
			SceneTransitionManager.TransitionToModeSelection();
		}

		public void OpenTerminateDialog()
		{
			EventSystem.current.SetSelectedGameObject(_yesButton.gameObject);
			_terminateCanvasGroup.DOFade(1f, 0.5f);
		}

		public void CloseTerminateDialog()
		{
			EventSystem.current.SetSelectedGameObject(_eventTrigger.gameObject);
			_terminateCanvasGroup.DOFade(0f, 0.5f);
		}

		public void TerminateApplication()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}