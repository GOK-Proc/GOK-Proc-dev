using DG.Tweening;
using Rhythm;
using TMPro;
using Transition;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Map
{
	[RequireComponent(typeof(Selectable))]
	public class EpisodeBox : MonoBehaviour, ISubmitHandler, ICancelHandler
	{
		[SerializeField] private TextMeshProUGUI _episodeNumberText;
		[SerializeField] private TextMeshProUGUI _titleText;

		[SerializeField] private GameObject _vsIcon;

		private EpisodeInfomation _info = new EpisodeInfomation();
		private bool _isTutorialEnabled = false;

		public void SetInfo(EpisodeInfomation info, bool isTutorialEnabled = false)
		{
			_info = info;
			_isTutorialEnabled = isTutorialEnabled;

			_episodeNumberText.text = $"Chapter {_info.Chapter}-{_info.Section}";
			_titleText.text = _info.Title;

			if (info.EpisodeType == EpisodeType.Rhythm)
			{
				_vsIcon.SetActive(true);
			}
		}

		public void OnSubmit(BaseEventData eventData)
		{
			switch (_info.EpisodeType)
			{
				case EpisodeType.Novel:
					SceneTransitionManager.TransitionToNovel(_info.NovelId);
					break;
				case EpisodeType.Rhythm:
					if (_info.IsTutorialNeeded && _isTutorialEnabled)
					{
						SceneTransitionManager.TransitionToBattleTutorial(_info.RhythmId, DifficultySelector.MapDifficulty);
					}
					else
					{
						SceneTransitionManager.TransitionToRhythm(_info.RhythmId, DifficultySelector.MapDifficulty, true);
					}
					break;
			}
		}

		public void OnCancel(BaseEventData eventData)
		{
			transform.parent.GetComponent<CanvasGroup>().DOFade(endValue: 0f, duration: 0.5f);
			EventSystem.current.SetSelectedGameObject(MapSpot.CurrentMapSpot);
		}
	}
}