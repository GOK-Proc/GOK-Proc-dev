using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Map
{
	public class MapSpot : Selectable, ISubmitHandler
	{
		[SerializeField] private EpisodeData _episodeData;
		[SerializeField] private EpisodeFlags _episodeFlags;

		[SerializeField] private EpisodeBox _episodeBox;

		[SerializeField] private GameObject _episodeBoxArea;

		[SerializeField] private GameObject _selectOff;
		[SerializeField] private GameObject _selectOn;

		[SerializeField] private List<EpisodeNumber> _episodes;

		protected override void Start()
		{
			base.Start();

			var dataDict = _episodeData.DataDict;
			var flagDict = _episodeFlags.FlagDict;

			if (!flagDict.Values.Contains(true))
			{
				gameObject.SetActive(false);
				return;
			}

			Selectable preObj = null;
			foreach (var episode in _episodes)
			{
				if (!flagDict.ContainsKey((episode.Chapter, episode.Section))) continue;

				if (flagDict[(episode.Chapter, episode.Section)])
				{
					var obj = Instantiate(_episodeBox, _episodeBoxArea.transform);
					obj.SetInfo(dataDict[(episode.Chapter, episode.Section)]);

					if (preObj != null)
					{
						Navigation preNavigation = preObj.navigation;
						preNavigation.selectOnDown = obj;
						preObj.navigation = preNavigation;

						Navigation navigation = obj.navigation;
						navigation.selectOnUp = preObj;
						obj.navigation = navigation;
					}

					preObj = obj;
				}
			}
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);

			_selectOff.SetActive(false);
			_selectOn.SetActive(true);
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);

			_selectOn.SetActive(false);
			_selectOff.SetActive(true);
		}

		public void OnSubmit(BaseEventData eventData)
		{
			_episodeBoxArea.SetActive(true);
			EventSystem.current.SetSelectedGameObject(_episodeBoxArea.transform.GetChild(0).gameObject);
		}
	}
}