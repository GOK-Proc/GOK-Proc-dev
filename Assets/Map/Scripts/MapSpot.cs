using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Transition;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Map
{
	[RequireComponent(typeof(Selectable))]
	public class MapSpot : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, ICancelHandler
	{
		[SerializeField] private EpisodeData _episodeData;
		[SerializeField] private EpisodeFlags _episodeFlags;

		[SerializeField] private EpisodeBox _episodeBox;

		[SerializeField] private TextMeshProUGUI _labelText;

		[SerializeField] private GameObject _selectOff;
		[SerializeField] private GameObject _selectOn;

		[SerializeField] private CanvasGroup _episodeBoxArea;

		[SerializeField] private string _label;
		[SerializeField] private List<EpisodeNumber> _episodes;

		public static GameObject CurrentMapSpot { get; private set; }

		private GameObject _camera;

		private void Start()
		{
			var dataDict = _episodeData.DataDict;
			var flagDict = _episodeFlags.FlagDict;

			if (!_episodes.Any(x => flagDict.TryGetValue((x.Chapter, x.Section), out bool flag) && flag))
			{
				gameObject.SetActive(false);
				return;
			}

			_labelText.text = _label;

			GameObject firstSelectedObj = null;
			Selectable preSelectable = null;
			foreach (var episode in _episodes)
			{
				if (flagDict.TryGetValue((episode.Chapter, episode.Section), out bool flag) && flag)
				{
					var info = dataDict[(episode.Chapter, episode.Section)];
					var obj = Instantiate(_episodeBox, _episodeBoxArea.transform);

					var index = _episodeFlags.FlagList.FindIndex(x => x.Key == (episode.Chapter, episode.Section));
					if (index >= 0 && index < _episodeFlags.FlagList.Count - 1 && !_episodeFlags.FlagList[index + 1].Value)
					{
						obj.SetInfo(info, true);
					}
					else
					{
						obj.SetInfo(info);
					}

					var selectable = obj.GetComponent<Selectable>();
					if (info.EpisodeType == SceneTransitionManager.CurrentEpisodeType)
					{
						switch (info.EpisodeType)
						{
							case EpisodeType.Novel:
								if (info.NovelId == SceneTransitionManager.CurrentNovelId)
								{
									firstSelectedObj = obj.gameObject;
								}
								break;
							case EpisodeType.Rhythm:
								if (info.RhythmId == SceneTransitionManager.CurrentRhythmId)
								{
									firstSelectedObj = obj.gameObject;
								}
								break;
						}
					}

					if (preSelectable != null)
					{
						Navigation preNavigation = preSelectable.navigation;
						preNavigation.selectOnDown = selectable;
						preSelectable.navigation = preNavigation;

						Navigation navigation = selectable.navigation;
						navigation.selectOnUp = preSelectable;
						selectable.navigation = navigation;
					}

					preSelectable = selectable;
				}
			}

			_camera = GameObject.FindWithTag("MainCamera");

			if (firstSelectedObj != null)
			{
				var pos = new Vector3(Math.Max(transform.position.x, -4), transform.position.y, _camera.transform.position.z);
				_camera.transform.position = pos;

				_episodeBoxArea.alpha = 1;

				CurrentMapSpot = gameObject;

				EventSystem.current.SetSelectedGameObject(firstSelectedObj);
			}
		}

		public void OnSelect(BaseEventData eventData)
		{
			_selectOff.SetActive(false);
			_selectOn.SetActive(true);

			CurrentMapSpot = gameObject;

			var pos = new Vector3(Math.Max(transform.position.x, -4), transform.position.y, _camera.transform.position.z);
			_camera.transform.DOMove(pos, 0.8f).SetEase(Ease.OutCubic);
		}

		public void OnDeselect(BaseEventData eventData)
		{
			_selectOn.SetActive(false);
			_selectOff.SetActive(true);
		}

		public void OnSubmit(BaseEventData eventData)
		{
			_episodeBoxArea.DOFade(endValue: 1f, duration: 0.5f);

			EventSystem.current.SetSelectedGameObject(_episodeBoxArea.transform.GetChild(0).gameObject);
		}

		public void OnCancel(BaseEventData eventData)
		{
			DOTween.KillAll();
			SceneTransitionManager.TransitionToModeSelection();
		}
	}
}