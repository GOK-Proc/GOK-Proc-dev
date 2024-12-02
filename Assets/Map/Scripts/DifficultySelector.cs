using DG.Tweening;
using Rhythm;
using Settings;
using Transition;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map
{
	class DifficultySelector : MonoBehaviour
	{
		[SerializeField] private UserSettings _settings;

		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private MapSpot _firstMapSpot;
		[SerializeField] private DifficultyBox _firstDifficultyBox;

		private void Start()
		{
			if(_settings.ScenarioDifficulty == ScenarioDifficulty.None)
			{
				OpenDifficultySelectDIalog();
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(_firstMapSpot.gameObject);
			}
		}

		public void OpenDifficultySelectDIalog()
		{
			EventSystem.current.SetSelectedGameObject(_firstDifficultyBox.gameObject);
			_canvasGroup.DOFade(1f, 0.5f);
		}

		public void SetMapDifficulty(ScenarioDifficulty difficulty)
		{
			_settings.ScenarioDifficulty = difficulty;
			_settings.Save();

			_canvasGroup.DOFade(0f, 0.5f).OnComplete(() => EventSystem.current.SetSelectedGameObject(_firstMapSpot.gameObject));
		}

	}
}