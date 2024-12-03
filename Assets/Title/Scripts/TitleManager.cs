using UnityEngine;
using Transition;
using KanKikuchi.AudioManager;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Settings;

namespace Title
{
	public class TitleManager : MonoBehaviour
	{
		[SerializeField] private UserSettings _userSettings;

		[SerializeField] private CanvasGroup _terminateCanvasGroup;
		[SerializeField] private Selectable _yesButton;
		[SerializeField] private EventTrigger _eventTrigger;

		private void Start()
		{
			BGMManager.Instance.ChangeBaseVolume(_userSettings.BgmVolume / 10f);
			SEManager.Instance.ChangeBaseVolume(_userSettings.SoundEffectVolume / 10f);
			switch (_userSettings.ScreenMode)
			{
				case ScreenMode.Windowed:
					Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, FullScreenMode.Windowed);
					break;
				case ScreenMode.FullScreen:
					Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
					break;
			}
			switch (_userSettings.FrameRate)
			{
				case FrameRate.VSync:
					QualitySettings.vSyncCount = 1;
					Application.targetFrameRate = -1;
					break;
				case FrameRate.Fps30:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 30;
					break;
				case FrameRate.Fps60:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 60;
					break;
				case FrameRate.Fps120:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 120;
					break;
				case FrameRate.Fps144:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 144;
					break;
				case FrameRate.Fps240:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = 240;
					break;
				case FrameRate.Unlimited:
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = -1;
					break;
			}

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