using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EpisodeData : ScriptableObject
{
	public Dictionary<(int, int), EpisodeInfomation> EpisodeDataDict { get; private set; } = new Dictionary<(int, int), EpisodeInfomation>();

	[SerializeField] private List<EpisodeInfomation> _episodeDataList;

#if UNITY_EDITOR
	private void OnValidate()
	{
		EpisodeDataDict.Clear();
		foreach (var item in _episodeDataList)
		{
			if (EpisodeDataDict.ContainsKey((item.Chapter, item.Section)))
			{
				Debug.LogWarning("章, 節番号に重複があります");
			}
			else
			{
				EpisodeDataDict.Add((item.Chapter, item.Section), item);
			}
		}
	}
#endif
}