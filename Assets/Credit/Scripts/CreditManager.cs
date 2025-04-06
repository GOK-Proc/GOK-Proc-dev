using DG.Tweening;
using Settings;
using Transition;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

namespace Credit
{
	public class CreditManager : MonoBehaviour
	{
		[SerializeField] private UserSettings _settings;
		[SerializeField] private VideoPlayer _player;
		[SerializeField] private EventTrigger _trigger;
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private CustomButton _cancelButton;

		private void Awake()
		{
			_player.SetDirectAudioVolume(0, _settings.BgmVolume / 10f);
		}

		private void Start()
		{
			_player.targetTexture.Release();
			_player.loopPointReached += (vp) => SceneTransitionManager.TransitionToTitle();
		}

		public void OpenSkipDialog()
		{
			_player.Pause();
			EventSystem.current.SetSelectedGameObject(_cancelButton.gameObject);
			_canvasGroup.DOFade(1f, 0.5f);
		}

		public void OnSkipButtonClick()
		{
			SceneTransitionManager.TransitionToTitle();
		}

		public void OnCancelButtonClick()
		{
			EventSystem.current.SetSelectedGameObject(_trigger.gameObject);
			_canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
			{
				_player.Play();
			});
		}
	}
}