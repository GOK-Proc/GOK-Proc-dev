using DG.Tweening;
using Rhythm;
using Transition;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map
{
	class DifficultySelector : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private MapSpot _firstMapSpot;
		[SerializeField] private DifficultyBox _firstDifficultyBox;

		static public Difficulty MapDifficulty { get; private set; }

		private void Start()
		{
			if(SceneTransitionManager.CurrentEpisodeType ==EpisodeType.None)
			{
				OpenDifficultySelectDIalog();
			}
		}

		public void OpenDifficultySelectDIalog()
		{
			EventSystem.current.SetSelectedGameObject(_firstDifficultyBox.gameObject);
			_canvasGroup.DOFade(1f, 0.5f);
		}

		public void SetMapDifficulty(Difficulty difficulty)
		{
			MapDifficulty = difficulty;
			_canvasGroup.DOFade(0f, 0.5f).OnComplete(() => EventSystem.current.SetSelectedGameObject(_firstMapSpot.gameObject));
		}

	}
}