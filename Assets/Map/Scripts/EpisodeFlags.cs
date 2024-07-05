using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class EpisodeFlags : ScriptableObject
{
	[SerializeField] private List<EpisodeFlagPair> _flagsList;
	public List<EpisodeFlagPair> FlagsList { get { return _flagsList; } private set { _flagsList = value; } }

#if UNITY_EDITOR
	private void OnValidate()
	{
		FlagsList = FlagsList.OrderBy(kvp => kvp.Key.Item1).ThenBy(kvp => kvp.Key.Item2).ToList();
	}

	public void ResetFlags(EpisodeData episodeData)
	{
		FlagsList.Clear();
		foreach (var episode in episodeData.DataList)
		{
			FlagsList.Add(new EpisodeFlagPair((episode.Chapter, episode.Section), false));
		}

		FlagsList = FlagsList.OrderBy(kvp => kvp.Key.Item1).ThenBy(kvp => kvp.Key.Item2).ToList();
	}
#endif
}