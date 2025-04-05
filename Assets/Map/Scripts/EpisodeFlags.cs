using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
	[CreateAssetMenu]
	public class EpisodeFlags : EncryptedScriptableObject
	{
		[SerializeField] private List<EpisodeFlagPair> _flagList;
		public List<EpisodeFlagPair> FlagList { get { return _flagList; } private set { _flagList = value; } }

		public Dictionary<(int, int), bool> FlagDict => FlagList.ToDictionary(x => x.Key, x => x.Value);

		public void SetNextFlag((int, int) episodeId)
		{
			var index = FlagList.FindIndex(x => x.Key == episodeId);
			if (index >= 0 && index < FlagList.Count - 1)
			{
				FlagList[index + 1].Value = true;
			}

			Save();
		}

#if UNITY_EDITOR
		public void ResetFlags(EpisodeData episodeData)
		{
			FlagList.Clear();
			foreach (var episode in episodeData.DataList)
			{
				FlagList.Add(new EpisodeFlagPair((episode.Chapter, episode.Section), false));
			}

			FlagList = FlagList.OrderBy(kvp => kvp.Key.Item1).ThenBy(kvp => kvp.Key.Item2).ToList();

			if (FlagList.Count > 0)
			{
				FlagList[0].Value = true;
			}

			Save();
		}
#endif
	}
}