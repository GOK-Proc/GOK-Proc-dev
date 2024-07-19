using Map;
using System.Collections.Generic;
using UnityEngine;

namespace Transition
{
	public class EpisodeFlagManager : MonoBehaviour
	{
		[SerializeField] private EpisodeData _episodeData;
		[SerializeField] private EpisodeFlags _episodeFlags;

		private Dictionary<NovelId, (int, int)> _novelDict;
		private Dictionary<RhythmId, (int, int)> _rhythmDict;

		public void SetFlag(NovelId novelId, bool value)
		{
			if (_novelDict == null)
			{
				_novelDict = new Dictionary<NovelId, (int, int)>();
				foreach (var episode in _episodeData.DataList)
				{
					if (!_novelDict.ContainsKey(episode.NovelId))
					{
						_novelDict.Add(episode.NovelId, (episode.Chapter, episode.Section));
					}
				}
			}

			if (_novelDict.ContainsKey(novelId))
			{
				_episodeFlags.SetFlag(_novelDict[novelId], value);
			}
		}

		public void SetFlag(RhythmId rhythmId, bool value)
		{
			if (_rhythmDict == null)
			{
				_rhythmDict = new Dictionary<RhythmId, (int, int)>();
				foreach (var episode in _episodeData.DataList)
				{
					if (!_rhythmDict.ContainsKey(episode.RhythmId))
					{
						_rhythmDict.Add(episode.RhythmId, (episode.Chapter, episode.Section));
					}
				}
			}

			if (_rhythmDict.ContainsKey(rhythmId))
			{
				_episodeFlags.SetFlag(_rhythmDict[rhythmId], value);
			}
		}
	}
}