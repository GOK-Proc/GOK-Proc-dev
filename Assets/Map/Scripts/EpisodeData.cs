using System.Collections.Generic;
using UnityEngine;

namespace Map
{
	[CreateAssetMenu]
	public class EpisodeData : ScriptableObject
	{
		[SerializeField] private EpisodeFlags _episodeFlags;

		[SerializeField] private List<EpisodeInfomation> _dataList;
		public List<EpisodeInfomation> DataList { get { return _dataList; } }

		private Dictionary<(int, int), EpisodeInfomation> _dataDict;
		public Dictionary<(int, int), EpisodeInfomation> DataDict
		{
			get
			{
				if(_dataDict == null) InitilaizeDictionary();
				
				return _dataDict;
			}

			private set
			{
				_dataDict = value;
			}
		}

		private void InitilaizeDictionary()
		{
			DataDict = new Dictionary<(int, int), EpisodeInfomation>();

			foreach (var episode in DataList)
			{
				if (!DataDict.ContainsKey((episode.Chapter, episode.Section)))
				{
					DataDict.Add((episode.Chapter, episode.Section), episode);
				}
			}
		}

#if UNITY_EDITOR
		private HashSet<(int, int)> _episodeNumbers = new HashSet<(int, int)>();

		private void OnValidate()
		{
			_episodeNumbers.Clear();
			foreach (var item in DataList)
			{
				if (_episodeNumbers.Contains((item.Chapter, item.Section)))
				{
					Debug.LogWarning("エピソード番号に重複があります");
				}
				else
				{
					_episodeNumbers.Add((item.Chapter, item.Section));
				}
			}

			_episodeFlags.ResetFlags(this);
		}
#endif
	}
}