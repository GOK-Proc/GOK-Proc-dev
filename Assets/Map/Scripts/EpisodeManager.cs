using System.Collections.Generic;
using UnityEngine;

public class EpisodeManager : MonoBehaviour
{
	[SerializeField] private EpisodeData _episodeData;
	[SerializeField] private EpisodeFlags _episodeFlags;

	private Dictionary<(int, int), EpisodeInfomation> _dataDict;
	private Dictionary<(int, int), bool> _flagsDict;

	public EpisodeInfomation GetInfo((int, int) episodeId)
	{
		if (_dataDict == null)
		{
			_dataDict = new Dictionary<(int, int), EpisodeInfomation>();
			foreach (var episode in _episodeData.DataList)
			{
				_dataDict.Add((episode.Chapter, episode.Section), episode);
			}
		}

		if (_dataDict.ContainsKey(episodeId))
		{
			return _dataDict[episodeId];
		}
		else
		{
			return new EpisodeInfomation();
		}
	}

	public bool GetFlag((int, int) episodeId)
	{
		if (_flagsDict == null)
		{
			_flagsDict = new Dictionary<(int, int), bool>();
			foreach (var fp in _episodeFlags.FlagsList)
			{
				_flagsDict.Add(fp.Key, fp.Value);
			}
		}

		if (_flagsDict.ContainsKey(episodeId))
		{
			return _flagsDict[episodeId];
		}
		else
		{
			return false;
		}
	}
}