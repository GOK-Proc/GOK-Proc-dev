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

		public void SetInfo(EpisodeInfomation info)
		{
			_info = info;

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
					//TODO: Difficultyは暫定 設定を参照する
					SceneTransitionManager.TransitionToRhythm(_info.RhythmId, Difficulty.Expert, true);
					break;
			}
		}

		public void OnCancel(BaseEventData eventData)
		{
			transform.parent.gameObject.SetActive(false);
			EventSystem.current.SetSelectedGameObject(MapSpot.CurrentMapSpot);
		}
	}
}