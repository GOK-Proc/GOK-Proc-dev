using DG.Tweening;
using Rhythm;
using Settings;
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
		[SerializeField] private UserSettings _settings;

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
					Difficulty difficulty;
					switch (_settings.ScenarioDifficulty)
					{
						case ScenarioDifficulty.Easy:
							difficulty = Difficulty.Easy;
							break;
						case ScenarioDifficulty.Hard:
							difficulty = Difficulty.Hard;
							break;
						case ScenarioDifficulty.Expert:
							difficulty = Difficulty.Expert;
							break;
						default:
							difficulty = Difficulty.Easy;
							break;
					}

					if (_info.IsTutorialNeeded && _isTutorialEnabled)
					{
						SceneTransitionManager.TransitionToBattleTutorial(_info.RhythmId, difficulty);
					}
					else
					{
						SceneTransitionManager.TransitionToRhythm(_info.RhythmId, difficulty, true);
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