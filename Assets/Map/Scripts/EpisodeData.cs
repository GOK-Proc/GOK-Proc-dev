using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
	[CreateAssetMenu]
	public class EpisodeData : ScriptableObject
	{
		[SerializeField] private EpisodeFlags _episodeFlags;

		private List<EpisodeInfomation> _preDataList;

		[SerializeField] private List<EpisodeInfomation> _dataList;
		public List<EpisodeInfomation> DataList { get { return _dataList; } }

		private Dictionary<(int, int), EpisodeInfomation> _dataDict;
		public Dictionary<(int, int), EpisodeInfomation> DataDict
		{
			get
			{
				if (_dataDict == null) _dataDict = DataList.ToDictionary(x => (x.Chapter, x.Section));

				return _dataDict;
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

			if (_preDataList != null && DataList.Count == _episodeNumbers.Count && DataList != _preDataList)
			{
				_episodeFlags.ResetFlags(this);
			}

			_preDataList = new List<EpisodeInfomation>(DataList);
		}
#endif
	}
}