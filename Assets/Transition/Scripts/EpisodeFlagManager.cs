using Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Transition
{
	public class EpisodeFlagManager : MonoBehaviour
	{
		[SerializeField] private EpisodeData _episodeData;
		[SerializeField] private EpisodeFlags _episodeFlags;

		private Dictionary<NovelId, (int, int)> _novelDict;
		private Dictionary<NovelId, (int, int)> NovelDict
		{
			get
			{
				if (_novelDict == null)
				{
					_novelDict = _episodeData.DataList.Where(x => x.EpisodeType == EpisodeType.Novel).ToDictionary(x => x.NovelId, x => (x.Chapter, x.Section));
				}

				return _novelDict;
			}
		}

		private Dictionary<RhythmId, (int, int)> _rhythmDict;
		private Dictionary<RhythmId, (int, int)> RhythmDict
		{
			get
			{
				if (_rhythmDict == null)
				{
					_rhythmDict = _episodeData.DataList.Where(x => x.EpisodeType == EpisodeType.Rhythm).ToDictionary(x => x.RhythmId, x => (x.Chapter, x.Section));
				}

				return _rhythmDict;
			}
		}

		public void SetNextFlag(NovelId novelId)
		{
			if (NovelDict.ContainsKey(novelId))
			{
				_episodeFlags.SetNextFlag(NovelDict[novelId]);
			}
		}

		public void SetNextFlag(RhythmId rhythmId)
		{
			if (RhythmDict.ContainsKey(rhythmId))
			{
				_episodeFlags.SetNextFlag(RhythmDict[rhythmId]);
			}
		}
	}
}